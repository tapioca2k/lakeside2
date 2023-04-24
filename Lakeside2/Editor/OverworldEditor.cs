using Lakeside2.WorldMap;
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
using Lakeside2.UI.Editor;

namespace Lakeside2.Editor
{

    class OverworldEditor : IGameState
    {
        public const string HELP_HOTKEY = "Press \"H\" for help.";
        public const string HELP_STRING =
            "A&D: Camera / E: New loc / R: Delete loc / L: Layers / " +
            "F1-2-3: Exit-Save-Load";

        ContentManager Content;
        Overworld map;
        UiSystem ui;
        OverworldCursor cursor;

        public Color background => map.background;

        UiTextDisplay statusLine;
        double t = 0;

        public OverworldEditor(ContentManager Content, Overworld map)
        {
            this.Content = Content;
            this.map = map;
            this.cursor = new OverworldCursor(Content);

            statusLine = new UiTextDisplay(HELP_HOTKEY);
            ui = new UiSystem(true);
            ui.addStripeElement(statusLine, StripePosition.Left);
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

            if (input.isKeyHeld(Keys.A)) map.x -= 5;
            else if (input.isKeyHeld(Keys.D)) map.x += 5;

            if (input.isKeyPressed(Keys.H))
            {
                ui.pushElement(new UiTextBox(HELP_STRING, false), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.F2)) // Save
            {
                UiElement filename = new UiTextInput("Save Filename: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "")
                    {
                        string json = JsonSerializer.Serialize<OverworldMeta>(map.meta, SerializableMap.OPTIONS);
                        File.WriteAllText("Content/map/" + input.text, json);
                    }
                });
                ui.pushElement(filename, Vector2.One);
            }
            else if (input.isKeyPressed(Keys.F3)) // Load
            {
                UiElement filename = new UiTextInput("Load Filename: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "")
                    {
                        map.loadMap("Content/map/" + input.text);
                        map.setPlayerLocation();
                    }
                });
                ui.pushElement(filename, Vector2.One);
            }
            else if (input.isKeyPressed(Keys.E)) // Create new map location
            {
                ui.pushElement(new UiTextInput("Filename: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "")
                    {
                        map.addLocation(input.text, cursor.getLocation() + new Vector2((float)map.x, 0));
                        map.setPlayerLocation();
                    }
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.R)) // Delete map location
            {
                Vector2 cursorCenter = cursor.center + new Vector2((float)map.x, 0);
                for (int i = 0; i < map.meta.locations.Count; i++)
                {
                    if (Vector2.Distance(map.meta.locations[i].center, cursorCenter) < 10)
                    {
                        map.meta.locations.Remove(map.meta.locations[i]);
                        map.setPlayerLocation();
                        break;
                    }
                }
            }
            else if (input.isKeyPressed(Keys.L)) // Layer editor
            {
                ui.pushElement(new UiLayerEditor(Content, map), new Vector2(10, 30));
            }
        }

        public void update(double dt)
        {
            ui.update(dt);

            t += dt;
            if (t > 3 && statusLine.text == HELP_HOTKEY)
            {
                statusLine.text = "";
            }
        }

        public double getCameraPosition(int mapWidth)
        {
            return Math.Max(0, Math.Min(mapWidth - World.PORTAL_WIDTH, cursor.getLocation().X - World.HALF_PORTAL_WIDTH));
        }

        public void draw(SBWrapper wrapper)
        {
            cursor.draw(wrapper, cursor.getLocation());
            ui.draw(wrapper);
        }

    }
}
