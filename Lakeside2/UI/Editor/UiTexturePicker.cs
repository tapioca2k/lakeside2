using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Lakeside2.UI.Editor
{
    abstract class UiTexturePicker : UiElement
    {

        public override Vector2 size
        {
            get
            {
                return new Vector2(160, 160);
            }
        }

        public IDrawable selected
        {
            get
            {
                return all[index];
            }
        }

        Texture2D cursor;
        List<IDrawable> all;
        int index;

        public UiTexturePicker(ContentManager Content)
        {
            setBackground(Game1.BG_COLOR, false);
            all = populateList(Content);
            cursor = Content.Load<Texture2D>("entities/cursor");
            index = 0;
        }

        public abstract List<IDrawable> populateList(ContentManager Content);

        public override void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.W)) tryMoveCursor(-10);
            else if (input.isKeyPressed(Keys.A)) tryMoveCursor(-1);
            else if (input.isKeyPressed(Keys.S)) tryMoveCursor(10);
            else if (input.isKeyPressed(Keys.D)) tryMoveCursor(1);
            else if (input.isKeyPressed(Keys.Enter) || input.isKeyPressed(Keys.T)) finished = true;
        }

        void tryMoveCursor(int amnt)
        {
            if (index + amnt >= 0 && index + amnt < all.Count)
            {
                index += amnt;
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            int n = 0;
            while (n < all.Count)
            {
                all[n].draw(wrapper, new Vector2((n % 10) * Tile.TILE_SIZE, (n / 10) * Tile.TILE_SIZE));
                n++;
            }
            wrapper.draw(cursor, new Vector2((index % 10) * Tile.TILE_SIZE, (index / 10) * Tile.TILE_SIZE), new Rectangle(0, 0, 16, 16));
        }
    }
}
