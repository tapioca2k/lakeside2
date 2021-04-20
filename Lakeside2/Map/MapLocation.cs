using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Lakeside2.Map
{
    public class MapLocation
    {
        Texture2D texture;
        public string filename { get; set; }
        public Vector2 location { get; set; }

        [JsonIgnore]
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
            this.filename = filename;
            this.location = location;
            load(Content);
        }

        public MapLocation()
        {
        }

        public void load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("map/mapmarker");
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(texture, location);
        }
    }
}
