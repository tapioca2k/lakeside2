﻿using Lakeside2.Editor;
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
    public class NPC : Entity
    {
        public LuaScript script;
        public string filename;
        public bool locked;
        public string realName;

        string entityName;
        public override string name => entityName;

        public NPC(ContentManager Content, string filename, string scriptname, bool locked, string entityName, string realName)
        {
            loadAnimatedTexture(Content, filename);
            this.script = new LuaScript(scriptname);
            this.filename = filename;
            this.locked = locked;
            this.entityName = entityName;
            this.realName = realName;
        }

        // used by NPCConverter to deserialize NPCs without immediate access to asset loading
        public NPC(string filename, string scriptname, bool locked, string entityName, string realName)
        {
            this.script = new LuaScript(scriptname);
            this.filename = filename;
            this.locked = locked;
            this.entityName = entityName;
            this.realName = realName;
        }

        public void setScript(string filename)
        {
            script = new LuaScript(filename);
            // scripts that don't load are invalid but it's safe actually
            //if (!script.loaded) script = null; // not a valid script
        }

        public void setTexture(ContentManager Content, string filename)
        {
            loadAnimatedTexture(Content, filename);
            this.filename = filename;
        }

        public void setName(string name)
        {
            if (name != Player.ENTITY_NAME && name != Cursor.ENTITY_NAME) this.entityName = name;
        }

        public object[] interact(Lua lua)
        {
            if (script != null) return script.execute(this, lua);
            else return new object[0];
        }

        public new void setDirection(Directions direction)
        {
            if (this.locked) return;
            animation.set((int)direction);
            this.facing = direction;
        }

        public override string ToString()
        {
            return name;
        }

    }
}
