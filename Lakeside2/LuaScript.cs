using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Lakeside2
{

    class LuaScript
    {
        const string PREFIX = "Content/scripts/";

        public string filename { get; set; }
        public bool loaded;
        string script;

        public LuaScript(string filename)
        {
            this.filename = filename;
            load();
        }

        public LuaScript()
        {
        }

        public void load()
        {
            if (loaded || this.filename == "" || this.filename == null) return;

            try
            {
                script = File.ReadAllText(PREFIX + filename);
                loaded = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Script not found: " + filename);
                script = "";
                loaded = false;
            }
        }

        public object[] execute(Entity caller, Lua lua)
        {
            lua["me"] = caller;
            object[] returnvals = lua.DoString(script);
            return returnvals;
        }

        public override string ToString()
        {
            return filename;
        }
    }
}
