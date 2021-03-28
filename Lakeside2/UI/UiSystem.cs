using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiSystem
    {
        UiStripe stripe;
        List<UiElement> stack;
        List<Vector2> locations;

        public UiSystem(ContentManager Content)
        {
            stripe = new UiStripe(Content);
            stack = new List<UiElement>();
            locations = new List<Vector2>();
        }

        // create a UiSystem with no logo on the stripe (for edit mode)
        public UiSystem()
        {
            stripe = new UiStripe();
            stack = new List<UiElement>();
            locations = new List<Vector2>();
        }

        public void update(double dt)
        {
            stripe.update(dt);
            if (stack.Count > 0)
            {
                UiElement top = stack[stack.Count - 1];
                top.update(dt);
                if (top.finished)
                {
                    stack.RemoveAt(stack.Count - 1);
                    locations.RemoveAt(locations.Count - 1);
                }
            }
        }

        public bool onInput(InputHandler input)
        {
            if (stack.Count > 0)
            {
                stack[stack.Count - 1].onInput(input);
                return true;
            }
            return false;
        }

        public void pushElement(UiElement element, Vector2 location)
        {
            element.setUiSystem(this);
            stack.Add(element);
            locations.Add(location);
        }

        public void addStripeElement(UiElement element, char location)
        {
            stripe.addElement(element, location);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            SBWrapper wrapper = new SBWrapper(spriteBatch);
            for (int i = 0; i < stack.Count; i++)
            {
                stack[i].draw(wrapper.setOrigin(locations[i]));
            }

            stripe.draw(spriteBatch);
        }

    }
}
