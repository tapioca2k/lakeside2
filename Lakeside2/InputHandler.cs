using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using static Lakeside2.InputBindings;

namespace Lakeside2
{
    /// <summary>
    /// InputHandler manages the state of the keyboard, mouse, and gamepad.
    /// Only one instance of InputHandler is needed, and it should only be 
    /// updated once per frame.
    /// 
    /// InputHandler uses the terminology "Pressed" to refer to a key or
    /// button being pressed down for the first frame (i.e. rising edge),
    /// and "Held" for a key or button staying pressed down after that.
    /// 
    /// To support rebindable controls, <see cref="isCommandHeld(Bindings)"/> 
    /// and <see cref="isCommandPressed(Bindings)"/> should be used over the 
    /// raw key/gamepad check functions.
    /// </summary>
    public class InputHandler
    {
        /// <summary>
        /// Gamepad analog stick deadzone. Any reading below this value will 
        /// be truncated to zero.
        /// </summary>
        public static float DEADZONE = 0.1f;

        /// <summary>
        /// Gamepad buttons tracked by InputHandler
        /// </summary>
        public static Buttons[] TRACKED_BUTTONS = new Buttons[]
        {
            Buttons.A, Buttons.B, Buttons.Y, Buttons.X,
            Buttons.DPadUp, Buttons.DPadRight, Buttons.DPadDown, Buttons.DPadLeft,
            Buttons.LeftShoulder, Buttons.RightShoulder,
            Buttons.LeftTrigger, Buttons.RightTrigger,
            Buttons.LeftStick, Buttons.RightStick,
            Buttons.Start, Buttons.Back
        };

        public enum ActiveInput
        {
            Keyboard, Gamepad
        }

        Dictionary<Keys, bool> keys;

        int mleft;
        int mright;

        /// <summary>
        /// Mouse position vector2
        /// </summary>
        public Vector2 mousePosition;

        /// <summary>
        /// Which input type is active right now, i.e. KB/M or Gamepad.
        /// If both are being used on the same frame, gamepad will take priority.
        /// </summary>
        public ActiveInput activeInput;


        Dictionary<Buttons, bool>[] gamepads;
        Vector2[] stickPositions;

        public InputHandler()
        {
            keys = new Dictionary<Keys, bool>();
            mleft = 0;
            mright = 0;
            mousePosition = Vector2.Zero;
            gamepads = new Dictionary<Buttons, bool>[4];
            for (int x = 0; x < 4; x++) gamepads[x] = new Dictionary<Buttons, bool>();
            stickPositions = new Vector2[8]
            {
                Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero,
                Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero
            };
        }

        /// <summary>
        /// Buttons currently being held
        /// </summary>
        /// <param name="n">Gamepad number 1-4</param>
        /// <returns></returns>
        public List<Buttons> getHeldButtons(int n)
        {
            return gamepads[n].Keys.ToList<Buttons>();
        }

        /// <summary>
        /// Keys currently being held
        /// </summary>
        /// <returns></returns>
        public List<Keys> getHeldKeys()
        {
            return keys.Keys.ToList<Keys>();
        }

        /// <summary>
        /// Keys currently being pressed
        /// </summary>
        /// <returns></returns>
        public Keys[] getPressedKeys()
        {
            IEnumerable<Keys> pk = from k
                                   in keys.Keys
                                   where keys[k]
                                   select k;
            return new List<Keys>(pk).ToArray();
        }

        /// <summary>
        /// Buttons currently being pressed
        /// </summary>
        /// <param name="n">Gamepad number 1-4</param>
        /// <returns></returns>
        public Buttons[] getPressedButtons(int n)
        {
            IEnumerable<Buttons> buttons = from b
                                           in gamepads[n].Keys
                                           where gamepads[n][b]
                                           select b;
            return new List<Buttons>(buttons).ToArray();
        }

        /// <summary>
        /// Update the state of the keyboard, mouse, and four gamepads.
        /// This should only be called once per frame.
        /// </summary>
        public void update()
        {
            updateKeyboard();
            updateMouse();
            updateGamepad(0, GamePad.GetState(PlayerIndex.One));
            updateGamepad(1, GamePad.GetState(PlayerIndex.Two));
            updateGamepad(2, GamePad.GetState(PlayerIndex.Three));
            updateGamepad(3, GamePad.GetState(PlayerIndex.Four));
        }

