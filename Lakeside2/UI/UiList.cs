﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI
{
    class UiList : UiElement
    {
        const int LEFT_BUFFER = 25;
        const int RIGHT_BUFFER = 5;
        const int Y_BUFFER = 5;
        const int SPACING = 15;

        public override Vector2 size
        {
            get
            {
                float x = 0, y = 0;
                options.ForEach(text =>
                {
                    if (text.size.X > x) x = text.size.X;
                    y += text.size.Y;
                });
                return new Vector2(LEFT_BUFFER + x + RIGHT_BUFFER, y + Y_BUFFER);
            }
        }

        Texture2D pointer;
        List<UiTextDisplay> options;
        public int selected;

        public UiList(ContentManager Content, string[] options)
        {
            setBackground(Color.White);
            selected = 0;
            pointer = Content.Load<Texture2D>("pointer");
            this.options = new List<UiTextDisplay>();
            for (int i = 0; i < options.Length; i++)
            {
                this.options.Add(new UiTextDisplay(options[i]));
            }
        }

        public override void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.W) && --selected < 0) selected = this.options.Count - 1;
            else if (input.isKeyPressed(Keys.S) && ++selected == this.options.Count) selected = 0;
            else if (input.isKeyPressed(Keys.E)) finished = true;
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            int y = Y_BUFFER;
            wrapper.draw(pointer, new Vector2(5, Y_BUFFER + (selected * SPACING)));
            for (int i = 0; i < options.Count; i++)
            {
                options[i].draw(new SBWrapper(wrapper, new Vector2(LEFT_BUFFER, y)));
                y += SPACING;
            }
        }

    }
}
