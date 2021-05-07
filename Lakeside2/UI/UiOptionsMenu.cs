using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiOptionsMenu : UiElement
    {
        public const int WIDTH = 155;
        public const int HEIGHT = 80;
        public override Vector2 size => new Vector2(WIDTH, HEIGHT);

        Game1 game;
        Texture2D pointer;
        UiElement[] sections;
        UiLeftRightPicker resolution;
        int index;
        bool discard;

        public UiOptionsMenu(Game1 game, ContentManager Content)
        {
            this.game = game;
            setBackground(Color.White, true);
            pointer = Content.Load<Texture2D>("pointer");
            sections = new UiElement[3]
            {
                new UiTextDisplay("Resolution"),
                new UiTextDisplay("Save changes"),
                new UiTextDisplay("Discard changes")
            };
            discard = false;
            index = 0;

            resolution = new UiLeftRightPicker(Content,
                new string[4] { "x1 (320x180)", "x2 (640x320)", "x4 (1280x720)", "x6 (1920x1080)" });
            resolution.selected = GameInfo.resolution;
            resolution.addCallback(element =>
            {
                resolution.enabled = false;
            });

            this.addCallback(element =>
            {
                if (!discard)
                {
                    GameInfo.resolution = resolution.selected;
                    GameInfo.save();
                    game.setResolution();
                }
            });
        }

        public override void onInput(InputHandler input)
        {
            if (resolution.enabled) // interact with enabled
            {
                resolution.onInput(input);
            }
            else // interact with sections
            {
                if (input.isCommandPressed("move_up")) index--;
                else if (input.isCommandPressed("move_down")) index++;
                else if (input.isCommandPressed("interact"))
                {
                    switch (index)
                    {
                        case 0: resolution.enabled = true; break;
                        case 1: finished = true; break;
                        case 2: finished = true; discard = true; break;
                    }
                }

                if (index < 0) index = sections.Length - 1;
                else if (index >= sections.Length) index = 0;
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            sections[0].draw(new SBWrapper(wrapper, new Vector2(20, 1)));
            resolution.draw(new SBWrapper(wrapper, new Vector2(15, 20)));
            sections[1].draw(new SBWrapper(wrapper, new Vector2(20, 40)));
            sections[2].draw(new SBWrapper(wrapper, new Vector2(20, 60)));
            wrapper.draw(pointer, new Vector2(1, 40 * index - (index == sections.Length - 1 ? 20: 0)));
        }


    }
}
