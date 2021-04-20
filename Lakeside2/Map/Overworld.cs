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
        Game1 game;
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

        public Overworld(ContentManager Content, Game1 game, Player p, string current)
        {
            this.game = game;
            background = Content.Load<Texture2D>("map/background");
            foreground = Content.Load<Texture2D>("map/foreground");
            width = foreground.Width;
            stripe = new UiStripe();
            x = 0;

            player = new MapPlayer(Content, p, Vector2.Zero);
            locations = new List<MapLocation>(); // TODO populate from JSON
            locations.Add(new MapLocation(Content, "forestguard.json", new Vector2(274, 116)));
            locations.Add(new MapLocation(Content, "beach.json", new Vector2(532, 120)));

            // put player in the correct spot
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].filename == current)
                {
                    index = i; break;
                }
            }
            setPlayerLocation();

            stripe.addElement(new UiObjectMonitor<List<MapLocation>>(locations, locs =>
            {
                return locs[index].filename;
            }), 'l');
        }

        public void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.A) && index > 0)
            {
                index--;
                setPlayerLocation();
            }
            else if (input.isKeyPressed(Keys.D) && index < locations.Count - 1)
            {
                index++;
                setPlayerLocation();
            }
            else if (input.isKeyPressed(Keys.E))
            {
                game.goToWorld(player.p, selected.filename);
            }
        }

        void setPlayerLocation()
        {
            player.feet = locations[index].center;
            x = (int)Math.Max(0, player.feet.X - (Game1.INTERNAL_WIDTH / 2));
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
