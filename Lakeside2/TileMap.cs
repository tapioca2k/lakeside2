using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class TileMap
    {
        Tile[,] map;
        bool[,] collision;
        public int width;
        public int height;

        public static Vector2 realToTile(Vector2 real)
        {
            return new Vector2(
                (int)real.X / Tile.TILE_SIZE,
                (int)real.Y / Tile.TILE_SIZE);
        }

        public TileMap(ContentManager Content, int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new Tile[width, height];
            collision = new bool[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = new Tile(Content, "unknown");
                    collision[x, y] = true;
                }
            }
        }

        public bool checkCollision(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            else return collision[x, y];
        }
        public bool checkCollision(Vector2 coordindates)
        {
            return checkCollision((int) coordindates.X, (int) coordindates.Y);
        }

        public void draw(SpriteBatch spriteBatch, Vector2 position)
        {
            int startX = Math.Max(0, (int) Math.Floor(-position.X / Tile.TILE_SIZE));
            int startY = Math.Max(0, (int)Math.Floor(-position.Y / Tile.TILE_SIZE));
            for (int x = startX; x < Math.Min(width, Game1.TILE_WIDTH); x++)
            {
                for (int y = startY; y < Math.Min(height, Game1.TILE_HEIGHT); y++)
                {
                    Vector2 mapSpace = position + (new Vector2(x, y) * Tile.TILE_SIZE);
                    map[x, y].draw(spriteBatch, mapSpace);
                }
            }
        }

    }
}
