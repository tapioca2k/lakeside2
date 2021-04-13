using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lakeside2
{
    class SerializableMap
    {
        const string MAPS_DIRECTORY = "/maps/";

        static JsonSerializerOptions OPTIONS;
        static SerializableMap()
        {
            OPTIONS = new JsonSerializerOptions();
            OPTIONS.Converters.Add(new NPCConverter());
        }

        public string[] tilenames { get; set; }
        public int[][] tiles { get; set; }
        public bool[][] collision { get; set; }
        public uint color { get; set; }
        public List<LuaScript> scripts { get; set; }
        public List<KeyValuePair<int, int>> scriptLocations { get; set; }
        public List<NPC> npcs { get; set; }

        // wrestle engine-useful TileMap format into less useful (but writable) SerializableMap
        public static SerializableMap FromTilemap(TileMap map)
        {
            SerializableMap s = new SerializableMap();

            Dictionary<string, int> tileNumbers = new Dictionary<string, int>();
            int tileCount = 0;

            s.tiles = new int[map.width][];;
            s.collision = new bool[map.width][];
            s.color = map.color.PackedValue;

            s.scripts = new List<LuaScript>();
            s.scriptLocations = new List<KeyValuePair<int, int>>();

            s.npcs = map.npcs;

            for (int x = 0; x < map.width; x++)
            {
                s.tiles[x] = new int[map.height];
                s.collision[x] = new bool[map.height];
                for (int y = 0; y < map.height; y++)
                {
                    Tile tile = map.map[x, y];
                    string tilename = tile.filename;
                    if (!tileNumbers.ContainsKey(tilename)) tileNumbers.Add(tilename, tileCount++);
                    s.tiles[x][y] = tileNumbers[tilename];
                    s.collision[x][y] = map.checkCollision(new Vector2(x, y));
                    if (tile.script != null)
                    {
                        s.scripts.Add(tile.script);
                        s.scriptLocations.Add(new KeyValuePair<int, int>(x, y));
                    }
                }
            }
            s.tilenames = new string[tileCount];
            tileNumbers.Keys.CopyTo(s.tilenames, 0);

            return s;
        }
        
        // the above process, but in reverse
        public static TileMap ToTilemap(ContentManager Content, string filename, SerializableMap s)
        {
            int width = s.tiles.Length, height = s.tiles[0].Length;
            Tile[,] tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0;y < height; y++)
                {
                    tiles[x, y] = new Tile(Content, s.tilenames[s.tiles[x][y]]);
                    tiles[x, y].collision = s.collision[x][y];
                }
            }
            for (int i = 0; i < s.scripts.Count; i++)
            {
                s.scripts[i].load();
                KeyValuePair<int, int> location = s.scriptLocations[i];
                tiles[location.Key, location.Value].script = s.scripts[i];
            }

            // load NPC textures
            s.npcs.ForEach(npc => npc.setTexture(Content, npc.filename));

            return new TileMap(tiles, new Color(s.color), filename, s.npcs);
        }

        public static TileMap Load(ContentManager Content, string filename)
        {
            //try
            //{
                string json = File.ReadAllText(Content.RootDirectory + MAPS_DIRECTORY + filename);
            Debug.WriteLine(json);
                SerializableMap s = JsonSerializer.Deserialize<SerializableMap>(json, OPTIONS);
                return ToTilemap(Content, filename, s);
            /*}
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("No map file found: " + filename);
                return null;
            }*/
        }

        public static void Save(ContentManager Content, TileMap map, string filename)
        {
            map.filename = filename;
            SerializableMap s = FromTilemap(map);
            string json = JsonSerializer.Serialize(s, OPTIONS);
            string location = Content.RootDirectory + MAPS_DIRECTORY + filename;
            File.WriteAllText(location, json);
        }
    }
}
