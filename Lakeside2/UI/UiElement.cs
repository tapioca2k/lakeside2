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

        private bool done;
        public bool finished
        {
            get
            {
                return done;
            }
            set
            {
                done = value;
                if (done && callback != null)
                {
                    this.callback.Invoke(this);
                }
            }
        }

        private Func<UiElement, bool> callback;
        Color background = Color.Transparent;

        public UiElement addCallback(Func<UiElement, bool> callback)
        {
            this.callback = callback;
            return this;
        }

        public void setBackground(Color color)
        {
            background = color;
        }

        public virtual void update(double dt)
        {
        }

        public virtual void onInput(InputHandler input)
        {
        }

        public abstract void draw(SpriteBatch spriteBatch, Vector2 location);

        // TODO figure out why C# isn't letting me do the inheritance I want here...
        protected void drawBackground(SpriteBatch spriteBatch, Vector2 location)
        {
            spriteBatch.Draw(Game1.WHITE_PIXEL, new Rectangle(location.ToPoint(), size.ToPoint()), background);
        }

    }
}
