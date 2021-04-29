using Lakeside2.Editor;
using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class TitleScreen : IGameState
    {
        Game1 game;
        ContentManager Content;
        Texture2D bg;
        UiSystem ui;
        IGameState editor;
        bool editing => editor != null;

        UiList mainList;

        public TitleScreen(Game1 game, ContentManager Content)
        {
            this.game = game;
            this.Content = Content;
            bg = Content.Load<Texture2D>(GameInfo.titleBackground);
            ui = new UiSystem(false);

            mainList = new UiList(Content, new string[4] { "New Game", "Load Game", "Options", "Quit" });
            mainList.addCallback(element =>
            {
                element.finished = false; // cancel removing the main list
                UiList list = (UiList)element;
                int selected = list.selected;
                switch (selected)
                {
                    case 0: // new game
                        {
                            TimeOfDay.restart();
                            if (GameInfo.startOverworld)
                            {
                                game.goToMap(new Player(Content, null, null), GameInfo.startMap);
                            }
                            else
                            {
                                game.goToWorld(new Player(Content, null, null), GameInfo.startMap);
                            }
                            break;
                        }
                    case 1: // load game
                        {
                            break;
                        }
                    case 2: // options
                        {
                            break;
                        }
                    case 3: // quit
                        {
                            game.Exit();
                            break;
                        }
                }
            });

            Vector2 center = Vector2.Round(new Vector2(
                Game1.INTERNAL_WIDTH / 2 - mainList.size.X / 2, 
                (Game1.INTERNAL_HEIGHT / 2 - mainList.size.Y / 2) + 20));

            ui.pushElement(mainList, center);
        }


        public void onInput(InputHandler input)
        {
            if (editing) editor.onInput(input);
            else ui.onInput(input);

            if (input.isKeyPressed(Keys.F1))
            {
                if (editing)
                {
                    editor = null;
                    bg = Content.Load<Texture2D>(GameInfo.titleBackground);
                }
                else editor = new GameEditor(game, Content);
            }
        }

        public void update(double dt)
        {
            if (editing) editor.update(dt);
            else ui.update(dt);
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(bg);
            ui.draw(wrapper);
            if (editing) editor.draw(wrapper);
        }
    }
}
