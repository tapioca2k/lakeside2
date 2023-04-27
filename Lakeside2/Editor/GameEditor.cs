using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Editor
{
    class GameEditor : IGameState
    {
        Game1 game;
        ContentManager Content;
        TitleScreen titleScreen;

        UiSystem ui;

        public Color background => titleScreen.background;

        public GameEditor(Game1 game, ContentManager Content, TitleScreen titleScreen)
        {
            this.game = game;
            this.Content = Content;
            this.titleScreen = titleScreen;
            ui = new UiSystem(false);
            makeMenu();
        }

        void makeMenu()
        {
            UiList list = new UiList(Content, new string[4] { "Window Name", "Menu BG", "Starting Map", "Save" });
            list.addCallback((e) =>
            {
                UiList l = (UiList)e;
                switch (l.selected)
                {
                    case 0: // Window Name
                        {
                            ui.pushElement(new UiTextInput("BG Filename: ").addCallback(element =>
                            {
                                UiTextInput input = (UiTextInput)element;
                                if (input.text != "")
                                {
                                    GameInfo.titleBackground = input.text;
                                    titleScreen.updateBackground();
                                }
                            }), Point.Zero);
                            break;
                        }
                    case 1: // Menu BG
                        {
                            ui.pushElement(new UiTextInput("Name: ").addCallback(element =>
                            {
                                UiTextInput input = (UiTextInput)element;
                                if (input.text != "")
                                {
                                    GameInfo.title = input.text;
                                }
                            }), Point.Zero);
                            break;
                        }
                    case 2: // Starting Map
                        {
                            ui.pushElement(new UiOptionBox(Content, "Start game where?", "Overworld", "Map").addCallback(element =>
                            {
                                UiOptionBox option = (UiOptionBox)element;
                                ui.pushElement(new UiTextInput("Filename: ").addCallback(element2 =>
                                {
                                    UiTextInput filename = (UiTextInput)element2;
                                    if (filename.text != "")
                                    {
                                        GameInfo.startMap = filename.text;
                                        GameInfo.startOverworld = (option.selected == 0);
                                    }
                                }), new Point(0, 65));
                            }), Point.Zero);
                            break;
                        }
                    case 3: // Save
                        {
                            GameInfo.save();
                            break;
                        }
                    default: break; // Tried to close the menu, just allow it
                }
            });
            ui.pushElement(list, new Point(5, 5));
        }

        public void onInput(InputHandler input)
        {
            bool interacting = ui.onInput(input);
            if (interacting) return;
        }

        public void update(double dt)
        {
            ui.update(dt);
            if (ui.getElementCount() == 0) // menu was closed, remake it
            {
                makeMenu();
            }
        }

        public void draw(SBWrapper wrapper)
        {
            ui.draw(wrapper);
        }
    }
}
