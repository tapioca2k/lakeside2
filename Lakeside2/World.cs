using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
        TileMap map;

        List<IEntity> entities;
        Player player;

        public World(ContentManager Content)
        {
            this.Content = Content;

            map = SerializableMap.Load(Content, "default.txt");
            player = new Player(Content, map);
            camera = new TilemapCamera(map, player);
            ui = new UiSystem(Content);
            camera.centerPlayer(true);

            entities = new List<IEntity>();
            entities.Add(player);

            ui.addStripeElement(new UiObjectMonitor<Player>(player, (p) =>
            {
                return p.tileLocation.ToString();
            }), 'l');
        }

        public void update(double dt)
        {
            player.update(dt);
            camera.update(dt);
            ui.update(dt);
        }

        public void onInput(InputHandler input)
        {
            player.onInput(input);
            ui.onInput(input);


            // DEBUG attempt to save the current TileMap
            if (input.isKeyPressed(Keys.B))
            {
                SerializableMap.Save(Content, map, "DefaultMap.txt");
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            camera.draw(spriteBatch, entities);
            ui.draw(spriteBatch);
        }

    }
}
