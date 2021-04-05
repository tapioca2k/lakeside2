using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    interface IDrawable
    {
        public void draw(SBWrapper wrapper, Vector2 location);
    }
}
