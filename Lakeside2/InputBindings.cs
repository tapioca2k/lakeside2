using Lakeside2.Serialization;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace Lakeside2
{
    public enum Bindings
    {
        Up, Down, Left, Right,
        Interact, Start, Back
    }

    // mix buttons and keys together into a rebindable input system
    public static class InputBindings
    {
        const string INPUTS_JSON = "Content/input.json";

        static List<SerializableBinding> boundInputs;

        static InputBindings()
        {
            loadKeybinds();
        }

        public static void loadDefaults()
        {
            boundInputs = new List<SerializableBinding>();
            setKeybinds(Bindings.Up, Keys.W, Keys.Up, Buttons.DPadUp);
            setKeybinds(Bindings.Down, Keys.S, Keys.Down, Buttons.DPadDown);
            setKeybinds(Bindings.Left, Keys.A, Keys.Left, Buttons.DPadLeft);
            setKeybinds(Bindings.Right, Keys.D, Keys.Right, Buttons.DPadRight);
            setKeybinds(Bindings.Interact, Keys.E, Buttons.A);
            setKeybinds(Bindings.Start, Keys.Escape, Buttons.Start);
            setKeybinds(Bindings.Back, Keys.Back, Buttons.B);
        }

        public static void setKeybinds(Bindings command, params object[] inputs)
        {
            boundInputs.RemoveAll(b => b.binding == command);

            List<Keys> k = new List<Keys>();
            List<Buttons> g = new List<Buttons>();

            foreach (object o in inputs)
            {
                if (o is Keys) k.Add((Keys)o);
                else if (o is Buttons) g.Add((Buttons)o);
            }

            boundInputs.Add(new SerializableBinding(command, k, g));
        }

        public static List<object> getInputs(Bindings command)
        {
            SerializableBinding b = (from binding
                                     in boundInputs
                                     where binding.binding == command
                                     select binding).First();
            List<object> allBindings = new List<object>();
            b.keys.ForEach(k => allBindings.Add(k));
            b.buttons.ForEach(g => allBindings.Add(g));

            return allBindings;
        }

        public static void saveKeybinds(string path = INPUTS_JSON)
        {
            string json = JsonSerializer.Serialize(boundInputs);
            File.WriteAllText(path, json);
        }

        public static void loadKeybinds(string path = INPUTS_JSON)
        {
            string json = File.ReadAllText(path);
            boundInputs = JsonSerializer.Deserialize<List<SerializableBinding>>(json);
        }

    }
}
