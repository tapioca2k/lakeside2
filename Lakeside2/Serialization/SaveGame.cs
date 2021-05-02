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
        public Dictionary<string, int> inventory { get; set; }
        public Vector2 location { get; set; }

        public double time { get; set; }

        public static void Save(string filename, Player player, string map, bool overworld)
        {
            // convert inventory into something serializable
            Dictionary<string, int> sInventory = new Dictionary<string, int>();
            foreach (Item i in player.getInventory().Keys)
            {
                sInventory.Add(i.name, player.getInventory()[i]);
            }

            SaveGame save = new SaveGame
            {
                flags = Flags.getAllFlags(),
                strings = Flags.getAllStrings(),
                map = map,
                overworld = overworld,
                inventory = sInventory,
                location = player.getTileLocation(),
                time = TimeOfDay.getTime().TotalMilliseconds
            };

            string json = JsonSerializer.Serialize(save, SerializableMap.OPTIONS);
            File.WriteAllText("save/" + filename, json);
        }

        public static SaveGame Load(ContentManager Content, string filename)
        {
            string json = File.ReadAllText("save/" + filename + ".json");
            SaveGame save = JsonSerializer.Deserialize<SaveGame>(json, SerializableMap.OPTIONS);
            return save;
        }
    }
}
