using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    // ScriptNode that executes a bit of code on its first update
    public class ActionNode : ScriptNode
    {

        Action action;

        public ActionNode(Action action)
        {
            this.action = action;
        }

        public override void start()
        {
            action.Invoke();
            finished = true;
            base.start();
        }
    }
}
