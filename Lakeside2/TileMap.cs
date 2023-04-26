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

    public class TileMap
    {
        private class PathComparer : IComparer<List<Point>>
        {
            public int Compare(List<Point> a, List<Point> b)
            {
                return a.Count - b.Count;
            }
        }

        public string name;
        public Tile[,] map;
        public int width;
        public int height;
        public Color color;
        public string filename;
        public List<NPC> npcs;
        public List<LuaScript> tileScripts;
        public Point playerStart;

        public static Point worldToTile(Vector2 real)
        {
            return new Point(
                (int)real.X / Tile.TILE_SIZE,
                (int)real.Y / Tile.TILE_SIZE);
        }

        // default, empty TileMap
        public TileMap(ContentManager Content, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.color = Color.Black;
            this.filename = "(Unsaved)";
            this.playerStart = Point.Zero;
            this.npcs = new List<NPC>();
            this.tileScripts = new List<LuaScript>();
            map = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = Tile.EmptyTile(Content);
                }
            }
        }

        // only SerializableMap.ToTilemap() should use this constructor
        public TileMap(string name, Tile[,] tiles, Color color, string filename, List<NPC> npcs, List<LuaScript> scripts, Point playerStart)
        {
            this.name = name;
            this.map = tiles;
            this.width = tiles.GetLength(0);
            this.height = tiles.GetLength(1);
            this.color = color;
            this.filename = filename;
            this.npcs = npcs;
            this.tileScripts = scripts;
            this.playerStart = playerStart;
        }

        public Tile getTile(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return null;
            else return map[x, y];
        }
        public Tile getTile(Point tileLocation)
        {
            return getTile(tileLocation.X, tileLocation.Y);
        }

        public void setTile(int x, int y, Tile tile)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;
            else map[x, y] = tile;
        }
        public void setTile(Point tileLocation, Tile tile)
        {
            setTile(tileLocation.X, tileLocation.Y, tile);
        }

        public void setNPC(Point tileLocation, NPC npc)
        {
            NPC present = getNPC(tileLocation);
            if (present != null) npcs.Remove(present);
            if (npc != null)
            {
                npc.setTileLocation(tileLocation);
                npcs.Add(npc);
            }
        }

        public NPC getNPC(Point tileLocation)
        {
            return npcs.Find(npc => npc.getTileLocation().Equals(tileLocation));
        }

        public NPC getNPC(string name)
        {
            return npcs.Find(n => n.name == name);
        }

        public LuaScript getScript(Point tileLocation)
        {
            return tileScripts.Find(s => s.getTileLocation().Equals(tileLocation));
        }

        public void setScript(Point tileLocation, LuaScript script)
        {
            LuaScript present = getScript(tileLocation);
            if (present != null) tileScripts.Remove(present);
            if (script != null)
            {
                script.location = tileLocation;
                tileScripts.Add(script);
            }
        }

        public bool checkCollision(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            else if (getNPC(new Point(x, y)) != null) return false;
            else return map[x, y].collision;
        }

        public bool checkCollision(Point coordindates)
        {
            return checkCollision(coordindates.X, coordindates.Y);
        }

        public void stepOn(Player player, Point tileLocation, Lua worldLua)
        {
            Tile t = getTile(tileLocation);
            LuaScript script = getScript(tileLocation);
            if (script != null) script.execute(player, worldLua);
        }

        // for the editor
        public void resize(ContentManager Content, int newWidth, int newHeight)
        {
            Tile[,] newtiles = new Tile[newWidth, newHeight];
            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    if (x >= width || y >= height) newtiles[x, y] = Tile.EmptyTile(Content);
                    else newtiles[x, y] = map[x, y];
                }
            }
            map = newtiles;
            width = newWidth;
            height = newHeight;
        }

        public void insertRow(ContentManager Content, int position)
        {
            resize(Content, width, height + 1);
            for (int x = 0; x < width; x++)
            {
                for (int y = height - 1; y > position; y--)
                {
                    map[x, y] = map[x, y - 1];
                }
            }
            for (int x = 0; x < width; x++)
            {
                map[x, position] = Tile.EmptyTile(Content);
            }
        }

        public void deleteRow(int position)
        {
            Tile[,] newtiles = new Tile[width, height - 1];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    Tile t = map[x, y];
                    if (y >= position) t = map[x, y + 1];
                    newtiles[x, y] = t;
                }
            }
            map = newtiles;
            height--;
        }

        public void insertColumn(ContentManager Content, int position)
        {
            resize(Content, width + 1, height);
            for (int x = width - 1; x > position; x--)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = map[x - 1, y];
                }
            }
            for (int y = 0; y < height; y++)
            {
                map[position, y] = Tile.EmptyTile(Content);
            }
        }

        public void deleteColumn(int position)
        {
            Tile[,] newtiles = new Tile[width - 1, height];
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile t = map[x, y];
                    if (x >= position) t = map[x + 1, y];
                    newtiles[x, y] = t;
                }
            }
            map = newtiles;
            width--;
        }

        // dirtiest pathfinding you've ever seen
        // TODO implement proper A* for this LOL
        public List<Point> computePath(Point start, Point end, Point player)
        {
            if (start == end) return new List<Point>();
            List<List<Point>> allPaths = new List<List<Point>>();
            allPaths.Add(new List<Point>(new Point[1] { start }));
            bool[,] visited = new bool[width, height];
            visited[start.X, start.Y] = true;

            int depth = 0;
            while (depth++ < 1000)
            {
                allPaths.Sort(new PathComparer());
                List<Point> shortest = allPaths[0];
                Point head = shortest[shortest.Count - 1];
                Point[] proposals = new Point[4] { 
                    new Point(head.X + 1, head.Y), 
                    new Point(head.X - 1, head.Y), 
                    new Point(head.X, head.Y + 1), 
                    new Point(head.X, head.Y - 1)
                };
                foreach (Point p in proposals)
                {
                    if (checkCollision(p) && !visited[p.X, p.Y] && player != p)
                    {
                        visited[p.X, p.Y] = true;
                        List<Point> newpath = new List<Point>(shortest);
                        newpath.Add(p);
                        if (p == end) return newpath; // shortest path start>end found!
                        allPaths.Add(newpath);
                    }
                }
                allPaths.Remove(allPaths[0]);
            }
            return new List<Point>(); // max depth reached - give up
        }

        public void draw(SBWrapper wrapper)
        {
            /*
            int startX = Math.Max(0, (int) Math.Floor(-wrapper.location.X / Tile.TILE_SIZE));
            int startY = Math.Max(0, (int)Math.Floor(-wrapper.location.Y / Tile.TILE_SIZE));
            for (int x = startX; x < Math.Min(width, Game1.TILE_WIDTH); x++)
            {
                for (int y = startY; y < Math.Min(height, Game1.TILE_HEIGHT); y++)
                {
                    map[x, y].draw(wrapper, new Vector2(x, y) * Tile.TILE_SIZE);
                }
            }
            */
            // TODO determine why above algorithm stopped working... drawing all tiles should work for now
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y].draw(wrapper, new Vector2(x, y) * Tile.TILE_SIZE);
                }
            }

        }

    }
}
