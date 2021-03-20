using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
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

        public void input(InputHandler ih)
        {
            if (finished) return;
            else if (ih.isKeyPressed(Keys.Enter)) finished = true;
            else if (ih.isKeyPressed(Keys.Escape))
            {
                finished = true;
                text = "";
            }
            else if (ih.isKeyPressed(Keys.Back))
            {
                text = text.Substring(0, text.Length - 1);
            }
            else
            {
                foreach (Keys k in ih.heldKeys)
                {
                    if (ih.isKeyPressed(k))
                    {
                        if (TRANSLATION.ContainsKey(k)) text += TRANSLATION[k];
                        else text += k.ToString().ToLower();
                    }
                }
            }
        }


    }
}
