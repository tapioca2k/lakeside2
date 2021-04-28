using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    public class UiSystem
    {
        UiStripe stripe;
        List<UiElement> stack;
        List<Vector2> locations;
        public bool hasStripe => stripe != null;

        public UiSystem(ContentManager Content)
        {
            stripe = new UiStripe(Content);
            stack = new List<UiElement>();
            locations = new List<Vector2>();
        }

        // create a UiSystem with no logo on the stripe (for edit mode)
        public UiSystem(bool hasStripe = true)
        {
            if (hasStripe) stripe = new UiStripe();
            stack = new List<UiElement>();
            locations = new List<Vector2>();
        }

        public void update(double dt)
        {
            if (hasStripe) stripe.update(dt);
            if (stack.Count > 0)
            {
                UiElement top = stack[stack.Count - 1];
                top.update(dt);
                while (stack.Count > 0 && stack[stack.Count - 1].finished)
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
            if (hasStripe) stripe.addElement(element, location);
        }

        public void draw(SBWrapper wrapper)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                stack[i].draw(new SBWrapper(wrapper, locations[i]));
            }
            if (hasStripe) stripe.draw(wrapper);
        }

    }
}
