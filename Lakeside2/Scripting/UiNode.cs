using Lakeside2.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    class UiNode : ScriptNode
    {

        UiElement element;

        public UiNode(UiSystem system, UiElement element)
        {
            this.element = element;
            system.pushElement(element, Vector2.Zero);
        }

        public override void update(double dt)
        {
            this.finished = element.finished;
        }
    }
}
