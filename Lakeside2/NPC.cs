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

        public NPC(ContentManager Content, string filename, string scriptname, Vector2 location)
        {
            loadAnimatedTexture(Content, filename);
            this.script = new LuaScript(scriptname);
        }

    }
}
