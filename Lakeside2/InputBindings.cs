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

    /// <summary>
    /// InputBindings mixes keys and gamepad buttons together and
    /// abstracts them behind <see cref="Bindings"/>.
    /// </summary>
    public static class InputBindings
    {
        const string INPUTS_JSON = "Content/input.json";

        /// <summary>
        /// Input commands to bind to
        /// </summary>
        public enum Bindings
        {
            Up, Down, Left, Right,
            Interact, Start, Back
        }

        static List<SerializableBinding> boundInputs;

        static InputBindings()
        {
            loadKeybinds();
        }

        static SerializableBinding getSerializableBinding(Bindings command)
        {
            SerializableBinding b = (from binding
                         in boundInputs
                                     where binding.binding == command
                                     select binding).First();
            return b;
        }

        /// <summary>
        /// Load hard-coded default input bindings
        /// </summary>
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

        /// <summary>
        /// Bind keys and buttons to an input command. Any old bindings will
        /// be removed.
        /// </summary>
        /// <param name="command"><see cref="Bindings"/> to bind to</param>
        /// <param name="inputs">Keys and Gamepad buttons to bind</param>
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

        /// <summary>
        /// Bind keys to an input command. Any existing gamepad bindings
        /// will be preserved.
        /// </summary>
        /// <param name="command"><see cref="Bindings"/> to bind to</param>
        /// <param name="keys">Keys to bind</param>
        public static void setKeyboardBinds(Bindings command, params Keys[] keys)
        {
            SerializableBinding b = getSerializableBinding(command);
            b.keys = new List<Keys>(keys);
        }

        /// <summary>
        /// Bind gamepad buttons to an input command. Any existing key bindings
        /// will be preserved.
        /// </summary>
        /// <param name="command"><see cref="Bindings"/> to bind to</param>
        /// <param name="keys">Buttons to bind</param>
        public static void setGamepadBinds(Bindings command, params Buttons[] buttons)
        {
            SerializableBinding b = getSerializableBinding(command);
            b.buttons = new List<Buttons>(buttons);
        }

        /// <summary>
        /// Get all inputs bound to a command
        /// </summary>
        /// <param name="command"><see cref="Bindings"/></param>
        /// <returns></returns>
        public static List<object> getInputs(Bindings command)
        {
            SerializableBinding b = getSerializableBinding(command);
            List<object> allBindings = new List<object>();
            b.keys.ForEach(k => allBindings.Add(k));
            b.buttons.ForEach(g => allBindings.Add(g));

            return allBindings;
        }

        /// <summary>
        /// Save current input bindings to a file
        /// </summary>
        /// <param name="path">Filename</param>
        public static void saveKeybinds(string path = INPUTS_JSON)
        {
            string json = JsonSerializer.Serialize(boundInputs);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Load input bindings from a file. The current bindings will be
        /// overwritten.
        /// </summary>
        /// <param name="path">Filename</param>
        public static void loadKeybinds(string path = INPUTS_JSON)
        {
            string json = File.ReadAllText(path);
            boundInputs = JsonSerializer.Deserialize<List<SerializableBinding>>(json);
        }

    }
}
