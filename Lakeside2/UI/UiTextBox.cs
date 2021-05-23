using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Lakeside2.InputBindings;

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

        // wrap using measured size of string (good)
        public static string WrapString(string unwrapped, SpriteFont font, int maxWidth)
        {
            int cursor = 1, last = 0;
            string wrapped = "";
            while (cursor < unwrapped.Length)
            {
                string line = unwrapped.Substring(last, cursor - last);
                if (font.MeasureString(line).X > maxWidth) // found end of line
                {
                    int flyback = cursor;
                    while (flyback > last) // look back for last work break
                    {
                        char c = unwrapped[flyback];
                        if (c == ' ') // found place to wrap on
                        {
                            wrapped += unwrapped.Substring(last, flyback - last) + "\n";
                            break;
                        }
                        flyback--;
                    }
                    if (flyback <= last) // couldn't find a space in this entire line. arbitrary break mid-word
                    {
                        wrapped += unwrapped.Substring(last, cursor - last) + "\n";
                        last = cursor;
                        cursor = last + 1;
                    }
                    else
                    {
                        last = flyback += 1;
                        cursor = last + 1;
                    }
                    continue;
                }
                cursor++;
            }
            wrapped += unwrapped.Substring(last, unwrapped.Length - last); // clean up
            return wrapped;
        }

        public UiTextBox(string text, bool scroll)
        {
            setBackground(Color.White, true);
            textdisp = new UiTextDisplay("");
            fullText = WrapString(text, textdisp.font, Game1.INTERNAL_WIDTH - 10);
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
