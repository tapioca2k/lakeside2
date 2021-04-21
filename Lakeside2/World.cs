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
        EditingOverlay editor;

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
            camera.centerEntity(true);

            ui.addStripeElement(new UiObjectMonitor<Player>(this.player, (p) =>
            {
                return p.getTileLocation().ToString();
            }), 'l');
        }

        public void setMap(TileMap map)
        {
            camera.setMap(map);
            player.setTileLocation(map.playerStart);
            resetEntities();
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
            if (!editing)
            {
                bool interacting = ui.onInput(input);
                if (!interacting && scripts.Count == 0)
                {
                    player.onInput(input);

                    // other game controls go here...

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
                    editor = new EditingOverlay(Content, this);
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
