using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Serialization
{
    // non-static version of GameInfo.cs for saving/loading
    public class SerializableGameInfo
    {
        public string title { get; set; }
        public string titleBackground { get; set; }
        public string startMap { get; set; }
        public bool startOverworld { get; set; }
    }
}
