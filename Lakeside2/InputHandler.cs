using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Lakeside2
{


    public class InputHandler
    {
        public static float DEADZONE = 0.1f;
        public static Buttons[] TRACKED_BUTTONS = new Buttons[]
        {
            Buttons.A, Buttons.B, Buttons.Y, Buttons.X,
            Buttons.DPadUp, Buttons.DPadRight, Buttons.DPadDown, Buttons.DPadLeft,
            Buttons.LeftShoulder, Buttons.RightShoulder,
            Buttons.LeftTrigger, Buttons.RightTrigger,
            Buttons.Start, Buttons.Back
        };

        internal bool isKeyPressed(object escape)
        {
            throw new NotImplementedException();
        }

        Dictionary<Keys, bool> keys;

        int mleft;
        int mright;
        public Vector2 mousePosition;


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

        public List<Buttons> heldButtons(int n)
        {
            return gamepads[n].Keys.ToList<Buttons>();
        }

        public List<Keys> heldKeys
        {
            get { return keys.Keys.ToList<Keys>(); }
        }

        public void update()
        {
            updateKeyboard();
            updateMouse();
            updateGamepad(0, GamePad.GetState(PlayerIndex.One));
            updateGamepad(1, GamePad.GetState(PlayerIndex.Two));
            updateGamepad(2, GamePad.GetState(PlayerIndex.Three));
            updateGamepad(3, GamePad.GetState(PlayerIndex.Four));
        }

        public void updateKeyboard()
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] pressed = state.GetPressedKeys();
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

        public void updateMouse()
        {
            MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState(); // static reference to avoid confusion
            mousePosition = new Vector2(mouse.X, mouse.Y);
            if (mouse.LeftButton == ButtonState.Pressed) mleft++;
            else mleft = 0;
            if (mouse.RightButton == ButtonState.Pressed) mright++;
            else mright = 0;
        }

        public bool isGamePadConnected(int n)
        {
            return GamePad.GetState(n).IsConnected;
        }

        public void updateGamepad(int n, GamePadState state)
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
                    if (handledButtons.Contains(tb) && gamepad[tb])
                    {
                        gamepad[tb] = false;
                    }
                    else if (!handledButtons.Contains(tb))
                    {
                        gamepad[tb] = true;
                    }
                }
            }
        }

        public bool isKeyPressed(Keys k)
        {
            return (keys.Keys.Contains(k) && keys[k]);
        }
        public bool isKeyHeld(Keys k)
        {
            return keys.Keys.Contains(k);

        }

        public bool isAnyKeyPressed(params Keys[] keys)
        {
            return Array.Exists(keys, key => isKeyPressed(key));
        }

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
        public Vector2 stickPosition(int n, char s)
        {
            n *= 2;
            if (s == 'r') n++;
            return stickPositions[n];
        }

    }
}
