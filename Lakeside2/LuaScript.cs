using Microsoft.Xna.Framework;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Lakeside2
{

    public class LuaScript
    {
        const string PREFIX = "Content/scripts/";

        public string filename { get; set; }
        public Point location { get; set; }
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
            if (!loaded) return new object[0];
            lua["me"] = caller;
            object[] returnvals = lua.DoString(script);
            return returnvals;
        }

        public object[] callFunction(string name, Lua lua)
        {
            if (!loaded) return new object[0];
            LuaFunction fn = lua.GetFunction(name);
            if (fn != null) return fn.Call(new object[0]);
            else return new object[0];
        }

        public Point getTileLocation()
        {
            return location;
        }

        public override string ToString()
        {
            return filename;
        }
    }
}
