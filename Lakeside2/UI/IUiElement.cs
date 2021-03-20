using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    abstract class IUiElement
    {
        public abstract Vector2 size
        {
            get;
        }

        public bool finished
        {
            get;
            set;
        }

        public abstract void draw(SpriteBatch spriteBatch, Vector2 location);

    }
}
