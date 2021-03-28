using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class Tile
    {
        const string TILE_LOCATION = "tiles/";
        public const int TILE_SIZE = 16;

        public string filename;
        public bool collision;
        Texture2D texture;

        public Tile(ContentManager Content, string filename)
        {
            this.filename = filename;
            this.collision = true;
            texture = Content.Load<Texture2D>(TILE_LOCATION + filename);
            if (texture.Width != texture.Height || texture.Width != TILE_SIZE)
            {
                throw new Exception("Tile " + filename + " incorrect size");
            }
        }

        public void setTexture(ContentManager Content, string filename)
        {
            texture = Content.Load<Texture2D>(TILE_LOCATION + filename);
            this.filename = filename;
        }

        public void draw(SpriteBatch spriteBatch, Vector2 location)
        {
            spriteBatch.Draw(texture, location, Color.White);
        }

        public void draw(SBWrapper wrapper, Vector2 location)
        {
            wrapper.draw(texture, location);
        }
    }
}
