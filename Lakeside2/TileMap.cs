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
        public int width;
        public int height;

        public TileMap(ContentManager Content, int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = new Tile(Content, "unknown");
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, Vector2 position)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 mapSpace = position + (new Vector2(x, y) * Tile.TILE_SIZE);
                    map[x, y].draw(spriteBatch, mapSpace);
                }
            }
        }

    }
}