        void updateKeyboard()
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] pressed = state.GetPressedKeys();
            if (pressed.Length > 0) activeInput = ActiveInput.Keyboard;
            Keys[] handledKeys = keys.Keys.ToArray<Keys>();
            foreach (Keys k in handledKeys)
            {
                if (pressed.Contains(k) && keys[k])
                {
                    keys[k] = false;
                }
                else if (!pressed.Contains(k))
                {
                    keys.Remove(k);
                }
            }
            foreach (Keys k in pressed)
            {
                if (!keys.Keys.Contains(k))
                {
                    keys[k] = true;
                }
            }
        }

        void updateMouse()
        {
            MouseState mouse = Mouse.GetState();
            mousePosition = new Vector2(mouse.X, mouse.Y);
            if (mouse.LeftButton == ButtonState.Pressed) mleft++;
            else mleft = 0;
            if (mouse.RightButton == ButtonState.Pressed) mright++;
            else mright = 0;
        }

        void updateGamepad(int n, GamePadState state)
        {
            Dictionary<Buttons, bool> gamepad = gamepads[n];
            List<Buttons> handledButtons = new List<Buttons>(gamepad.Keys);

            if (!state.IsConnected) // no buttons pressed because controller isn't plugged in
            {
                foreach (Buttons b in handledButtons)
                {
                    gamepad.Remove(b);
                }
                return;
            }

            // Analog sticks
            stickPositions[n] = state.ThumbSticks.Left;
            stickPositions[n + 1] = state.ThumbSticks.Right;

            foreach (Buttons tb in TRACKED_BUTTONS)
            {
                if (state.IsButtonDown(tb))
                {
                    activeInput = ActiveInput.Gamepad;
                    if (handledButtons.Contains(tb) && gamepad[tb])
                    {
                        gamepad[tb] = false;
                    }
                    else if (!handledButtons.Contains(tb))
                    {
                        gamepad[tb] = true;
                    }
                }
                else
                {
                    gamepad.Remove(tb);
                }
            }
        }

        /// <summary>
        /// Is gamepad connected
        /// </summary>
        /// <param name="n">Gamepad number 1-4</param>
        /// <returns></returns>
        public bool isGamePadConnected(int n)
        {
            return GamePad.GetState(n).IsConnected;
        }

        public bool isKeyPressed(Keys k)
        {
            return (keys.Keys.Contains(k) && keys[k]);
        }
        public bool isKeyHeld(Keys k)
        {
            return keys.Keys.Contains(k);

        }

        /// <summary>
        /// Return true if any of the parameter keys are pressed
        /// </summary>
        /// <param name="keys">Keys to check</param>
        /// <returns></returns>
        public bool isAnyKeyPressed(params Keys[] keys)
        {
            return Array.Exists(keys, key => isKeyPressed(key));
        }

        /// <summary>
        /// Return true if any of the parameter keys are held
        /// </summary>
        /// <param name="keys">Keys to check</param>
        /// <returns></returns>
        public bool isAnyKeyHeld(params Keys[] keys)
        {
            return Array.Exists(keys, key => isKeyHeld(key));
        }

        public bool isMousePressed(char b)
        {
            if (b == 'l') return (mleft == 1);
            else if (b == 'r') return (mright == 1);
            else if (b == 'e') return (mright == 1 || mleft == 1);
            else return false;
        }
        public bool isMouseHeld(char b)
        {
            if (b == 'l') return (mleft > 0);
            else if (b == 'r') return (mright > 0);
            else if (b == 'e') return (mright > 0 || mleft > 0);
            else return false;
        }

        public bool isButtonPressed(int n, Buttons b)
        {
            Dictionary<Buttons, bool> gamepad = gamepads[n];
            return (gamepad.ContainsKey(b) && gamepad[b]);
        }

        public bool isButtonHeld(int n, Buttons b)
        {
            Dictionary<Buttons, bool> gamepad = gamepads[n];
            return (gamepad.ContainsKey(b));
        }

        /// <summary>
        /// Return position of an analog stick
        /// </summary>
        /// <param name="n">Gamepad number 1-4</param>
        /// <param name="s">Which stick</param>
        /// <returns></returns>
        public Vector2 stickPosition(int n, char s)
        {
            n *= 2;
            if (s == 'r') n++;
            return stickPositions[n];
        }

        /// <summary>
        /// Return true if an <see cref="InputBindings"/> command is being pressed
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool isCommandPressed(Bindings command)
        {
            List<object> inputs = InputBindings.getInputs(command);
            foreach (object o in inputs)
            {
                if (o is Keys && isKeyPressed((Keys)o)) return true;
                else if (o is Buttons && isButtonPressed(0, (Buttons)o)) return true;
            }
            return false;
        }

        /// <summary>
        /// Return true if an <see cref="InputBindings"/> command is being held
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool isCommandHeld(Bindings command)
        {
            List<object> inputs = InputBindings.getInputs(command);
            foreach (object o in inputs)
            {
                if (o is Keys && isKeyHeld((Keys)o)) return true;
                else if (o is Buttons && isButtonHeld(0, (Buttons)o)) return true;
            }
            return false;
        }

    }
}
