using Lakeside2.Editor;
using Lakeside2.Scripting;
using Lakeside2.Serialization;
using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Lakeside2.InputBindings;

namespace Lakeside2
{
    public class World : IGameState
    {
        public const int PORTAL_WIDTH = Game1.INTERNAL_WIDTH;
        public const int HALF_PORTAL_WIDTH = PORTAL_WIDTH / 2;
        public const int PORTAL_HEIGHT = Game1.INTERNAL_HEIGHT - 16 - 4;
        public const int HALF_PORTAL_HEIGHT = PORTAL_HEIGHT / 2;

        public Color background => map.color;

        public Game1 game;
        ContentManager Content;

        public UiSystem ui;

        public TilemapCamera camera;
        public TileMap map
        {
            get
            {
                return camera.getMap();
            }
        }

        List<Entity> entities;
        Player player;

        bool editing = false;
        WorldEditor editor;

        public Lua lua;

        Queue<ScriptChain> scripts;

        public World(ContentManager Content, Game1 game, Player player, string filename)
        {
            this.game = game;
            this.Content = Content;
            this.scripts = new Queue<ScriptChain>();
            this.ui = new UiSystem(Content);

            lua = new Lua();
            lua.LoadCLRPackage();

            this.player = player;
            this.player.setWorld(this);

            lua["l"] = new LuaAPI(game, this, ui, this.player, Content);
            lua["player"] = this.player;
            lua.DoString(@"
                import ('Lakeside2', 'Lakeside2')
                import ('Lakeside2', 'Lakeside2.UI')
                import ('Lakeside2', 'Lakeside2.Scripting')");

            camera = new TilemapCamera(null);
            TileMap map;
            if (filename == null) map = new TileMap(Content, 20, 10);
            else map = SerializableMap.Load(Content, filename);
            setMap(map);
            camera.setCenteringEntity(this.player);
        }

        public void setMap(TileMap map)
        {
            setMap(map, map.playerStart);
        }

        public void setMap(TileMap map, Point location)
        {
            camera.setMap(map);
            player.setTileLocation(location);
            resetEntities();
            camera.forceUpdate();

            LuaScript startScript = map.getScript(map.playerStart);
            if (startScript != null) startScript.execute(player, lua);
        }

        // reset entities list
        void resetEntities()
        {
            entities = new List<Entity>();
            entities.Add(player);
            entities.AddRange(map.npcs);
        }

        public void queueScript(ScriptChain chain)
        {
            scripts.Enqueue(chain);
        }

        public void onInput(InputHandler input)
        {
            // normal game controls
            if (!editing)
            {
                bool interacting = ui.onInput(input);
                if (!interacting && scripts.Count == 0)
                {
                    player.onInput(input);

                    if (input.isCommandPressed(Bindings.Start))
                    {
                        ui.pushElement(new UiPauseMenu(game, Content, player, map.filename, false), 
                            new Vector2(Tile.TILE_SIZE, Tile.TILE_SIZE));
                    }

                }
            }
            else
            {
                editor.onInput(input);
            }

            // enter/exit editing mode
            if (input.isKeyPressed(Keys.F1))
            {
                editing = !editing;
                if (editing)
                {
                    editor = new WorldEditor(Content, this);
                }
                else
                {
                    editor = null;
                    camera.setCenteringEntity(player); // editing mode stole this
                    resetEntities(); // reload entities for ones created in the editor
                }
            }
        }

        public void update(double dt)
        {
            if (!editing)
            {
                ui.update(dt);
                entities.ForEach((entity) =>
                {
                    entity.update(dt);
                });
                camera.update(dt);
                if (scripts.Count > 0)
                {
                    scripts.Peek().update(dt);
                    if (scripts.Peek().finished) scripts.Dequeue();
                }
            }
            else
            {
                editor.update(dt);
            }

            // sort entities (for perspective)
            for (int i = 1; i < entities.Count; i++)
            {
                Entity key = entities[i];
                int j = i - 1;
                while (j >= 0 && entities[j].getLocation().Y > key.getLocation().Y)
                {
                    entities[j + 1] = entities[j];
                    j = j - 1;
                }
                entities[j + 1] = key;
            }
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.drawRectangle(new Vector2(Game1.INTERNAL_WIDTH, Game1.INTERNAL_HEIGHT), map.color);
            if (editing)
            {
                editor.draw(wrapper);
            }
            else
            {
                camera.draw(wrapper, entities);
                ui.draw(wrapper);
            }
        }

    }
}
