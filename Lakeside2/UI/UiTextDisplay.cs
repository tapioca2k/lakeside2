using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiTextDisplay : UiElement
    {
        // I don't know where else to put this...
        public static string YesOrNo(bool val)
        {
            if (val) return "Yes";
            else return "No";
        }

        public override Vector2 size
        {
            get
            {
                string t = text;
                Vector2 measured = font.MeasureString(text);
                if (measured.X < 5) measured.X = 5;
                if (measured.Y < 5) measured.Y = 5;
                return measured;
            }
        }

        public string text
        {
            get;
            set;
        }

        protected SpriteFont font;

        public UiTextDisplay(SpriteFont font, string text)
        {
            this.font = font;
            this.text = text;
        }

        public UiTextDisplay()
        {
            this.font = Fonts.get("Arial");
            this.text = "";
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            wrapper.drawString(text, Vector2.Zero, Color.Black, font);
        }

    }
}
