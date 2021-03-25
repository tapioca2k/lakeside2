using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Editor
{
    class Cursor : IEntity
    {
        Texture2D texture;
        Vector2 location;

        public Vector2 getLocation()
        {
            return location;
        }

        public Vector2 getTileLocation()
        {
            return new Vector2(location.X / 16, location.Y / 16);
        }

        public Cursor(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("cursor");
            location = Vector2.Zero;
        }

        public void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.W)) location.Y -= Tile.TILE_SIZE;
            else if (input.isKeyPressed(Keys.A)) location.X -= Tile.TILE_SIZE;
            else if (input.isKeyPressed(Keys.S)) location.Y += Tile.TILE_SIZE;
            else if (input.isKeyPressed(Keys.D)) location.X += Tile.TILE_SIZE;
        }

        public void draw(SpriteBatch spriteBatch, TilemapCamera camera)
        {
            spriteBatch.Draw(texture, camera.worldToScreen(location), Color.White);
        }
    }
}
