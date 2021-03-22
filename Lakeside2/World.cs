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
        UiSystem ui;

        TilemapCamera camera;
        TileMap map;

        Player player;

        public World(ContentManager Content)
        {
            map = new TileMap(Content, 20, 10);
            player = new Player(Content, map);
            camera = new TilemapCamera(map, player);
            ui = new UiSystem(Content);

            ui.addStripeElement(new UiPlayerLocationDisplay(Fonts.get("Arial"), player), 'l');
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
            camera.draw(spriteBatch);
            player.draw(spriteBatch);
            ui.draw(spriteBatch);
        }

    }
}
