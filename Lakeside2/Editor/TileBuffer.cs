using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakeside2.Editor
{
    internal class TileBuffer
    {
        Tile[,] tiles;
        Point start;
        Point stop;

        public TileBuffer()
        {
            clear();
        }

        public void clear()
        {
            start = Point.Zero;
            stop = Point.Zero;
            tiles = new Tile[0, 0];
        }

        public void setStart(Point loc)
        {
            this.start = loc;
        }

        public void setStop(TileMap map, Point loc)
        {
            this.stop = loc;
            this.tiles = new Tile[stop.X - start.X, stop.Y - start.Y];
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[x, y] = map.getTile(start.ToVector2() + new Vector2(x, y));
                }
            }
        }

        // delete the tiles within the buffer boundary
        public void delete(TileMap map, ContentManager Content)
        {
            for (int x = start.X; x < stop.X; x++)
            {
                for (int y = start.Y; y < stop.Y; y++)
                {
                    map.setTile(new Vector2(x, y), Tile.EmptyTile(Content));
                }
            }
        }

        // copy tiles in the buffer to a location
        public void paste(TileMap map, Point loc)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    map.setTile(loc.ToVector2() + new Vector2(x, y), new Tile(tiles[x, y]));
                }
            }
        }

    }
}
