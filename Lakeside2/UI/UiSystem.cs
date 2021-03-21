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

        public void onInput(InputHandler input)
        {
            if (stack.Count > 0)
            {
                stack[stack.Count - 1].onInput(input);
            }
        }

        public void pushElement(UiElement element, Vector2 location)
        {
            stack.Add(element);
            locations.Add(location);
        }

        public void addStripeElement(UiElement element, char location)
        {
            stripe.addElement(element, location);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                stack[i].draw(spriteBatch, locations[i]);
            }

            stripe.draw(spriteBatch);
        }

    }
}
