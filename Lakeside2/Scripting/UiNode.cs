using Lakeside2.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    public class UiNode : ScriptNode
    {

        UiElement element;
        UiSystem system;

        public UiNode(UiSystem system, UiElement element)
        {
            this.element = element;
            this.system = system;
        }

        public override void start()
        {
            system.pushElement(element, Point.Zero);
            base.start();
        }

        public override void update(double dt)
        {
            this.finished = element.finished;
        }
    }
}
