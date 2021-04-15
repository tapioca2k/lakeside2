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

namespace Lakeside2
{
    class LuaAPI
    {
        World world;
        UiSystem ui;
        public Player player { get; set; }
        ContentManager Content;

        public static Vector2 makeVector2(int x, int y)
        {
            return new Vector2(x, y);
        }

        public static Color makeColor(int r, int g, int b)
        {
            return new Color(r, g, b);
        }

        public LuaAPI(World world, UiSystem ui, Player player, ContentManager Content)
        {
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
            Game1.music.playSfx(filename);
        }

        public void loadMap(string filename)
        {
            TileMap newMap = SerializableMap.Load(Content, filename);
            if (newMap != null)
            {
                world.setMap(newMap);
            }
        }

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

        public ScriptNode SDialog(string text)
        {
            return new UiNode(ui, new UiTextBox(text));
        }

        public ScriptNode SDialog(string text, LuaFunction callback)
        {
            return new UiNode(ui, new UiTextBox(text).addCallback(element =>
            {
                callback.Call(new object[0]);
            }));
        }

        public ScriptNode SBranch(string text, string option1, string option2, LuaFunction f1, LuaFunction f2)
        {
            return new UiNode(ui, new UiOptionBox(Content, text, option1, option2).addCallback(element =>
            {
                UiOptionBox option = (UiOptionBox)element;
                if (option.selected == 0) f1.Call(new object[0]);
                else if (option.selected == 1) f2.Call(new object[0]);
            }));
        }

        public ScriptNode SFunction(LuaFunction func)
        {
            return new FunctionNode(func);
        }

        // basic movement x tiles left/right, y tiles up/down
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

        // path finding move to specific tile
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

    }
}
