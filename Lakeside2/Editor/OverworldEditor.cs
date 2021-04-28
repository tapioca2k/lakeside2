using Lakeside2.Map;
using Lakeside2.Serialization;
using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using static Lakeside2.Entity;

namespace Lakeside2.Editor
{

    class OverworldEditor : IGameState
    {

        ContentManager Content;
        Overworld map;
        UiSystem ui;
        OverworldCursor cursor;

        public OverworldEditor(ContentManager Content, Overworld map)
        {
            this.Content = Content;
            this.map = map;
            this.cursor = new OverworldCursor(Content);

            ui = new UiSystem();
            ui.addStripeElement(new UiTexture(Content, "owhotkeys"), StripePosition.Left);
            ui.addStripeElement(new UiObjectMonitor<Cursor>(this.cursor, (cursor) =>
            {
                return cursor.getLocation().ToString();
            }), StripePosition.Right);
        }

        public void onInput(InputHandler input)
        {
            bool interacting = ui.onInput(input);
            if (interacting) return;

            cursor.onInput(input);

            if (input.isKeyPressed(Keys.F2)) // Save
            {
                string json = JsonSerializer.Serialize<List<MapLocation>>(map.locations, SerializableMap.OPTIONS);
                File.WriteAllText("Content/map/map.json", json);
            }
            else if (input.isKeyPressed(Keys.E)) // Create new map location
            {
                ui.pushElement(new UiTextInput("Filename: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "")
                    {
                        map.locations.Add(new MapLocation(Content, input.text, cursor.getLocation()));
                        map.sortLocations();
                        map.setPlayerLocation();
                    }
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.R)) // Delete map location
            {
                for (int i = 0; i < map.locations.Count; i++)
                {
                    if (Vector2.Distance(map.locations[i].center, cursor.center) < 10)
                    {
                        map.locations.RemoveAt(i);
                        map.setPlayerLocation();
                        break;
                    }
                }
            }
        }

        public void update(double dt)
        {
            ui.update(dt);
            map.x = getCameraPosition(map.width);
        }

        public double getCameraPosition(int mapWidth)
        {
            return Math.Max(0, Math.Min(mapWidth - World.PORTAL_WIDTH, cursor.getLocation().X - World.HALF_PORTAL_WIDTH));
        }

        public void draw(SBWrapper wrapper)
        {
            ui.draw(wrapper);
            cursor.draw(new SBWrapper(wrapper, new Vector2(-(int)map.x, 0)), cursor.getLocation());
        }

    }
}
