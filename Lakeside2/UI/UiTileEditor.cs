using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI
{
    class UiTileEditor : UiElement
    {
        public override Vector2 size
        {
            get
            {
                return new Vector2(160, 160);
            }
        }

        public Tile tile;
        public int x, y;

        public UiTileEditor(Tile tile, int x, int y)
        {
            this.tile = tile;
            this.x = x;
            this.y = y;
            setBackground(Color.White);
        }

        public override void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.E))
            {
                finished = true;
            }
            else if (input.isKeyPressed(Keys.T))
            {
                // TODO open tile picker
            }
            else if (input.isKeyPressed(Keys.W))
            {
                this.tile.collision = !this.tile.collision;
            }
        }

        public override void draw(SpriteBatch spriteBatch, Vector2 location)
        {
            drawBackground(spriteBatch, location);
            tile.draw(spriteBatch, location + new Vector2(5, 5));

            spriteBatch.DrawString(Fonts.get("Arial"), 
                "(T)ile: " + tile.filename, 
                location + new Vector2(25, 5), Color.Black);

            spriteBatch.DrawString(Fonts.get("Arial"), 
                "(W)alkable: " + UiTextDisplay.YesOrNo(tile.collision), 
                location + new Vector2(5, 25), Color.Black);
        }

    }
}
