using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    // mix buttons and keys together into a rebindable input system
    class InputBindings
    {
        Dictionary<string, List<object>> boundInputs;

        public InputBindings()
        {
            boundInputs = new Dictionary<string, List<object>>();

            // TODO - load these from JSON, have a user interface for viewing and setting them
            setKeybinds("move_up", Keys.W, Keys.Up, Buttons.DPadUp);
            setKeybinds("move_down", Keys.S, Keys.Down, Buttons.DPadDown);
            setKeybinds("move_left", Keys.A, Keys.Left, Buttons.DPadLeft);
            setKeybinds("move_right", Keys.D, Keys.Right, Buttons.DPadRight);

            setKeybinds("interact", Keys.E, Buttons.A);
            setKeybinds("escape", Keys.Escape, Buttons.B);
        }

        void setKeybinds(string command, params object[] inputs)
        {
            List<object> binds = new List<object>();
            foreach (object o in inputs)
            {
                if (o is Keys || o is Buttons)
                {
                    binds.Add(o);
                }
            }
            if (boundInputs.ContainsKey(command)) boundInputs[command] = binds;
            else boundInputs.Add(command, binds);
        }

        public List<object> getInputs(string command)
        {
            if (!boundInputs.ContainsKey(command)) return new List<object>();
            else return boundInputs[command];
        }

    }
}
