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
        Color background = Color.Transparent;
        protected UiSystem system;

        public void setUiSystem(UiSystem system)
        {
            this.system = system;
        }

        public UiElement addCallback(Action<UiElement> callback)
        {
            this.callbacks.Add(callback);
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

        public abstract void draw(SBWrapper wrapper);

        // TODO figure out why C# isn't letting me do the inheritance I want here...
        protected void drawBackground(SBWrapper wrapper)
        {
            wrapper.drawRectangle(size, background);
        }

    }
}
