using Lakeside2.Editor;
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
    class World
    {
        public const int PORTAL_WIDTH = Game1.INTERNAL_WIDTH;
        public const int HALF_PORTAL_WIDTH = PORTAL_WIDTH / 2;
        public const int PORTAL_HEIGHT = Game1.INTERNAL_HEIGHT - 16 - 4;
        public const int HALF_PORTAL_HEIGHT = PORTAL_HEIGHT / 2;

        ContentManager Content;

        UiSystem ui;

        TilemapCamera camera;
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

        Lua lua;


        public World(ContentManager Content, string filename=null)
        {
            this.Content = Content;

            TileMap map = new TileMap(Content, 20, 10);
            ui = new UiSystem(Content);

            lua = new Lua();
            lua.LoadCLRPackage();
            lua["api"] = new LuaAPI(this, ui, player);
            lua.DoString(@"
                import ('Lakeside2', 'Lakeside2')
                import ('Lakeside2', 'Lakeside2.UI')
                import ('Lakeside2', 'Lakeside2.UI.Scripting')");

            player = new Player(Content, this, lua);
            camera = new TilemapCamera(map);

            camera.setCenteringEntity(player);
            camera.centerEntity(true);

            resetEntities();

            ui.addStripeElement(new UiObjectMonitor<Player>(player, (p) =>
            {
                return p.getTileLocation().ToString();
            }), 'l');
        }

        void resetEntities()
        {
            entities = new List<Entity>();
            entities.Add(player);
            entities.AddRange(map.npcs);
        }

        public void onInput(InputHandler input)
        {
            if (!editing)
            {
                bool interacting = ui.onInput(input);
                if (!interacting) player.onInput(input);
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
                    editor = new EditingOverlay(Content, camera);
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
                camera.update(dt);
                entities.ForEach((entity) =>
                {
                    entity.update(dt);
                });
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
