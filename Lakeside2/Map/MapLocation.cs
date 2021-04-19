using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Map
{
    class MapLocation
    {
        Texture2D texture;
        public string filename;
        Vector2 location;

        public Vector2 center
        {
            get
            {
                return new Vector2(location.X + (texture.Width / 2), location.Y + (texture.Height / 2));
            }
            set
            {
                location = new Vector2(value.X - (texture.Width / 2), value.Y - (texture.Height / 2));
            }
        }

        public MapLocation(ContentManager Content, string filename, Vector2 location)
        {
            texture = Content.Load<Texture2D>("map/mapmarker");
            this.filename = filename;
            this.location = location;
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(texture, location);
        }
    }
}
