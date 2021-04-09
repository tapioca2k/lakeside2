using Microsoft.Xna.Framework;
using NLua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI.Scripting
{
    // not actually a UI element, just an excuse to call a function from within a ScriptChain
    class FunctionNode : UiElement
    {
        public override Vector2 size => Vector2.Zero;

        LuaFunction func;

        public FunctionNode(LuaFunction func)
        {
            this.func = func;
        }

        public override void update(double dt)
        {
            func.Call(new object[0]);
        }

        public override void onInput(InputHandler input)
        {
            // TODO right now finished has to be set in onInput if it plans to propagate any more ui elements at the same time
            // otherwise update immediately eats the new UiElement, then gets rid of this one next frame
            // Fix that somehow
            finished = true;
        }

        public override void draw(SBWrapper wrapper)
        {
        }
    }
}
