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
        TilemapCamera camera;
        Tile lastEditedTile;

        TileMap map
        {
            get
            {
                return camera.getMap();
            }
        }

        public EditingOverlay(ContentManager Content, TilemapCamera camera)
        {
            this.Content = Content;

            cursor = new Cursor(Content);

            ui = new UiSystem();
            ui.addStripeElement(new UiEditorStripe(Content), 'l');
            ui.addStripeElement(new UiObjectMonitor<Cursor>(cursor, (cursor) =>
            {
                return cursor.getTileLocation().ToString();
            }), 'r');

            this.camera = camera;
            this.camera.setCenteringEntity(cursor);
        }

        public void onInput(InputHandler input)
        {
            bool interactingWithUi = ui.onInput(input);
            if (interactingWithUi) return;

            cursor.onInput(input);

            if (input.isKeyPressed(Keys.F2)) // Save
            {
                UiElement filename = new UiTextInput("Save Filename: ").addCallback((element) =>
                {
                    SerializableMap.Save(Content, map, ((UiTextInput)element).text);
                });
                ui.pushElement(filename, Vector2.One);
            }
            else if (input.isKeyPressed(Keys.F3)) // Load
            {
                UiElement filename = new UiTextInput("Load Filename: ").addCallback((element) =>
                {
                    TileMap newMap = SerializableMap.Load(Content, ((UiTextInput)element).text);
                    this.camera.setMap(newMap);
                    cursor.setLocation(Vector2.Zero);
                });
                ui.pushElement(filename, Vector2.One);
            }
            else if (input.isKeyPressed(Keys.E)) // Edit tile properties
            {
                Tile selected = map.getTile(cursor.getTileLocation());
                ui.pushElement(new UiTileEditor(Content, selected).addCallback((element) =>
                {
                    UiTileEditor editor = (UiTileEditor)element;
                    map.setTile(cursor.getTileLocation(), editor.tile);
                    lastEditedTile = editor.tile;

                }), new Vector2(160, 0));
            }
            else if (input.isKeyPressed(Keys.P)) // Tile painter
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
            camera.draw(wrapper, new List<Entity>(new Entity[] { cursor }));
            ui.draw(wrapper);
        }
    }
}
