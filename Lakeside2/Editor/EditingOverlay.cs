using Lakeside2.UI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
            cursor.onInput(input);
            ui.onInput(input);

            if (input.isKeyPressed(Keys.F2))
            {
                SerializableMap.Save(Content, map, "savedmap.txt"); // TODO prompt for filename
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
