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
        UiStripe stripe;
        
        //UiSystem ui;

        TilemapCamera camera;
        TileMap map;

        public World(ContentManager Content)
        {
            map = new TileMap(Content, 20, 10);
            camera = new TilemapCamera(map);
            stripe = new UiStripe(Content);
        }

        public void update(double dt)
        {
            camera.update(dt);
        }

        public void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.W)) camera.tileMoveY(-1);
            if (input.isKeyPressed(Keys.A)) camera.tileMoveX(-1);
            if (input.isKeyPressed(Keys.S)) camera.tileMoveY(1);
            if (input.isKeyPressed(Keys.D)) camera.tileMoveX(1);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            camera.draw(spriteBatch);
            // ui.draw(spriteBatch);

            stripe.draw(spriteBatch);
        }
    }
}
