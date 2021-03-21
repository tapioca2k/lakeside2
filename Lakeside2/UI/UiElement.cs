using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    abstract class UiElement
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

        public virtual void update(double dt)
        {
        }

        public virtual void onInput(InputHandler input)
        {
        }

        public abstract void draw(SpriteBatch spriteBatch, Vector2 location);

    }
}
