using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Editor
{
    class OverworldCursor : Cursor
    {
        public OverworldCursor(ContentManager Content) : base(Content)
        {
        }

        public override void onInput(InputHandler input)
        {
            if (input.isKeyHeld(Keys.W)) location.Y--;
            else if (input.isKeyHeld(Keys.S)) location.Y++;

            if (input.isKeyHeld(Keys.A)) location.X--;
            else if (input.isKeyHeld(Keys.D)) location.X++;
        }

        public Vector2 center
        {
            get
            {
                return getLocation() + new Vector2(Tile.TILE_SIZE / 2, Tile.TILE_SIZE / 2);
            }
        }

    }
}
