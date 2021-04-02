using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class Fonts
    {
        const string FONTS = "fonts/";
        static Dictionary<string, SpriteFont> fonts;

        static Fonts()
        {
            fonts = new Dictionary<string, SpriteFont>();
        }

        public static SpriteFont loadFont(ContentManager Content, string filename)
        {
            try
            {
                SpriteFont f = Content.Load<SpriteFont>(FONTS + filename);
                fonts.Add(filename, f);
                return f;
            }
            catch (ContentLoadException e)
            {
                return null;
            }
        }

        public static SpriteFont get(string name)
        {
            if (fonts.ContainsKey(name)) return fonts[name];
            else return null;
        }
    }
}
