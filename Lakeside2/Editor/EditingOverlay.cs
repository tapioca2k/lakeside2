using Lakeside2.UI;
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
        TileMap map;

        public EditingOverlay(ContentManager Content, TilemapCamera camera, TileMap map)
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
            this.map = map;
            this.camera.setCenteringEntity(cursor);
        }

        public void onInput(InputHandler input)
        {
            bool interactingWithUi = ui.onInput(input);
            if (interactingWithUi) return;

            cursor.onInput(input);

            if (input.isKeyPressed(Keys.F2))
            {
                UiElement filename = new UiTextInput().addCallback((element) =>
                {
                    SerializableMap.Save(Content, map, ((UiTextInput)element).text);
                    return true;
                });
                filename.setBackground(Color.White);
                ui.pushElement(filename, Vector2.One);
            }
            else if (input.isKeyPressed(Keys.F3))
            {
                UiElement filename = new UiTextInput().addCallback((element) =>
                {
                    TileMap newMap = SerializableMap.Load(Content, ((UiTextInput)element).text);
                    this.map = newMap;
                    // TODO this appears to draw correctly when leaving edit mode but the map in World isn't changing. WILL cause bugs.
                    this.camera.setMap(newMap);
                    cursor.setLocation(Vector2.Zero);
                    return true;
                });
                filename.setBackground(Color.White);
                ui.pushElement(filename, Vector2.One);
            }
        }

        public void update(double dt)
        {
            camera.update(dt);
            ui.update(dt);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            camera.draw(spriteBatch, new List<IEntity>(new IEntity[] { cursor }));
            ui.draw(spriteBatch);
        }
    }
}
