using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class Overworld : IGameState
    {
        Texture2D background;
        Texture2D foreground;
        UiStripe stripe;
        int width;
        int x;

        public Overworld(ContentManager Content)
        {
            background = Content.Load<Texture2D>("map/background");
            foreground = Content.Load<Texture2D>("map/foreground");
            width = foreground.Width;
            stripe = new UiStripe();
            x = 0;
        }

        public void onInput(InputHandler input)
        {
            if (input.isKeyHeld(Keys.A) && x > 0) x--;
            else if (input.isKeyHeld(Keys.D) && x < width - Game1.INTERNAL_WIDTH) x++;
        }

        public void update(double dt)
        {
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(background);
            wrapper.draw(foreground, Vector2.Zero, new Rectangle(x, 0, Game1.INTERNAL_WIDTH, Game1.INTERNAL_HEIGHT));
            stripe.draw(wrapper);
        }


    }
}
