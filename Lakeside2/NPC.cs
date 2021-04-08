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
        public bool locked;

        public NPC(ContentManager Content, string filename, string scriptname, bool locked)
        {
            loadAnimatedTexture(Content, filename);
            this.script = new LuaScript(scriptname);
            this.filename = filename;
            this.locked = locked;
        }

        // used by NPCConverter to deserialize NPCs without immediate access to asset loading
        public NPC(string filename, string scriptname, bool locked)
        {
            this.script = new LuaScript(scriptname);
            this.filename = filename;
            this.locked = locked;
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

        public new void setDirection(Directions direction)
        {
            if (this.locked) return;
            animation.set((int)direction);
            this.facing = direction;
        }

        public override string ToString()
        {
            return filename;
        }

    }
}
