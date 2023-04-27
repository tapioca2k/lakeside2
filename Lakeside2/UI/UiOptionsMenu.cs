using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Lakeside2.InputBindings;

namespace Lakeside2.UI
{
    class UiOptionsMenu : UiElement
    {
        public const int WIDTH = 155;
        public const int HEIGHT = 100;
        public override Vector2 size => new Vector2(WIDTH, HEIGHT);

        Game1 game;
        ContentManager Content;
        Texture2D pointer;
        UiElement[] sections;
        UiLeftRightPicker resolution;
        int index;
        bool discard;

        public UiOptionsMenu(Game1 game, ContentManager Content)
        {
            this.game = game;
            this.Content = Content;
            setBackground(Game1.BG_COLOR, true);
            pointer = Content.Load<Texture2D>("pointer");
            sections = new UiElement[4]
            {
                new UiTextDisplay("Resolution"),
                new UiTextDisplay("Rebind inputs..."),
                new UiTextDisplay("Save changes"),
                new UiTextDisplay("Discard changes")
            };
            discard = false;
            index = 0;

            resolution = new UiLeftRightPicker(Content,
                new string[4] { "x1 (320x180)", "x2 (640x360)", "x4 (1280x720)", "x6 (1920x1080)" });
            resolution.selected = GameInfo.resolution;
            resolution.addCallback(element =>
            {
                resolution.finished = false;
                resolution.enabled = false;
                if (resolution.selected == -1)
                {
                    resolution.selected = GameInfo.resolution;
                }
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
                if (input.isCommandPressed(Bindings.Up))
                {
                    index--;
                    MusicManager.playSfx("cursor");
                }
                else if (input.isCommandPressed(Bindings.Down))
                {
                    index++;
                    MusicManager.playSfx("cursor");
                }
                else if (input.isCommandPressed(Bindings.Interact))
                {
                    MusicManager.playSfx("select");
                    switch (index)
                    {
                        case 0: resolution.enabled = true; break;
                        case 1:
                            {
                                UiInputBinding bindingEditor = new UiInputBinding(Content);
                                system.pushElement(bindingEditor, new Point(
                                    Game1.INTERNAL_WIDTH / 2 - (int)bindingEditor.size.X / 2,
                                    Game1.INTERNAL_HEIGHT / 2 - (int)bindingEditor.size.Y / 2));
                                break;
                            }
                        case 2: finished = true; break;
                        case 3: discard = true; finished = true; break;
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
            sections[3].draw(new SBWrapper(wrapper, new Vector2(20, 80)));
            wrapper.draw(pointer, new Vector2(1, 20 * index + (index > 0 ? 20 : 0)));
        }


    }
}
