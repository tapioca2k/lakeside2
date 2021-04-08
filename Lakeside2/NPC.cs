using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Lakeside2
{
    class NPC : Entity
    {
        public LuaScript script;
        public string filename;

        public NPC(ContentManager Content, string filename, string scriptname)
        {
            loadAnimatedTexture(Content, filename);
            this.script = new LuaScript(scriptname);
            this.filename = filename;
        }

        public NPC(string filename, string scriptname)
        {
            this.script = new LuaScript(scriptname);
            this.filename = filename;
            // defer any loading for later using the set() methods
        }

        public void setScript(string filename)
        {
            script = new LuaScript(filename);
            if (!script.loaded) script = null; // not a valid script
        }

        public void setTexture(ContentManager Content, string filename)
        {
            loadAnimatedTexture(Content, filename);
            this.filename = filename;
        }

        public object[] interact(Lua lua)
        {
            lua["me"] = this;
            return script.execute(lua);
        }

        public override string ToString()
        {
            return filename;
        }

    }
}
