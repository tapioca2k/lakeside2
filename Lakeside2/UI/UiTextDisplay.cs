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
        public override Vector2 size
        {
            get
            {
                string t = text;
                return font.MeasureString(text);
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


        public override void draw(SpriteBatch spriteBatch, Vector2 location)
        {
            spriteBatch.DrawString(font, text, location, Color.Black);
        }

    }
}
