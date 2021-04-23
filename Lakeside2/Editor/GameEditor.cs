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
        ContentManager Content;

        UiSystem ui;

        public GameEditor(ContentManager Content)
        {
            this.Content = Content;
            ui = new UiSystem();
        }

        public void onInput(InputHandler input)
        {
            bool interacting = ui.onInput(input);
            if (interacting) return;
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
