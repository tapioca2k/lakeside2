using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class TilemapCamera
    {
        static float SLOWNESS = 0.1f;

        TileMap map;
        Vector2 location;

        bool inMove;
        Vector2 targetLocation;

        public TilemapCamera(TileMap map)
        {
            this.map = map;
            location = Vector2.Zero;

            inMove = false;
            targetLocation = Vector2.Zero;
        }

        public void rawMove(Vector2 amnt)
        {
            location += amnt;
        }

        public void tileMoveX(int amnt)
        {
            if (inMove) return;
            targetLocation = new Vector2(location.X + (amnt * Tile.TILE_SIZE), location.Y);
            inMove = true;
        }

        public void tileMoveY(int amnt)
        {
            if (inMove) return;
            targetLocation = new Vector2(location.X, location.Y + (amnt * Tile.TILE_SIZE));
            inMove = true;
        }

        public void update(double dt)
        {
            if (inMove)
            {
                // TODO smoothness
                rawMove(new Vector2(targetLocation.X - location.X, targetLocation.Y - location.Y));
                if (Vector2.Distance(location, targetLocation) < 1)
                {
                    location = targetLocation;
                    inMove = false;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            map.draw(spriteBatch, -location);
        }

    }
}
