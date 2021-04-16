using Lakeside2.Serialization;
using Lakeside2.UI;
using Lakeside2.UI.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.Editor
{
    class EditingOverlay
    {
        ContentManager Content;
        UiSystem ui;
        Cursor cursor;
        World world;
        TilemapCamera camera;
        Tile lastEditedTile;

        TileMap map
        {
            get
            {
                return camera.getMap();
            }
        }

        public EditingOverlay(ContentManager Content, World world)
        {
            this.Content = Content;
            this.world = world;
            cursor = new Cursor(Content);

            ui = new UiSystem();
            ui.addStripeElement(new UiEditorStripe(Content), 'l');
            ui.addStripeElement(new UiObjectMonitor<Cursor>(cursor, (cursor) =>
            {
                return cursor.getTileLocation().ToString();
            }), 'r');

            this.camera = world.camera;
            this.camera.setCenteringEntity(cursor);
        }

        public void onInput(InputHandler input)
        {
            bool interactingWithUi = ui.onInput(input);
            if (interactingWithUi) return;

            cursor.onInput(input);

            if (input.isKeyPressed(Keys.F2)) // Save
            {
                UiElement filename = new UiTextInput("Save Filename: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "") SerializableMap.Save(Content, map, input.text);
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
                        TileMap newMap = SerializableMap.Load(Content, input.text);
                        if (newMap != null)
                        {
                            this.world.setMap(newMap);
                            cursor.setLocation(Vector2.Zero);
                        }
                    }
                });
                ui.pushElement(filename, Vector2.One);
            }
            else if (input.isKeyPressed(Keys.E)) // Edit tile properties
            {
                Tile selected = map.getTile(cursor.getTileLocation());
                NPC npc = map.getNPC(cursor.getTileLocation());
                if (selected != null)
                {
                    ui.pushElement(new UiTileEditor(Content, selected, npc).addCallback(element =>
                    {
                        UiTileEditor editor = (UiTileEditor)element;
                        map.setTile(cursor.getTileLocation(), editor.tile);
                        map.setNPC(cursor.getTileLocation(), editor.npc);
                        lastEditedTile = editor.tile;
                    }), new Vector2(160, 0));
                }
            }
            else if (input.isKeyHeld(Keys.P)) // Tile painter
            {
                if (lastEditedTile != null)
                {
                    map.setTile(cursor.getTileLocation(), new Tile(lastEditedTile));
                }
            }
            else if (input.isKeyPressed(Keys.M)) // Edit map meta info
            {
                ui.pushElement(new UiMapMetaEditor(map), new Vector2(160, 0));
            }
        }

        public void update(double dt)
        {
            camera.update(dt);
            cursor.update(dt);
            ui.update(dt);
        }

        public void draw(SBWrapper wrapper)
        {
            List<Entity> entities = new List<Entity>();
            entities.Add(cursor);
            entities.AddRange(map.npcs);
            camera.draw(wrapper, entities);
            ui.draw(wrapper);
        }
    }
}
