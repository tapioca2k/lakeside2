using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    /// <summary>
    /// SBWrapper creates a wrapper around SpriteBatch draw calls that allows the programmer to
    /// write more legible drawing code elsewhere, and reduces the amount of math that needs to
    /// be done when drawing multiple elements relative to non-zero origin points.
    /// </summary>
    public class SBWrapper
    {
        public SpriteBatch spriteBatch;
        public Vector2 location;

        /// <summary>
        /// Create a new SBWrapper with origin Vector2.Zero
        /// </summary>
        /// <param name="spriteBatch"></param>
        public SBWrapper(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.location = Vector2.Zero;
        }

        /// <summary>
        /// Create a new SBWrapper that is a copy of another
        /// </summary>
        /// <param name="other">SBWrapper to copy</param>
        public SBWrapper(SBWrapper other)
        {
            spriteBatch = other.spriteBatch;
            this.location = other.location;
        }

        /// <summary>
        /// Create a new SBWrapper with a non-zero origin point
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="location">Origin for drawing</param>
        public SBWrapper(SpriteBatch spriteBatch, Vector2 location)
        {
            this.spriteBatch = spriteBatch;
            this.location = location;
        }

        /// <summary>
        /// Create a new SBWrapper that draws at an offset relative to another SBWrapper
        /// </summary>
        /// <param name="other">The other SBWrapper</param>
        /// <param name="location">Offset from the origin of <paramref name="other"/> to use as the new origin</param>
        public SBWrapper(SBWrapper other, Vector2 location)
        {
            spriteBatch = other.spriteBatch;
            this.location = other.location + location;
        }

        /// <summary>
        /// Change the origin of the wrapper.
        /// It is not recommended to use this function as it greatly complicates drawing multiple things.
        /// </summary>
        /// <param name="origin">New origin</param>
        /// <returns></returns>
        public SBWrapper setOrigin(Vector2 origin)
        {
            this.location = origin;
            return this;
        }

        /// <summary>
        /// Draw a texture at the origin.
        /// </summary>
        /// <param name="texture">Texture to draw</param>
        public void draw(Texture2D texture)
        {
            spriteBatch.Draw(texture, location, Color.White);
        }

        /// <summary>
        /// Draw a texture at an offset from the origin
        /// </summary>
        /// <param name="texture">Texture to draw</param>
        /// <param name="location">Offset from origin to draw at</param>
        public void draw(Texture2D texture, Vector2 location)
        {
            spriteBatch.Draw(texture, this.location + location, Color.White);
        }

        /// <summary>
        /// Draw a section of a texture at an offset from the origin
        /// </summary>
        /// <param name="texture">Texture to draw</param>
        /// <param name="location">Offset from origin to draw at</param>
        /// <param name="section">Section of texture to draw</param>
        public void draw(Texture2D texture, Vector2 location, Rectangle section)
        {
            spriteBatch.Draw(texture, this.location + location, section, Color.White);
        }

        /// <summary>
        /// Draw a rectangle made out of a texture at the origin
        /// </summary>
        /// <param name="size">Size of rectangle to draw</param>
        /// <param name="pixel">Source texture. Ideally should be a 1x1 pixel or other flat color</param>
        public void drawRectangle(Vector2 size, Texture2D pixel)
        {
            spriteBatch.Draw(pixel, new Rectangle(location.ToPoint(), size.ToPoint()), Color.White);
        }

        /// <summary>
        /// Draw a rectangle of a color at the origin
        /// </summary>
        /// <param name="size">Size of rectangle to draw</param>
        /// <param name="color">Color of rectangle to draw</param>
        public void drawRectangle(Vector2 size, Color color)
        {
            drawRectangle(size, color, this.location);
        }

        /// <summary>
        /// Draw a rectangle of a color at an offset from the origin
        /// </summary>
        /// <param name="size">Size of rectangle to draw</param>
        /// <param name="color">Color of rectangle to draw</param>
        /// <param name="location">Offset from origin to draw at</param>
        public void drawRectangle(Vector2 size, Color color, Vector2 location)
        {
            spriteBatch.Draw(Game1.WHITE_PIXEL, new Rectangle(location.ToPoint(), size.ToPoint()), color);
        }

        /// <summary>
        /// Draw a string at the origin
        /// </summary>
        /// <param name="text">String to draw</param>
        public void drawString(string text)
        {
            drawString(text, Vector2.Zero);
        }

        /// <summary>
        /// Draw a string at an offset from the origin
        /// </summary>
        /// <param name="text">String to draw</param>
        /// <param name="location">Offset from origin to draw at</param>
        public void drawString(string text, Vector2 location)
        {
            drawString(text, location, Color.Black);
        }

        /// <summary>
        /// Draw a string in a color at an offset from the origin
        /// </summary>
        /// <param name="text">String to draw</param>
        /// <param name="location">Offset from origin to draw at</param>
        /// <param name="color">Color to draw in</param>
        public void drawString(string text, Vector2 location, Color color)
        {
            drawString(text, location, color, Fonts.get("rainyhearts"));
        }

        /// <summary>
        /// Draw a string in a color and font at an offset from the origin
        /// </summary>
        /// <param name="text">String to draw</param>
        /// <param name="location">Offset from origin to draw at</param>
        /// <param name="color">Color to draw in</param>
        /// <param name="font">Font to draw in</param>
        public void drawString(string text, Vector2 location, Color color, SpriteFont font)
        {
            spriteBatch.DrawString(font, text, this.location + location, color);
        }


    }
}
