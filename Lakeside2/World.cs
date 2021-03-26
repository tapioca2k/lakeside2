using Lakeside2.Editor;
using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        List<IEntity> entities;
        Player player;

        bool editing = false;
        EditingOverlay editor;

        public World(ContentManager Content, string filename=null)
        {
            this.Content = Content;

            TileMap map = new TileMap(Content, 20, 10);
            player = new Player(Content, map);
            camera = new TilemapCamera(map);
            ui = new UiSystem(Content);

            camera.setCenteringEntity(player);
            camera.centerEntity(true);

            entities = new List<IEntity>();
            entities.Add(player);

            ui.addStripeElement(new UiObjectMonitor<Player>(player, (p) =>
            {
                return p.tileLocation.ToString();
            }), 'l');
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
                }
            }
        }
        public void update(double dt)
        {
            if (!editing)
            {
                player.update(dt);
                ui.update(dt);
                camera.update(dt);
            }
            else
            {
                editor.update(dt);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (editing)
            {
                editor.draw(spriteBatch);
            }
            else
            {
                camera.draw(spriteBatch, entities);
                ui.draw(spriteBatch);
            }
        }

    }
}
