using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    public interface IGameState
    {
        public Color background { get; }
        public void onInput(InputHandler input);
        public void update(double dt);
        public void draw(SBWrapper wrapper);
    }
}
