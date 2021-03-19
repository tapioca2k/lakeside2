using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class UiStripe
    {
        const int STRIPE_HEIGHT = Tile.TILE_SIZE + 4;
        Texture2D logo;
        Vector2 logoPosition;

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
                Math.Max(Game1.INTERNAL_HEIGHT - STRIPE_HEIGHT, (Game1.INTERNAL_HEIGHT - logo.Height) / 2));
        }

        /*
        public void addElement(UiElement element, Vector2 location)
        {
            // TODO
        }
        */

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.WHITE_PIXEL, new Rectangle(0, Game1.INTERNAL_HEIGHT - STRIPE_HEIGHT, Game1.INTERNAL_WIDTH, STRIPE_HEIGHT), Color.White);
            spriteBatch.Draw(logo, logoPosition, Color.White);
        }
    }
}
