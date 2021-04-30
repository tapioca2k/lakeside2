using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.WorldMap
{
    class OWPlayer
    {
        public Player p;
        Texture2D texture;
        Vector2 location;

        public Vector2 feet
        {
            get
            {
                return new Vector2(location.X + (texture.Width / 2), location.Y + texture.Height);
            }
            set
            {
                location = new Vector2(value.X - (texture.Width / 2), value.Y - texture.Height);
            }
        }

        public OWPlayer(ContentManager Content, Player p, Vector2 location)
        {
            texture = Content.Load<Texture2D>("map/mapman");
            this.p = p;
            this.location = location;
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(texture, location);
        }
    }
}
