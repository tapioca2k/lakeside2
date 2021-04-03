using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2
{
    class TileMap
    {
        ContentManager Content;
        public Tile[,] map;
        public int width;
        public int height;
        public Color color;
        public string filename;

        public static Vector2 worldToTile(Vector2 real)
        {
            return new Vector2(
                (int)real.X / Tile.TILE_SIZE,
                (int)real.Y / Tile.TILE_SIZE);
        }

        // default, empty TileMap
        public TileMap(ContentManager Content, int width, int height)
        {
            this.Content = Content;
            this.width = width;
            this.height = height;
            this.color = Color.Black;
            this.filename = "(Unsaved)";
            map = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = new Tile(Content, "unknown");
                }
            }
        }

        // only SerializableMap.ToTilemap() should use this constructor
        public TileMap(Tile[,] tiles, Color color, string filename)
        {
            this.map = tiles;
            this.width = tiles.GetLength(0);
            this.height = tiles.GetLength(1);
            this.color = color;
            this.filename = filename;
        }

        public Tile getTile(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return null;
            else return map[x, y];
        }
        public Tile getTile(Vector2 tileLocation)
        {
            return getTile((int)tileLocation.X, (int)tileLocation.Y);
        }

        public void setTile(int x, int y, Tile tile)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;
            else map[x, y] = tile;
        }
        public void setTile(Vector2 tileLocation, Tile tile)
        {
            setTile((int)tileLocation.X, (int)tileLocation.Y, tile);
        }

        public bool checkCollision(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            else return map[x, y].collision;
        }
        public bool checkCollision(Vector2 coordindates)
        {
            return checkCollision((int) coordindates.X, (int) coordindates.Y);
        }

        public void stepOn(Vector2 tileLocation, Lua worldLua)
        {
            Tile t = getTile(tileLocation);
            if (t.script != null) t.script.execute(worldLua);
        }

        // for the editor
        public void resize(int newWidth, int newHeight)
        {
            Tile[,] newtiles = new Tile[newWidth, newHeight];
            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    if (x >= width || y >= height) newtiles[x, y] = new Tile(Content, "unknown");
                    else newtiles[x, y] = map[x, y];
                }
            }
            map = newtiles;
            width = newWidth;
            height = newHeight;
        }

        public void draw(SBWrapper wrapper)
        {
            int startX = Math.Max(0, (int) Math.Floor(-wrapper.location.X / Tile.TILE_SIZE));
            int startY = Math.Max(0, (int)Math.Floor(-wrapper.location.Y / Tile.TILE_SIZE));
            for (int x = startX; x < Math.Min(width, Game1.TILE_WIDTH); x++)
            {
                for (int y = startY; y < Math.Min(height, Game1.TILE_HEIGHT); y++)
                {
                    map[x, y].draw(wrapper, new Vector2(x, y) * Tile.TILE_SIZE);
                }
            }
        }

    }
}
