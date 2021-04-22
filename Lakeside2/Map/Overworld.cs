using Lakeside2.Editor;
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
        private class LocationComparer : IComparer<MapLocation>
        {
            public int Compare(MapLocation a, MapLocation b)
            {
                return (int)(a.location.X - b.location.X);
            }
        }

        Game1 game;
        ContentManager Content;
        Texture2D background;
        Texture2D foreground;
        UiStripe stripe;
        int width;
        double x;

        OverworldEditor editor;
        bool editing => editor != null;

        MapPlayer player;
        public List<MapLocation> locations;
        int index;

        MapLocation selected
        {
            get
            {
                return locations[index];
            }
        }

        public Overworld(ContentManager Content, Game1 game, Player p, string current = null)
        {
            this.game = game;
            this.Content = Content;
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
            sortLocations();

            // put player in the correct spot
            if (current == null) index = 0;
            else
            {
                for (int i = 0; i < locations.Count; i++)
                {
                    if (locations[i].filename == current)
                    {
                        index = i; break;
                    }
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
            if (editing)
            {
                editor.onInput(input);
            }
            else
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

            if (input.isKeyPressed(Keys.F1))
            {
                if (editing)
                {
                    editor = null;
                }
                else
                {
                    editor = new OverworldEditor(Content, this);
                }
            }
        }

        public void setPlayerLocation()
        {
            if (index > locations.Count) index = locations.Count - 1;
            else if (index < 0) index = 0;
            player.feet = locations[index].center;
        }

        double getCameraDesired()
        {
            return Math.Min(width - Game1.INTERNAL_WIDTH, Math.Max(0, player.feet.X - (Game1.INTERNAL_WIDTH / 2)));
        }

        public void sortLocations()
        {
            locations.Sort(new LocationComparer());
        }

        public void update(double dt)
        {
            if (editing)
            {
                editor.update(dt);
                x = editor.getCameraPosition(width);
            }
            else
            {
                stripe.update(dt);
                double desired = getCameraDesired();
                if (x > desired) x -= Math.Min(dt * 300, x - desired);
                else if (x < desired) x += Math.Min(dt * 300, desired - x);
            }
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(background);
            wrapper.draw(foreground, Vector2.Zero, new Rectangle((int)x, 0, Game1.INTERNAL_WIDTH, Game1.INTERNAL_HEIGHT));

            SBWrapper relative = new SBWrapper(wrapper, new Vector2(-(int)x, 0));
            locations.ForEach(l => l.draw(relative));
            player.draw(relative);

            if (editing) editor.draw(wrapper, x);
            else stripe.draw(wrapper);
        }


    }
}
