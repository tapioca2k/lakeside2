using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI
{
    public enum StripePosition
    {
        Left,
        Right,
        Center
    };

    class UiStripe
    {
        public const int STRIPE_HEIGHT = Tile.TILE_SIZE + 4;
        public const int STRIPE_START = Game1.INTERNAL_HEIGHT - STRIPE_HEIGHT;

        public UiElement leftElement, centerElement, rightElement;
        Vector2 stripeLeft, stripeCenter, stripeRight;

        public UiStripe(ContentManager Content)
        {
            centerElement = new UiTexture(Content, "logo-default");
        }

        public UiStripe()
        {
        }

        
        public void addElement(UiElement element, StripePosition location)
        {
            if (location == StripePosition.Left)
            {
                leftElement = element;
                stripeLeft = new Vector2(0, STRIPE_START);
            }
            else if (location == StripePosition.Right)
            {
                rightElement = element;
                stripeRight = new Vector2(Game1.INTERNAL_WIDTH - rightElement.size.X, STRIPE_START);
            }
            else if (location == StripePosition.Center)
            {
                centerElement = element;
                stripeCenter = new Vector2(Math.Max(0, (int)(Game1.INTERNAL_WIDTH - centerElement.size.X) / 2),
                    Math.Max(STRIPE_START, (int)(Game1.INTERNAL_HEIGHT - centerElement.size.Y) / 2));
            }
        }
        
        public void update(double dt)
        {
            if (leftElement != null) leftElement.update(dt);
            if (rightElement != null)
            {
                rightElement.update(dt);
                stripeRight = new Vector2(Game1.INTERNAL_WIDTH - rightElement.size.X, STRIPE_START);
            }
            if (centerElement != null)
            {
                centerElement.update(dt);
                stripeCenter = new Vector2(Math.Max(0, (int)(Game1.INTERNAL_WIDTH - centerElement.size.X) / 2),
                    Math.Max(STRIPE_START, (int)(Game1.INTERNAL_HEIGHT - centerElement.size.Y) / 2));
            }

        }


        public void draw(SBWrapper wrapper)
        {
            SBWrapper stripeSpace = new SBWrapper(wrapper, new Vector2(0, STRIPE_START));
            stripeSpace.drawRectangle(new Vector2(Game1.INTERNAL_WIDTH, STRIPE_HEIGHT), Color.White);

            if (leftElement != null) leftElement.draw(new SBWrapper(wrapper, stripeLeft));
            if (centerElement != null) centerElement.draw(new SBWrapper(wrapper, stripeCenter));
            if (rightElement != null) rightElement.draw(new SBWrapper(wrapper, stripeRight));
        }
    }
}
