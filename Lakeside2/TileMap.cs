﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2
{
    public class PathComparer : IComparer<List<Vector2>>
    {
        public int Compare(List<Vector2> a, List<Vector2> b)
        {
            return b.Count - a.Count;
        }
    }

    class TileMap
    {
        ContentManager Content;
        public Tile[,] map;
        public int width;
        public int height;
        public Color color;
        public string filename;
        public List<NPC> npcs;

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
            npcs = new List<NPC>();
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
        public TileMap(Tile[,] tiles, Color color, string filename, List<NPC> npcs)
        {
            this.map = tiles;
            this.width = tiles.GetLength(0);
            this.height = tiles.GetLength(1);
            this.color = color;
            this.filename = filename;
            this.npcs = npcs;
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

        public void setNPC(Vector2 tileLocation, NPC npc)
        {
            NPC present = getNPC(tileLocation);
            if (present != null) npcs.Remove(present);
            npc.setTileLocation(tileLocation);
            npcs.Add(npc);
        }

        public NPC getNPC(Vector2 tileLocation)
        {
            return npcs.Find(npc => npc.getTileLocation().Equals(tileLocation));
        }

        public bool checkCollision(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            else if (getNPC(new Vector2(x, y)) != null) return false;
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

        // dirtiest pathfinding you've ever seen (TODO proper A* lol)
        public List<Vector2> computePath(Vector2 start, Vector2 end)
        {
            List<List<Vector2>> allPaths = new List<List<Vector2>>();
            allPaths.Add(new List<Vector2>(new Vector2[1] { start }));
            bool[,] visited = new bool[width, height];
            visited[(int)start.X, (int)start.Y] = true;

            int depth = 0;
            while (depth++ < 1000)
            {
                allPaths.Sort(new PathComparer());
                List<Vector2> shortest = new List<Vector2>(allPaths[allPaths.Count - 1]);
                Vector2 head = shortest[0];
                Vector2[] proposals = new Vector2[4] { 
                    new Vector2(head.X + 1, head.Y), 
                    new Vector2(head.X - 1, head.Y), 
                    new Vector2(head.X, head.Y + 1), 
                    new Vector2(head.X, head.Y - 1)
                };
                bool deadEnd = true;
                foreach (Vector2 p in proposals)
                {
                    if (checkCollision(p) && !visited[(int)p.X, (int)p.Y])
                    {
                        deadEnd = false;
                        visited[(int)p.X, (int)p.Y] = true;
                        List<Vector2> newpath = new List<Vector2>(shortest);
                        newpath.Add(p);
                        allPaths.Add(newpath);
                        if (p == end) return newpath;
                    }
                }
                if (deadEnd) allPaths.Remove(shortest); // stop checking this path
            }
            return new List<Vector2>();
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
