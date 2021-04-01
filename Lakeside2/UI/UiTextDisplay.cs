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
            return val ? "Yes" : "No";
        }
        public static string HasText(string val)
        {
            return val == "" || val == null ? "(None)" : val;
        }
        public static string TextOrNull(object val)
        {
            if (val == null) return "(None)";
            else return val.ToString();
        }

        public override Vector2 size
        {
            get
            {
                string t = text;
                Vector2 measured = font.MeasureString(prefix + text);
                if (measured.X < 5) measured.X = 5;
                if (measured.Y < 5) measured.Y = 5;
                return measured;
            }
        }

        string prefix = "";

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
            this.font = Fonts.get("rainyhearts");
            this.text = "";
        }

        public UiTextDisplay(string text)
        {
            this.font = Fonts.get("rainyhearts");
            this.text = text;
        }

        public void setPrefix (string prefix)
        {
            this.prefix = prefix;
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            wrapper.drawString(prefix + text, Vector2.Zero, Color.Black, font);
        }

    }
}
