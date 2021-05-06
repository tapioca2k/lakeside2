using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI
{
    class UiLeftRightPicker : UiTextDisplay
    {
        string largestString;
        Vector2 lSize = Vector2.Zero;
        public override Vector2 size => new Vector2(lSize.X + (20 * 2), 16 + 2);

        Texture2D leftArrow, rightArrow;

        string[] options;
        int selected;
        string selectedString
        {
            get
            {
                if (selected < 0 || selected >= options.Length) return null;
                else return options[selected];
            }
        }

        public UiLeftRightPicker(ContentManager Content, string[] options) : base()
        {
            leftArrow = Content.Load<Texture2D>("pointer-left");
            rightArrow = Content.Load<Texture2D>("pointer");
            this.options = options;
            this.selected = 0;
            this.text = selectedString;

            // calculate the size of the element only one time
            largestString = selectedString;
            for (int i = 0; i < options.Length; i++)
            {
                Vector2 stringSize = font.MeasureString(options[i]);
                if (stringSize.X > lSize.X)
                {
                    largestString = options[i];
                    lSize = stringSize;
                }
            }
        }

        public override void onInput(InputHandler input)
        {
            if (input.isCommandPressed("move_left")) selected--;
            else if (input.isCommandPressed("move_right")) selected++;
            if (selected < 0) selected = options.Length - 1;
            else if (selected >= options.Length) selected = 0;
            this.text = selectedString;

            if (input.isCommandPressed("select")) finished = true;
            else if (input.isCommandPressed("back"))
            {
                finished = true;
                selected = -1;
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            wrapper.draw(leftArrow, new Vector2(2, 1));
            base.draw(new SBWrapper(wrapper, new Vector2(20, 0)));
            wrapper.draw(rightArrow, new Vector2(20 + lSize.X + 2, 1));
        }

    }
}
