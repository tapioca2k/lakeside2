using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Lakeside2.InputBindings;

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
        public string selectedString
        {
            get
            {
                if (selected == -1) return null;
                else return options[selected].text;
            }
        }

        public UiList(ContentManager Content, string[] options)
        {
            setBackground(Color.White, true);
            selected = 0;
            pointer = Content.Load<Texture2D>("pointer");
            this.setStrings(options);
        }

        public void setStrings(string[] options)
        {
            this.options = new List<UiTextDisplay>();
            for (int i = 0; i < options.Length; i++)
            {
                this.options.Add(new UiTextDisplay(options[i]));
            }
        }

        public override void onInput(InputHandler input)
        {
            if (input.isCommandPressed(Bindings.Up))
            {
                MusicManager.playSfx("cursor");
                if (--selected < 0) selected = this.options.Count - 1;
            }
            else if (input.isCommandPressed(Bindings.Down))
            {
                MusicManager.playSfx("cursor");
                if (++selected == this.options.Count) selected = 0;
            }
            else if (input.isCommandPressed(Bindings.Interact))
            {
                MusicManager.playSfx("select");
                finished = true;
            }
            else if (input.isCommandPressed(Bindings.Back))
            {
                MusicManager.playSfx("back");
                selected = -1;
                finished = true;
            }
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
