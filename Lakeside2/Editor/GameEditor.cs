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

        UiSystem ui;

        public GameEditor(Game1 game, ContentManager Content)
        {
            this.game = game;
            this.Content = Content;
            ui = new UiSystem(true);
            ui.addStripeElement(new UiTexture(Content, "gmhotkeys"), 'l');
        }

        public void onInput(InputHandler input)
        {
            bool interacting = ui.onInput(input);
            if (interacting) return;

            if (input.isKeyPressed(Keys.T)) // title screen image
            {
                ui.pushElement(new UiTextInput("BG Filename: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "")
                    {
                        GameInfo.titleBackground = input.text;
                    }
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.N)) // game name
            {
                ui.pushElement(new UiTextInput("Name: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "")
                    {
                        GameInfo.title = input.text;
                    }
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.S)) // game start
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
                    }), new Vector2(0, 65));
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.F2)) // save
            {
                GameInfo.save();
            }
        }

        public void update(double dt)
        {
            ui.update(dt);
        }

        public void draw(SBWrapper wrapper)
        {
            ui.draw(wrapper);
        }
    }
}
