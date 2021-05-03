using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    // ScriptNode that runs multiple nodes at once
    public class MultiplexNode : ScriptNode
    {
        ScriptNode[] nodes;

        public MultiplexNode(params ScriptNode[] nodes)
        {
            this.nodes = nodes;
        }

        public override void start()
        {
            base.start();
            foreach (ScriptNode node in nodes)
                node.start();
        }

        public override void update(double dt)
        {
            bool allFinished = true;
            foreach (ScriptNode node in nodes)
            {
                node.update(dt);
                if (!node.finished) allFinished = false;
            }
            this.finished = allFinished;
        }
    }
}
