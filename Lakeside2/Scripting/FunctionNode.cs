using Microsoft.Xna.Framework;
using NLua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    // ScriptNode that executes a lua function on its first update
    public class FunctionNode : ActionNode
    {

        public FunctionNode(LuaFunction func) : base(() =>
        {
            func.Call(new object[0]);
        })
        { }

    }
}
