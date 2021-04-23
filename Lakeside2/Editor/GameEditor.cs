using Lakeside2.UI;
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
            ui = new UiSystem();
        }

        public void onInput(InputHandler input)
        {
            bool interacting = ui.onInput(input);
            if (interacting) return;

            // Quit to world map
            if (input.isKeyPressed(Keys.F8))
            {
                game.goToMap(new Player(Content, null, null), null);
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
