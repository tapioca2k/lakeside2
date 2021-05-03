using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    public class DelayNode : ScriptNode
    {
        double t;
        double delay;

        public DelayNode(double delay)
        {
            t = 0;
            this.delay = delay;
        }

        public override void start()
        {
            base.start();
            t = 0;
        }

        public override void update(double dt)
        {
            t += dt;
            finished = (t >= delay);
        }

    }
}
