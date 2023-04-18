using Lakeside2.UI;
using Lakeside2.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using Lakeside2.Serialization;

namespace Lakeside2
{
    public class LuaAPI
    {
        Game1 game;
        World world;
        UiSystem ui;
        ContentManager Content;
        public Player player;

        public static Vector2 makeVector2(int x, int y)
        {
            return new Vector2(x, y);
        }

        public static Color makeColor(int r, int g, int b)
        {
            return new Color(r, g, b);
        }

        public LuaAPI(Game1 game, World world, UiSystem ui, Player player, ContentManager Content)
        {
            this.game = game;
            this.world = world;
            this.ui = ui;
            this.player = player;
            this.Content = Content;
        }

        public Entity getEntity(string name)
        {
            return world.map.getNPC(name);
        }

        public void pushUiElement(UiElement element, int x, int y)
        {
            ui.pushElement(element, makeVector2(x, y));
        }

        public void queueMove(Entity entity, Vector2 direction)
        {
            // do we actually want collision detection here?
            if (world.map.checkCollision(entity.getTileLocation() + direction))
            {
                entity.queueMove(direction);
            }
        }

        public void playSfx(string filename)
        {
            MusicManager.playSfx(filename);
        }

        public void changeMap(string filename)
        {
            TileMap newMap = SerializableMap.Load(Content, filename);
            if (newMap != null)
            {
                pushUiElement(new UiScreenFade(() =>
                {
                    world.setMap(newMap);
                }), 0, 0);
            }
        }

        public void changeMap(string filename, Vector2 location)
        {
            TileMap newMap = SerializableMap.Load(Content, filename);
            if (newMap != null)
            {
                pushUiElement(new UiScreenFade(() =>
                {
                    world.setMap(newMap, location);
                }), 0, 0);
            }
        }

        public void goToOverworld()
        {
            game.goToOverworld(player, world.map.filename);
        }

        public void pushState(IGameState state)
        {
            game.pushState(state, true);
        }

        /// <summary>
        /// Sets an entity to follow the player as they walk around
        /// </summary>
        /// <param name="follower">Entity to follow player</param>
        public void followPlayer(Entity follower)
        {
            player.setFollower(follower);
        }
        /// <summary>
        /// Return true if a given entity is currently following the player
        /// </summary>
        /// <param name="follower">The entity to check</param>
        /// <returns></returns>
        public bool isFollowingPlayer(Entity follower)
        {
            return player.getFollower() == follower;
        }

        /// <summary>
        /// Creates and queues a ScriptChain for execution
        /// </summary>
        /// <param name="elements">The nodes, in order, to be a part of the chain. These should be created using the other functions in this object</param>
        public void makeChain(params ScriptNode[] elements)
        {
            // filter out any null elements, preserving order
            List<ScriptNode> nonNull = new List<ScriptNode>();
            for (int i = 0; i < elements.Length; i++) 
                if (elements[i] != null) nonNull.Add(elements[i]);

            for (int i = 1; i < nonNull.Count; i++)
            {
                nonNull[i - 1].next = nonNull[i];
            }

            world.queueScript(new ScriptChain(elements[0]));
        }

        /// <summary>
        /// Create a script node that represents a basic text box
        /// </summary>
        /// <param name="text">The main text</param>
        /// <returns>a UiNode with the text box</returns>
        public ScriptNode SDialog(string text)
        {
            return new UiNode(ui, new UiTextBox(text, true));
        }

        /// <summary>
        /// Create a script node that represents a text box with spoken words from an NPC
        /// </summary>
        /// <param name="npc">The speaking NPC</param>
        /// <param name="text">The main text</param>
        /// <returns>a UiNode with the text box</returns>
        public ScriptNode SNPCDialog(NPC npc, string text)
        {
            return new UiNode(ui, new UiNPCTextBox(npc, text));
        }

        /// <summary>
        /// Create a script node that represents a basic text box with a callback function
        /// </summary>
        /// <param name="text">The main text</param>
        /// <param name="callback">The callback function, executed after the text box is closed</param>
        /// <returns>a UiNode with the text box</returns>
        public ScriptNode SDialog(string text, LuaFunction callback)
        {
            return new UiNode(ui, new UiTextBox(text, true).addCallback(element =>
            {
                callback.Call(new object[0]);
            }));
        }

