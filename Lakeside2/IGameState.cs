using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    public interface IGameState
    {
        public void onInput(InputHandler input);
        public void update(double dt);
        public void draw(SBWrapper wrapper);
    }
}
