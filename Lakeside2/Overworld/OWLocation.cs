using Lakeside2.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace Lakeside2.WorldMap
{
    public class OWLocation
    {
        Texture2D texture;
        public string name;

        public string filename { get; set; }
        public Point location { get; set; }


        [JsonIgnore]
        public Vector2 center
        {
            get
            {
                return new Vector2(location.X + (texture.Width / 2), location.Y + (texture.Height / 2));
            }
            set
            {
                location = new Point((int)value.X - (texture.Width / 2), (int)value.Y - (texture.Height / 2));
            }
        }

        public OWLocation(ContentManager Content, string filename, Point location)
        {
            this.filename = filename;
            this.location = location;
            load(Content);
        }

        public OWLocation()
        {
        }

        public void load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("map/mapmarker");
            name = SerializableMap.Load(Content, filename).name;
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(texture, location);
        }
    }
}
