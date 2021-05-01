using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Lakeside2.Serialization
{
    class SaveGame
    {
        public Dictionary<string, int> flags { get; set; }
        public Dictionary<string, string> strings { get; set; }
        public string map { get; set; }
        public bool overworld { get; set; }
        public Dictionary<Item, int> inventory { get; set; }
        public Vector2 location { get; set; }

        public static void Save(string filename, Player player, string map, bool overworld)
        {
            SaveGame save = new SaveGame
            {
                flags = Flags.getAllFlags(),
                strings = Flags.getAllStrings(),
                map = map,
                overworld = overworld,
                inventory = player.getInventory(),
                location = player.getTileLocation()
            };

            string json = JsonSerializer.Serialize(save, SerializableMap.OPTIONS);
            File.WriteAllText("save/" + filename, json);
        }

        public static SaveGame Load(ContentManager Content, string filename)
        {
            string json = File.ReadAllText("save/" + filename);
            SaveGame save = JsonSerializer.Deserialize<SaveGame>(json, SerializableMap.OPTIONS);
            return save;
        }
    }
}