        /// <summary>
        /// Create a script node that represents a text box with two options
        /// </summary>
        /// <param name="text">The main text</param>
        /// <param name="option1">Text of option 1</param>
        /// <param name="option2">Text of option 2</param>
        /// <param name="f1">Lua function executed if option 1 is selected</param>
        /// <param name="f2">Lua function executed if option 2 is selected</param>
        /// <returns>a UiNode with the text box</returns>
        public ScriptNode SBranch(string text, string option1, string option2, LuaFunction f1, LuaFunction f2)
        {
            return new UiNode(ui, new UiOptionBox(Content, text, option1, option2).addCallback(element =>
            {
                UiOptionBox option = (UiOptionBox)element;
                if (option.selected == 0 && f1 != null) f1.Call(new object[0]);
                else if (option.selected == 1 && f2 != null) f2.Call(new object[0]);
            }));
        }

        /// <summary>
        /// Create a script node that executes a Lua function
        /// </summary>
        /// <param name="func">The function to execute</param>
        /// <returns>a FunctionNode with the function</returns>
        public ScriptNode SFunction(LuaFunction func)
        {
            return new FunctionNode(func);
        }

        /// <summary>
        /// Create a script node that moves an NPC up/down a fixed number of tiles, ignoring collision or pathfinding
        /// </summary>
        /// <param name="entity">The NPC to be moved</param>
        /// <param name="x">X component of movement</param>
        /// <param name="y">Y component of movement</param>
        /// <returns>a MoveNode with the moves</returns>
        public ScriptNode SMove(NPC entity, int x, int y)
        {
            if (entity == null || (x == 0 && y == 0))
            {
                return null;
            }
            List<Vector2> moves = new List<Vector2>();
            for (int i = 0; i < x; i++) moves.Add(new Vector2(1, 0));
            for (int i = 0; i > x; i--) moves.Add(new Vector2(-1, 0));
            for (int i = 0; i < y; i++) moves.Add(new Vector2(0, 1));
            for (int i = 0; i > y; i--) moves.Add(new Vector2(0, -1));

            return new MoveNode(entity, moves.ToArray());
        }

        /// <summary>
        /// Create a script node that moves an NPC with pathfinding
        /// </summary>
        /// <param name="entity">The NPC to be moved</param>
        /// <param name="tilePosition">The location they should move to</param>
        /// <returns>a MoveNode with the path. Empty if a path could not be found</returns>
        public ScriptNode SMove(NPC entity, Vector2 tilePosition)
        {
            if (entity == null || tilePosition == entity.getTileLocation())
            {
                return null;
            }
            List<Vector2> rawPath = world.map.computePath(
                entity.getTileLocation(), 
                tilePosition, 
                player.getTileLocation());
            List<Vector2> path = new List<Vector2>();
            for (int i = 1; i < rawPath.Count; i++) // compute directions from raw tile positions
            {
                path.Add(new Vector2(rawPath[i].X - rawPath[i - 1].X, rawPath[i].Y - rawPath[i - 1].Y));
            }
            return new MoveNode(entity, path.ToArray());
        }

        /// <summary>
        /// Create a script node that changes the song being played
        /// </summary>
        /// <param name="songName">The name of the song to be played</param>
        /// <returns>ActionNode that starts the song</returns>
        public ScriptNode SMusic(string songName)
        {
            return new ActionNode(() =>
            {
                MusicManager.playSong(songName);
            });
        }

        /// <summary>
        /// Create a script node that blocks on playing a sound effect
        /// </summary>
        /// <param name="effect">The name of the sound effect to be played</param>
        /// <returns>MultiplexNode containing the sound effect play and accompanying delay</returns>
        public ScriptNode SSfx(string effect)
        {
            double length = MusicManager.getSfxLength(effect);
            return new MultiplexNode(new ActionNode(() =>
            {
                MusicManager.playSfx(effect);
            }), new DelayNode(length));
        }

        /// <summary>
        ///  Create a script node for running multiple ScriptNodes simultaneously
        /// </summary>
        /// <param name="nodes">The nodes to be multiplexed</param>
        /// <returns>MultiplexNode containing the parameter nodes</returns>
        public ScriptNode SMultiplex(params ScriptNode[] nodes)
        {
            return new MultiplexNode(nodes);
        }

    }
}
