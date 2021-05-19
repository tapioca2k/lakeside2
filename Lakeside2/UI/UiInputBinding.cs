using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI
{

    class UiInputBinding : UiList
    {

        List<Bindings> bindingOrder;
        Bindings rebind;
        double rebindTime;

        public UiInputBinding(ContentManager Content) : base(Content, new string[0])
        {
            rebindTime = -1;
            bindingOrder = new List<Bindings>();
            foreach (Bindings command in Enum.GetValues(typeof(Bindings)))
            {
                bindingOrder.Add(command);
            }

            this.setStrings(buildStrings(-1));
            this.addCallback(element =>
            {
                if (selected == bindingOrder.Count) // Defaults
                {
                    InputBindings.loadDefaults();
                    this.setStrings(buildStrings(-1));
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
                else // editing binding at bindingOrder[selected]
                {
                    rebind = bindingOrder[selected];
                    this.setStrings(buildStrings(selected));
                    rebindTime = 0;
                    this.finished = false;
                }
            });
        }

        string[] buildStrings(int rebindIndex)
        {
            List<string> bindings = new List<string>();
            for (int i = 0; i < bindingOrder.Count; i++)
            {
                Bindings command = bindingOrder[i];
                if (i == rebindIndex)
                {
                    bindings.Add(command + ": " + 
                        "Waiting for input...");
                }
                else
                {
                    bindings.Add(command + ": " +
                        String.Join(',', InputBindings.getInputs(command)));
                }
            }
            bindings.Add("Reset to defaults");
            bindings.Add("Save changes");
            bindings.Add("Discard changes");
            return bindings.ToArray();
        }

        public override void update(double dt)
        {
            base.update(dt);
            if (rebindTime > 3) // keybinding timeout
            {
                rebindTime = -1;
                setStrings(buildStrings(-1));
            }
            else if (rebindTime >= 0) // waiting for rebind...
            {
                rebindTime += dt;
            }
        }

        public override void onInput(InputHandler input)
        {
            if (rebindTime < 0)
            {
                base.onInput(input);
            }
            else
            {
                switch (input.activeInput)
                {
                    case InputHandler.ActiveInput.Keyboard:
                        {
                            Keys[] pressedKeys = input.getPressedKeys();
                            if (pressedKeys.Length > 0)
                            {
                                InputBindings.setKeyboardBinds(rebind, pressedKeys[0]);
                                rebindTime = -1;
                                setStrings(buildStrings(-1));
                            }
                            break;
                        }
                    case InputHandler.ActiveInput.Gamepad:
                        {
                            Buttons[] pressedButtons = input.getPressedButtons(0);
                            if (pressedButtons.Length > 0)
                            {
                                InputBindings.setGamepadBinds(rebind, pressedButtons[0]);
                                rebindTime = -1;
                                setStrings(buildStrings(-1));
                            }
                            break;
                        }
                }
            }
        }
    }
}
