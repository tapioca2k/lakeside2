using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI
{
    class UiTextBox : UiElement
    {
        // time between characters scrolling, in seconds
        const double SCROLL_SPEED = .025;

        public override Vector2 size => new Vector2(Game1.INTERNAL_WIDTH, 60);

        UiTextDisplay textdisp;
        string fullText;

        int c;
        int cursor
        {
            get
            {
                return c;
            }
            set
            {
                c = value;
                if (textdisp != null) textdisp.text = fullText.Substring(0, c);
            }
        }

        double t;


        public UiTextBox(string text, bool scroll)
        {
            setBackground(Color.White, true);
            textdisp = new UiTextDisplay("");
            fullText = text;
            if (scroll) cursor = 0;
            else cursor = fullText.Length;
            t = 0;
        }

        public override void onInput(InputHandler input)
        {
            if (input.isCommandPressed(Bindings.Interact))
            {
                if (cursor < fullText.Length) cursor = fullText.Length;
                else finished = true;
            }
        }

        public override void update(double dt)
        {
            base.update(dt);
            if (cursor < fullText.Length)
            {
                t += dt;
                if (t >= dt)
                {
                    t = 0;
                    cursor++;
                }
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            textdisp.draw(new SBWrapper(wrapper, new Vector2(5, 5)));
        }
    }
}
