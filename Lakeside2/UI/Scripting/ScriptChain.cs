using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI.Scripting
{
    class ScriptChain : UiElement
    {

        UiScriptNode head;

        public override Vector2 size => Vector2.Zero;

        public ScriptChain(UiScriptNode head)
        {
            this.head = head;
        }

        public override void update(double dt)
        {
            if (head != null)
            {
                head.update(dt);
                if (head.finished)
                {
                    head = head.next;
                }
            }

            if (head == null)
            {
                finished = true;
            }
        }

        public override void onInput(InputHandler input)
        {
            if (head != null)
            {
                head.onInput(input);
                if (head.finished)
                {
                    head = head.next;
                }
            }

            if (head == null)
            {
                finished = true;
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            if (head != null) head.draw(wrapper);
        }

    }
}
