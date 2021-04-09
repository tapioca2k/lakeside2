using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiOptionBox : UiTextBox
    {
        string option1;
        string option2;
        public int selected;
        Texture2D pointer;

        public UiOptionBox(ContentManager Content, string text, string option1, string option2) : base(text)
        {
            this.option1 = option1;
            this.option2 = option2;
            selected = 0;
            pointer = Content.Load<Texture2D>("pointer");
        }

        public override void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.E))
            {
                finished = true;
            }
            else if (input.isAnyKeyPressed(Keys.W, Keys.S))
            {
                selected = Math.Abs(--selected);
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            base.draw(wrapper);
            wrapper.drawString(option1, new Vector2(25, 25));
            wrapper.drawString(option2, new Vector2(25, 40));
            wrapper.draw(pointer, new Vector2(5, 25 + (selected * 15)));
        }
    }
}
