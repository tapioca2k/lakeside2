using Microsoft.Xna.Framework;
using NLua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    // ScriptNode that executes a lua function on its first update
    class FunctionNode : ScriptNode
    {

        LuaFunction func;

        public FunctionNode(LuaFunction func)
        {
            this.func = func;
        }

        public override void update(double dt)
        {
            func.Call(new object[0]);
            finished = true;
        }

    }
}
