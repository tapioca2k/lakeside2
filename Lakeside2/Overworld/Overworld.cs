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

namespace Lakeside2.WorldMap
{
    // serialization of overworld data
    class OverworldMeta
    {
        public List<OWLocation> locations { get; set; }
        public List<string> layers { get; set; }

        public int baseLayer { get; set; }
    }

    class Overworld : IGameState
    {

        // sorts locations by X coordinate for ordering
        private class LocationComparer : IComparer<OWLocation>
        {
            public int Compare(OWLocation a, OWLocation b)
            {
                return (int)(a.location.X - b.location.X);
            }
        }

        Game1 game;
        ContentManager Content;
        public OverworldMeta meta;
        IGameState editor;
        UiSystem ui;
        OWPlayer player;
        List<Texture2D> layers;
        List<double> parallax;
        List<double> scrollValues;
        int index;
        public Color background => Game1.BG_COLOR;

        bool editing => editor != null;
        public int width => layers[meta.baseLayer].Width;
        public double x
        {
            get
            {
                return parallax[meta.baseLayer];
            }
            set
            {
                for (int i = 0; i < parallax.Count; i++)
                {
                    parallax[i] = value * scrollValues[i];
                }
                bindParallax();
            }
        }

        List<OWLocation> locations
        {
            get
            {
                return meta.locations;
            }
            set
            {
                meta.locations = value;
            }
        }

        OWLocation selected
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
            ui = new UiSystem();

            player = new OWPlayer(Content, p, Vector2.Zero);

            // load locations and texture layers from json
            string json = File.ReadAllText("Content/map/map.json");
            meta = JsonSerializer.Deserialize<OverworldMeta>(json, SerializableMap.OPTIONS);
            this.locations = meta.locations;
            locations.ForEach(l => l.load(Content));
            sortLocations();
            reloadLayers();

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

            ui.addStripeElement(new UiObjectMonitor<List<OWLocation>>(locations, locs =>
            {
                return Path.GetFileNameWithoutExtension(locs[index].filename);
            }), StripePosition.Center);

        }

        public void reloadLayers()
        {
            // load textures
            layers = new List<Texture2D>();
            parallax = new List<double>();
            scrollValues = new List<double>();
            meta.layers.ForEach(filename =>
            {
                layers.Add(Content.Load<Texture2D>("map/" + filename));
                parallax.Add(0);
            });

            // calculate how much each layer should scroll per update
            for (int i = 0; i < layers.Count; i++)
            {
                scrollValues.Add((layers[i].Width - Game1.INTERNAL_WIDTH) / (double)(width - Game1.INTERNAL_WIDTH));
            }

        }

        public void onInput(InputHandler input)
        {
            if (editing)
            {
                editor.onInput(input);
            }
            else
            {
                bool interacting = ui.onInput(input);
                if (interacting) return;

                if (input.isCommandPressed(Bindings.Left) && index > 0)
                {
                    index--;
                    setPlayerLocation();
                }
                else if (input.isCommandPressed(Bindings.Right) && index < locations.Count - 1)
                {
                    index++;
                    setPlayerLocation();
                }
                else if (input.isCommandPressed(Bindings.Interact))
                {
                    game.goToWorld(player.p, selected.filename);
                }

                if (input.isCommandPressed(Bindings.Start))
                {
                    ui.pushElement(new UiPauseMenu(game, Content, player.p, selected.filename, true),
                        new Vector2(Tile.TILE_SIZE, Tile.TILE_SIZE));
                }
            }

            if (input.isKeyPressed(Keys.F1))
            {
                if (editing) editor = null;
                else editor = new OverworldEditor(Content, this);
            }
        }

        public void setPlayerLocation()
        {
            if (index > locations.Count) index = locations.Count - 1;
            else if (index < 0) index = 0;
            player.feet = locations[index].center;
        }

        // get location the camera desires to be at
        double getCameraDesired()
        {
            return Math.Min(width - Game1.INTERNAL_WIDTH, Math.Max(0, player.feet.X - (Game1.INTERNAL_WIDTH / 2)));
        }

        // bind parallax values to within texture bounds to prevent weird stretching
        void bindParallax()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                parallax[i] = Math.Max(0, Math.Min(parallax[i], layers[i].Width - Game1.INTERNAL_WIDTH));
            }
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
            }
            else
            {
                ui.update(dt);
                double desired = getCameraDesired();
                if (x > desired)
                {
                    double fgScroll = Math.Min(dt * 300, x - desired);
                    for (int i = 0; i < parallax.Count; i++)
                    {
                        parallax[i] -= fgScroll * scrollValues[i];
                    }
                }
                else if (x < desired)
                {
                    double fgScroll = Math.Min(dt * 300, desired - x);
                    for (int i = 0; i < parallax.Count; i++)
                    {
                        parallax[i] += fgScroll * scrollValues[i];
                    }
                }
                bindParallax();
            }
        }

        public void draw(SBWrapper wrapper)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                wrapper.draw(layers[i], Vector2.Zero, new Rectangle((int)parallax[i], 0, Game1.INTERNAL_WIDTH, Game1.INTERNAL_HEIGHT));
            }
            //wrapper.draw(background);
            //wrapper.draw(foreground, Vector2.Zero, new Rectangle((int)x, 0, Game1.INTERNAL_WIDTH, Game1.INTERNAL_HEIGHT));

            SBWrapper relative = new SBWrapper(wrapper, new Vector2(-(int)x, 0));
            locations.ForEach(l => l.draw(relative));
            player.draw(relative);

            if (editing) editor.draw(wrapper);
            else ui.draw(wrapper);
        }


    }
}
