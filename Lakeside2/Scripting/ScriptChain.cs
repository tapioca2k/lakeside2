using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.Scripting
{
    class ScriptChain
    {

        ScriptNode head;
        public bool finished;

        public ScriptChain(ScriptNode head)
        {
            this.head = head;
            this.head.start();
            this.finished = false;
        }

        public void update(double dt)
        {
            if (head != null)
            {
                head.update(dt);
                if (head.finished)
                {
                    head = head.next;
                    if (head != null) head.start(); // prevent frame gaps
                    else finished = true;
                }
            }
        }

    }
}
