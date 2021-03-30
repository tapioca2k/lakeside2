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
    class NPC : IEntity
    {

        Texture2D texture;
        Vector2 location;
        public LuaScript script;

        public NPC(ContentManager Content, string filename, string scriptname, Vector2 location)
        {
            this.texture = Content.Load<Texture2D>(filename);
            this.location = location;
            this.script = new LuaScript(scriptname);
        }

        public Vector2 getLocation()
        {
            return location;
        }

        public void update(double dt)
        {
            // animate, walk around, etc?
        }

        public void draw(SBWrapper wrapper, TilemapCamera camera)
        {
            wrapper.draw(texture, camera.worldToScreen(location));
        }


    }
}
