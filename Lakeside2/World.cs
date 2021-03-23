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


        UiSystem ui;

        TilemapCamera camera;
        TileMap map;

        List<IEntity> entities;
        Player player;

        public World(ContentManager Content)
        {
            map = new TileMap(Content, 20, 10);
            player = new Player(Content, map);
            camera = new TilemapCamera(map, player);
            ui = new UiSystem(Content);
            camera.centerPlayer(true);

            entities = new List<IEntity>();
            entities.Add(player);

            ui.addStripeElement(new UiObjectMonitor(player, (player) =>
            {
                Player p = (Player)player;
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
        }

        public void draw(SpriteBatch spriteBatch)
        {
            camera.draw(spriteBatch, entities);
            ui.draw(spriteBatch);
        }

    }
}
