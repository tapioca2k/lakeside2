using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{

    class UiInputBinding : UiList
    {

        List<Bindings> bindingOrder;

        public UiInputBinding(ContentManager Content) : base(Content, new string[0])
        {
            bindingOrder = new List<Bindings>();
            foreach (Bindings command in Enum.GetValues(typeof(Bindings)))
            {
                bindingOrder.Add(command);
            }

            this.setStrings(buildStrings());
            this.addCallback(element =>
            {
                if (selected == bindingOrder.Count) // Defaults
                {
                    InputBindings.loadDefaults();
                    this.setStrings(buildStrings());
                    this.finished = false;
                }
                else if (selected == bindingOrder.Count + 1) // Save+Exit
                {
                    InputBindings.saveKeybinds();
                }
                else if (selected == bindingOrder.Count + 2) // Discard+Exit
                {
                    InputBindings.loadKeybinds();
                }
                else // TODO editing binding at bindingOrder[selected]
                {
                    this.finished = false;
                }
            });
        }

        string[] buildStrings()
        {
            List<string> bindings = new List<string>();
            for (int i = 0; i < bindingOrder.Count; i++)
            {
                Bindings command = bindingOrder[i];
                bindings.Add(command + ": " +
                    String.Join(',', InputBindings.getInputs(command)));
            }
            bindings.Add("Reset to defaults");
            bindings.Add("Save changes");
            bindings.Add("Discard changes");
            return bindings.ToArray();
        }
    }
}
