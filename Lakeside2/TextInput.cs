using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class TextInput
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


        SpriteFont font;

        public Vector2 position;
        public string typed;
        public bool finished;

        public TextInput(SpriteFont font, Vector2 position)
        {
            this.font = font;
            this.position = position;
            this.finished = false;
            this.typed = "";
        }

        static void keyPressed(object sender, EventArgs e)
        {

        } 

        public void update(InputHandler ih)
        {
            if (finished) return;
            else if (ih.isKeyPressed(Keys.Enter)) finished = true;
            else if (ih.isKeyPressed(Keys.Escape))
            {
                finished = true;
                typed = "";
            }
            else if (ih.isKeyPressed(Keys.Back))
            {
                typed = typed.Substring(0, typed.Length - 1);
            }
            else
            {
                foreach (Keys k in ih.heldKeys)
                {
                    if (ih.isKeyPressed(k))
                    {
                        if (TRANSLATION.ContainsKey(k)) typed += TRANSLATION[k];
                        else typed += k.ToString().ToLower();
                    }
                }
            }
        }

        public void draw(SpriteBatch sb)
        {
            sb.DrawString(font, typed, position, Color.Black);
        }


    }
}
