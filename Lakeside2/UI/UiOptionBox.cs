using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using static Lakeside2.InputBindings;

namespace Lakeside2.UI
{
    class UiOptionBox : UiTextBox
    {
        string option1;
        string option2;
        public int selected;
        Texture2D pointer;

        public UiOptionBox(ContentManager Content, string text, string option1, string option2) : base(text, false)
        {
            this.option1 = option1;
            this.option2 = option2;
            selected = 0;
            pointer = Content.Load<Texture2D>("pointer");
        }

        public override void onInput(InputHandler input)
        {
            if (input.isCommandPressed(Bindings.Interact))
            {
                MusicManager.playSfx("select");
                finished = true;
            }
            else if (input.isCommandPressed(Bindings.Up) || input.isCommandPressed(Bindings.Down))
            {
                selected = Math.Abs(--selected);
                MusicManager.playSfx("cursor");
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
