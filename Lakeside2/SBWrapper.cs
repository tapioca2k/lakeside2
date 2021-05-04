using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    // SpriteBatch wrapper for certain very common operations in draw calls
    public class SBWrapper
    {
        public SpriteBatch spriteBatch;
        public Vector2 location;

        public SBWrapper(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.location = Vector2.Zero;
        }

        public SBWrapper(SBWrapper other)
        {
            spriteBatch = other.spriteBatch;
            this.location = other.location;
        }

        public SBWrapper(SpriteBatch spriteBatch, Vector2 location)
        {
            this.spriteBatch = spriteBatch;
            this.location = location;
        }

        public SBWrapper(SBWrapper other, Vector2 location)
        {
            spriteBatch = other.spriteBatch;
            this.location = other.location + location;
        }

        public SBWrapper setOrigin(Vector2 origin)
        {
            this.location = origin;
            return this;
        }

        public void draw(Texture2D texture)
        {
            spriteBatch.Draw(texture, location, Color.White);
        }

        public void draw(Texture2D texture, Vector2 location)
        {
            spriteBatch.Draw(texture, this.location + location, Color.White);
        }

        public void draw(Texture2D texture, Vector2 location, Rectangle section)
        {
            spriteBatch.Draw(texture, this.location + location, section, Color.White);
        }

        public void drawRectangle(Vector2 size, Texture2D pixel)
        {
            spriteBatch.Draw(pixel, new Rectangle(location.ToPoint(), size.ToPoint()), Color.White);
        }

        public void drawRectangle(Vector2 size, Color color)
        {
            drawRectangle(size, color, this.location);
        }

        public void drawRectangle(Vector2 size, Color color, Vector2 location)
        {
            spriteBatch.Draw(Game1.WHITE_PIXEL, new Rectangle(location.ToPoint(), size.ToPoint()), color);
        }

        public void drawString(string text)
        {
            drawString(text, Vector2.Zero);
        }

        public void drawString(string text, Vector2 location)
        {
            drawString(text, location, Color.Black);
        }

        public void drawString(string text, Vector2 location, Color color)
        {
            drawString(text, location, color, Fonts.get("rainyhearts"));
        }

        public void drawString(string text, Vector2 location, Color color, SpriteFont font)
        {
            spriteBatch.DrawString(font, text, this.location + location, color);
        }


    }
}
