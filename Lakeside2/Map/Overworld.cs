using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Map
{
    class Overworld : IGameState
    {
        Texture2D background;
        Texture2D foreground;
        UiStripe stripe;
        int width;
        int x;

        MapPlayer player;
        List<MapLocation> locations;
        int index;

        MapLocation selected
        {
            get
            {
                return locations[index];
            }
        }

        public Overworld(ContentManager Content)
        {
            background = Content.Load<Texture2D>("map/background");
            foreground = Content.Load<Texture2D>("map/foreground");
            width = foreground.Width;
            stripe = new UiStripe();
            x = 0;

            player = new MapPlayer(Content, Vector2.Zero);
            locations = new List<MapLocation>(); // TODO populate from JSON
            locations.Add(new MapLocation(Content, "forestguard.json", new Vector2(274, 116)));
            locations.Add(new MapLocation(Content, "beach.json", new Vector2(532, 120)));

            player.feet = locations[0].center;
            index = 0;
            x = (int)Math.Max(0, player.feet.X - (Game1.INTERNAL_WIDTH / 2));

            stripe.addElement(new UiObjectMonitor<List<MapLocation>>(locations, locs =>
            {
                return locs[index].filename;
            }), 'l');
        }

        public void onInput(InputHandler input)
        {
            //if (input.isKeyHeld(Keys.A) && x > 0) x--;
            //else if (input.isKeyHeld(Keys.D) && x < width - Game1.INTERNAL_WIDTH) x++;

            if (input.isKeyPressed(Keys.A) && index > 0)
            {
                index--;
                player.feet = locations[index].center;
                x = (int)Math.Max(0, player.feet.X - (Game1.INTERNAL_WIDTH / 2));
            }
            else if (input.isKeyPressed(Keys.D) && index < locations.Count - 1)
            {
                index++;
                player.feet = locations[index].center;
                x = (int)Math.Max(0, player.feet.X - (Game1.INTERNAL_WIDTH / 2));
            }
        }

        public void update(double dt)
        {
            stripe.update(dt);
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(background);
            wrapper.draw(foreground, Vector2.Zero, new Rectangle(x, 0, Game1.INTERNAL_WIDTH, Game1.INTERNAL_HEIGHT));

            SBWrapper relative = new SBWrapper(wrapper, new Vector2(-x, 0));
            locations.ForEach(l => l.draw(relative));
            player.draw(relative);

            stripe.draw(wrapper);
        }


    }
}
