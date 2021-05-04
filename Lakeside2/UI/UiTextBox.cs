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
        public override Vector2 size => new Vector2(Game1.INTERNAL_WIDTH, 60);

        UiTextDisplay textdisp;

        public UiTextBox(string text)
        {
            setBackground(Color.White);
            textdisp = new UiTextDisplay(text);
        }

        public override void onInput(InputHandler input)
        {
            if (input.isCommandPressed("interact"))
            {
                finished = true;
            }
        }

        public override void update(double dt)
        {
            base.update(dt);
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            textdisp.draw(new SBWrapper(wrapper, new Vector2(5, 5)));
        }
    }
}
