using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    public enum Bindings
    {
        Up, Down, Left, Right,
        Interact, Start, Back
    }


    // mix buttons and keys together into a rebindable input system
    class InputBindings
    {
        Dictionary<Bindings, List<object>> boundInputs;

        public InputBindings()
        {
            boundInputs = new Dictionary<Bindings, List<object>>();

            // TODO - load these from JSON, have a user interface for viewing and setting them
            setKeybinds(Bindings.Up, Keys.W, Keys.Up, Buttons.DPadUp);
            setKeybinds(Bindings.Down, Keys.S, Keys.Down, Buttons.DPadDown);
            setKeybinds(Bindings.Left, Keys.A, Keys.Left, Buttons.DPadLeft);
            setKeybinds(Bindings.Right, Keys.D, Keys.Right, Buttons.DPadRight);

            setKeybinds(Bindings.Interact, Keys.E, Buttons.A);
            setKeybinds(Bindings.Start, Keys.Escape, Buttons.Start);
            setKeybinds(Bindings.Back, Keys.Back, Buttons.B);
        }

        void setKeybinds(Bindings command, params object[] inputs)
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

        public List<object> getInputs(Bindings command)
        {
            if (!boundInputs.ContainsKey(command)) return new List<object>();
            else return boundInputs[command];
        }

    }
}
