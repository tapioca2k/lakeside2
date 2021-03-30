using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lakeside2
{
    class Tile
    {
        const string TILE_LOCATION = "tiles/";
        public const int TILE_SIZE = 16;

        public string filename;
        public bool collision;

        public LuaScript script;

        Texture2D texture;

        public Tile(ContentManager Content, string filename)
        {
            this.filename = filename;
            this.collision = true;
            this.script = null;
            texture = Content.Load<Texture2D>(TILE_LOCATION + filename);
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
            this.script = other.script;
        }

        public void setTexture(ContentManager Content, string filename)
        {
            texture = Content.Load<Texture2D>(TILE_LOCATION + filename);
            this.filename = filename;
        }

        public void setScript(string filename)
        {
            script = new LuaScript(filename);
            if (!script.loaded) script = null; // not a valid script
        }

        public void draw(SBWrapper wrapper, Vector2 location)
        {
            wrapper.draw(texture, location);
        }
    }
}
