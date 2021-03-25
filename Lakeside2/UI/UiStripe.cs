using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiStripe
    {
        public const int STRIPE_HEIGHT = Tile.TILE_SIZE + 4;
        public const int STRIPE_START = Game1.INTERNAL_HEIGHT - STRIPE_HEIGHT;
        Texture2D logo;
        Vector2 logoPosition;

        public UiElement leftElement, rightElement;
        Vector2 stripeLeft, stripeRight;

        public UiStripe(ContentManager Content)
        {
            try
            {
                logo = Content.Load<Texture2D>("logo");
            }
            catch (ContentLoadException e) // custom logo not found
            {
                logo = Content.Load<Texture2D>("logo-default");
            }
            logoPosition = new Vector2(Math.Max(0, (Game1.INTERNAL_WIDTH - logo.Width) / 2), 
                Math.Max(STRIPE_START, (Game1.INTERNAL_HEIGHT - logo.Height) / 2));
        }

        public UiStripe()
        {
            logo = null;
            logoPosition = Vector2.Zero;
        }

        
        public void addElement(UiElement element, char location)
        {
            if (location == 'l')
            {
                leftElement = element;
                stripeLeft = new Vector2(0, STRIPE_START);
            }
            else if (location == 'r')
            {
                rightElement = element;
                stripeRight = new Vector2(Game1.INTERNAL_WIDTH - rightElement.size.X, STRIPE_START);
            }
        }
        
        public void update(double dt)
        {
            if (leftElement != null) leftElement.update(dt);
            if (rightElement != null)
            {
                rightElement.update(dt);
                // update stripeRight location in case the element changed size
                stripeRight = new Vector2(Game1.INTERNAL_WIDTH - rightElement.size.X, STRIPE_START);
            }

        }


        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.WHITE_PIXEL, 
                new Rectangle(0, Game1.INTERNAL_HEIGHT - STRIPE_HEIGHT, Game1.INTERNAL_WIDTH, STRIPE_HEIGHT), 
                Color.White);
            if (logo != null)
            {
                spriteBatch.Draw(logo, logoPosition, Color.White);
            }

            if (leftElement != null) leftElement.draw(spriteBatch, stripeLeft);
            if (rightElement != null) rightElement.draw(spriteBatch, stripeRight);
        }
    }
}
