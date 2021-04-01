using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiTextInput : UiTextDisplay
    {
        static Dictionary<Keys, char> TRANSLATION = new Dictionary<Keys, char>()
        {
            { Keys.D0, '0' },
            { Keys.D1, '1' },
            { Keys.D2, '2' },
            { Keys.D3, '3' },
            { Keys.D4, '4' },
            { Keys.D5, '5' },
            { Keys.D6, '6' },
            { Keys.D7, '7' },
            { Keys.D8, '8' },
            { Keys.D9, '9' },
            { Keys.Space, ' ' },
            { Keys.OemPeriod, '.' }
        };

        public UiTextInput(SpriteFont font) : base(font, "")
        {
        }

        public UiTextInput() : base()
        {
            setBackground(Color.White);
        }

        public UiTextInput(string prefix) : base()
        {
            setBackground(Color.White);
            setPrefix(prefix);
        }

        public override void onInput(InputHandler input)
        {
            if (finished) return;
            else if (input.isKeyPressed(Keys.Enter)) finished = true;
            else if (input.isKeyPressed(Keys.Escape))
            {
                finished = true;
                text = "";
            }
            else if (input.isKeyPressed(Keys.Back))
            {
                text = text.Substring(0, text.Length - 1);
            }
            else
            {
                foreach (Keys k in input.heldKeys)
                {
                    if (input.isKeyPressed(k))
                    {
                        if (TRANSLATION.ContainsKey(k)) text += TRANSLATION[k];
                        else text += k.ToString().ToLower();
                    }
                }
            }
        }


    }
}
