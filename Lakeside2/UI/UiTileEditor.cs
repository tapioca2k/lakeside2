using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

        ContentManager Content;
        public Tile tile;
        public int x, y;

        public UiTileEditor(ContentManager Content, Tile tile, int x, int y)
        {
            this.Content = Content;
            this.tile = tile;
            this.x = x;
            this.y = y;
            setBackground(Color.White);
        }

        public override void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.Enter) || input.isKeyPressed(Keys.E))
            {
                finished = true;
            }
            else if (input.isKeyPressed(Keys.T))
            {
                system.pushElement(new UiTilePicker(Content).addCallback((element) =>
                {
                    UiTilePicker picker = (UiTilePicker)element;
                    Tile picked = picker.selectedTile;
                    this.tile.setTexture(Content, picked.filename);
                    return true;
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.W))
            {
                this.tile.collision = !this.tile.collision;
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            tile.draw(wrapper, new Vector2(5, 5));

            wrapper.drawString("(T)ile: " + tile.filename, new Vector2(25, 5));
            wrapper.drawString("(W)alkable: " + UiTextDisplay.YesOrNo(tile.collision), new Vector2(5, 25));
        }

    }
}
