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
    class UiTilePicker : UiElement
    {

        public override Vector2 size
        {
            get
            {
                return new Vector2(160, 160);
            }
        }

        public Tile selectedTile
        {
            get
            {
                return allTiles[selected];
            }
        }

        Texture2D cursor;
        static List<Tile> allTiles;
        int selected;

        public UiTilePicker(ContentManager Content)
        {
            setBackground(Color.White);
            if (allTiles == null) // need to load all tiles on first run
            {
                allTiles = new List<Tile>();
                foreach (string file in Directory.EnumerateFiles("Content/tiles"))
                {
                    string cleaned = Path.GetFileNameWithoutExtension(file);
                    allTiles.Add(new Tile(Content, cleaned));
                }
            }
            cursor = Content.Load<Texture2D>("cursor");
            selected = 0;
        }

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
            if (selected + amnt >= 0 && selected + amnt < allTiles.Count)
            {
                selected += amnt;
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            int n = 0;
            while (n < allTiles.Count)
            {
                allTiles[n].draw(wrapper, new Vector2((n%10) * Tile.TILE_SIZE, (n/10) * Tile.TILE_SIZE));
                n++;
            }
            wrapper.draw(cursor, new Vector2((selected % 10) * Tile.TILE_SIZE, (selected / 10) * Tile.TILE_SIZE));
        }
    }
}
