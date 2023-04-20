using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiTexture : UiElement
    {

        Texture2D texture;
        public override Vector2 size => new Vector2(texture.Width, texture.Height);
        
        public UiTexture(ContentManager Content, string filename)
        {
            setTexture(Content, filename);
        }

        public void setTexture(ContentManager Content, string filename)
        {
            texture = Content.Load<Texture2D>(filename);
        }

        public override void draw(SBWrapper wrapper)
        {
            wrapper.draw(texture);
        }

    }
}
