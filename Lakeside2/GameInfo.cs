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
        public static int resolution { get; set; }

        static GameInfo()
        {
            string json = File.ReadAllText("Content/game.json");
            SerializableGameInfo gi = JsonSerializer.Deserialize<SerializableGameInfo>(json);

            title = gi.title;
            titleBackground = gi.titleBackground;
            startMap = gi.startMap;
            startOverworld = gi.startOverworld;
            resolution = gi.resolution;
        }

        public static void save()
        {
            SerializableGameInfo gi = new SerializableGameInfo();
            gi.title = title;
            gi.titleBackground = titleBackground;
            gi.startMap = startMap;
            gi.startOverworld = startOverworld;
            gi.resolution = resolution;

            string json = JsonSerializer.Serialize(gi);
            File.WriteAllText("Content/game.json", json);
        }

        public static int getResolutionScale()
        {
            switch (resolution)
            {
                case 0: return 1;
                case 1: return 2;
                case 2: return 4;
                case 3: return 6;
                default: return 1;
            }
        }

    }
}
