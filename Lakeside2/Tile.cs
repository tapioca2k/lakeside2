using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lakeside2
{
    public class Tile : IDrawable
    {
        const string TILES = "tiles/";
        public const int TILE_SIZE = 16;

        public string filename;
        public bool collision;

        Texture2D texture;

        public Tile(ContentManager Content, string filename)
        {
            this.filename = filename;
            this.collision = true;
            texture = Content.Load<Texture2D>(TILES + filename);
            if (texture.Width != texture.Height || texture.Width != TILE_SIZE)
            {
                throw new Exception("Tile " + filename + " incorrect size");
            }
        }
        
        // for copying tiles
        public Tile(Tile other)
        {
            this.filename = other.filename;
            this.collision = other.collision;
            this.texture = other.texture;
        }

        public void setTexture(ContentManager Content, string filename)
        {
            texture = Content.Load<Texture2D>(TILES + filename);
            this.filename = filename;
        }

        public void draw(SBWrapper wrapper, Vector2 location)
        {
            wrapper.draw(texture, location);
        }
    }
}
