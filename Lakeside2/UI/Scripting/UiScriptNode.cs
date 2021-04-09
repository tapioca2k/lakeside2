using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI.Scripting
{
    class UiScriptNode : UiElement
    {
        public UiScriptNode next;
        public UiElement element;

        public override Vector2 size => element.size;

        public UiScriptNode(UiElement element)
        {
            this.element = element;
        }

        public override void update(double dt)
        {
            element.update(dt);
            finished = element.finished;
        }

        public override void onInput(InputHandler input)
        {
            element.onInput(input);
            finished = element.finished;
        }

        public override void draw(SBWrapper wrapper)
        {
            element.draw(wrapper);
        }
    }
}
