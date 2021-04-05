using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

    }
}
