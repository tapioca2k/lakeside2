using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Editor
{
    class Cursor : Entity
    {
        public const string ENTITY_NAME = "cursor";

        public override string name => ENTITY_NAME;

        public Cursor(ContentManager Content)
        {
            loadAnimatedTexture(Content, "cursor");
        }

        public virtual void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.W)) location.Y -= Tile.TILE_SIZE;
            else if (input.isKeyPressed(Keys.A)) location.X -= Tile.TILE_SIZE;
            else if (input.isKeyPressed(Keys.S)) location.Y += Tile.TILE_SIZE;
            else if (input.isKeyPressed(Keys.D)) location.X += Tile.TILE_SIZE;
        }
    }
}
