using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Serialization
{
    // serializable representation of keybinds
    class SerializableBinding
    {
        public Bindings binding { get; set; }
        public List<Keys> keys { get; set; }
        public List<Buttons> buttons { get; set; }

        public SerializableBinding(Bindings binding, List<Keys> keys, List<Buttons> buttons)
        {
            this.binding = binding;
            this.keys = keys;
            this.buttons = buttons;
        }

        public SerializableBinding()
        {

        }
    }
}
