using Lakeside2.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Lakeside2
{
    public static class GameInfo
    {
        public static string title { get; set; }
        public static string titleBackground { get; set; }
        public static string startMap { get; set; }
        public static bool startOverworld { get; set; }


        static GameInfo()
        {
            string json = File.ReadAllText("Content/game.json");
            SerializableGameInfo gi = JsonSerializer.Deserialize<SerializableGameInfo>(json);

            title = gi.title;
            titleBackground = gi.titleBackground;
            startMap = gi.startMap;
            startOverworld = gi.startOverworld;
        }

        public static void save()
        {
            SerializableGameInfo gi = new SerializableGameInfo();
            gi.title = title;
            gi.titleBackground = titleBackground;
            gi.startMap = startMap;
            gi.startOverworld = startOverworld;

            string json = JsonSerializer.Serialize(gi);
            File.WriteAllText("Content/game.json", json);
        }

    }
}
