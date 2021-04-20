using Lakeside2.Serialization;
using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Lakeside2.Map
{
    class Overworld : IGameState
    {
        Game1 game;
        Texture2D background;
        Texture2D foreground;
        UiStripe stripe;
        int width;
        double x;

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

            locations = new List<MapLocation>();
            // load locations from json
            string json = File.ReadAllText("Content/map/map.json");
            locations = JsonSerializer.Deserialize<List<MapLocation>>(json, SerializableMap.OPTIONS);
            locations.ForEach(l => l.load(Content));

            // put player in the correct spot
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].filename == current)
                {
                    index = i; break;
                }
            }
            setPlayerLocation();
            x = getCameraDesired();

            stripe.addElement(new UiObjectMonitor<List<MapLocation>>(locations, locs =>
            {
                return Path.GetFileNameWithoutExtension(locs[index].filename);
            }), 'c');
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
        }

        double getCameraDesired()
        {
            return Math.Min(width - Game1.INTERNAL_WIDTH, Math.Max(0, player.feet.X - (Game1.INTERNAL_WIDTH / 2)));
        }

        public void update(double dt)
        {
            stripe.update(dt);

            double desired = getCameraDesired();
            if (x > desired) x -= Math.Min(dt * 300, x - desired);
            else if (x < desired) x += Math.Min(dt * 300, desired - x);
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(background);
            wrapper.draw(foreground, Vector2.Zero, new Rectangle((int)x, 0, Game1.INTERNAL_WIDTH, Game1.INTERNAL_HEIGHT));

            SBWrapper relative = new SBWrapper(wrapper, new Vector2(-(int)x, 0));
            locations.ForEach(l => l.draw(relative));
            player.draw(relative);

            stripe.draw(wrapper);
        }


    }
}
