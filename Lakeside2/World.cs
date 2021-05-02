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

namespace Lakeside2
{
    public class World : IGameState
    {
        public const int PORTAL_WIDTH = Game1.INTERNAL_WIDTH;
        public const int HALF_PORTAL_WIDTH = PORTAL_WIDTH / 2;
        public const int PORTAL_HEIGHT = Game1.INTERNAL_HEIGHT - 16 - 4;
        public const int HALF_PORTAL_HEIGHT = PORTAL_HEIGHT / 2;

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

            lua["l"] = new LuaAPI(this, ui, this.player, Content);
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

            /*
            ui.addStripeElement(new UiObjectMonitor<Player>(this.player, (p) =>
            {
                return p.getTileLocation().ToString();
            }), StripePosition.Left);
            */

            ui.addStripeElement(new UiClock(), StripePosition.Left);
        }

        public void setMap(TileMap map)
        {
            setMap(map, map.playerStart);
        }

        public void setMap(TileMap map, Vector2 location)
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

                    if (input.isKeyPressed(Keys.Escape))
                    {
                        ui.pushElement(new UiList(Content, new string[4] { "Resume", "Save", "Options", "Quit" }).addCallback(element =>
                        {
                            UiList list = (UiList)element;
                            switch (list.selected)
                            {
                                case 1: // save
                                    {
                                        ui.pushElement(new UiSavePicker(Content, true).addCallback(element2 =>
                                        {
                                            UiSavePicker savePicker = (UiSavePicker)element2;
                                            string filename = savePicker.selectedString;
                                            if (filename == UiSavePicker.CREATE_NEW_FILE)
                                                filename = "save" + savePicker.GetHashCode(); // create new file name
                                            SaveGame.Save(filename + ".json", player, map.filename, false);
                                        }), new Vector2(Tile.TILE_SIZE * 10, Tile.TILE_SIZE));
                                        break;
                                    }
                                case 2: // TODO options
                                    {
                                        break;
                                    }
                                case 3: // TODO quit
                                    {
                                        break;
                                    }
                            }
                        }), new Vector2(Tile.TILE_SIZE, Tile.TILE_SIZE));
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
