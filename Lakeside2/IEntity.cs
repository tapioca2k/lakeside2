using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    interface IEntity
    {
        public Vector2 getLocation();
        public void draw(SBWrapper wrapper, TilemapCamera camera);
    }
}
