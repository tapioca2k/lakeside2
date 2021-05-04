using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    public abstract class UiElement
    {
        public abstract Vector2 size
        {
            get;
        }

        public bool finished
        {
            get
            {
                return done;
            }
            set
            {
                done = value;
                if (done)
                {
                    this.callbacks.ForEach(action => action.Invoke(this));
                }
            }
        }

        private bool done;
        private List<Action<UiElement>> callbacks = new List<Action<UiElement>>();
        private Color background = Color.Transparent;
        private bool border = false;
        protected UiSystem system;

        public virtual void setUiSystem(UiSystem system)
        {
            this.system = system;
        }

        public UiElement addCallback(Action<UiElement> callback)
        {
            this.callbacks.Add(callback);
            return this;
        }

        public void setBackground(Color color, bool border)
        {
            background = color;
            this.border = border;
        }

        public virtual void update(double dt)
        {
        }

        public virtual void onInput(InputHandler input)
        {
        }

        public abstract void draw(SBWrapper wrapper);

        protected void drawBackground(SBWrapper wrapper)
        {
            if (border) wrapper.drawRectangle(size + new Vector2(2, 2), Color.Black, wrapper.location - Vector2.One);
            wrapper.drawRectangle(size, background);
        }

    }
}
