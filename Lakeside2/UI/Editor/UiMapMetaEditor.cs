using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI.Editor
{
    class UiMapMetaEditor : UiElement
    {

        public override Vector2 size
        {
            get
            {
                return new Vector2(160, 160);
            }
        }

        TileMap map;

        public UiMapMetaEditor(TileMap map)
        {
            this.map = map;
            setBackground(Color.White);
        }

        public override void onInput(InputHandler input)
        {
            if (input.isKeyPressed(Keys.Enter) || input.isKeyPressed(Keys.M))
            {
                finished = true;
            }
            else if (input.isKeyPressed(Keys.R)) // resize map
            {
                system.pushElement(new UiTextInput("Width: ").addCallback((element) => 
                {
                    UiTextInput widthInput = (UiTextInput)element;
                    int newWidth = int.Parse(widthInput.text);
                    system.pushElement(new UiTextInput("Height: ").addCallback((element2) =>
                    {
                        UiTextInput heightInput = (UiTextInput)element2;
                        int newHeight = int.Parse(heightInput.text);
                        map.resize(newWidth, newHeight);

                    }), Vector2.Zero);

                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.B))
            {
                // push elements in reverse order
                system.pushElement(new UiTextInput("B: ").addCallback((element) =>
                {
                    UiTextInput input = (UiTextInput)element;
                    byte b = Convert.ToByte(input.text);
                    map.color.B = b;

                }), Vector2.Zero);
                system.pushElement(new UiTextInput("G: ").addCallback((element) =>
                {
                    UiTextInput input = (UiTextInput)element;
                    byte g = Convert.ToByte(input.text);
                    map.color.G = g;

                }), Vector2.Zero);
                system.pushElement(new UiTextInput("R: ").addCallback((element) =>
                {
                    UiTextInput input = (UiTextInput)element;
                    byte r = Convert.ToByte(input.text);
                    map.color.R = r;

                }), Vector2.Zero);
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            wrapper.drawString(map.filename, new Vector2(5, 5));
            wrapper.drawString("Size: " + new Vector2(map.width, map.height), new Vector2(5, 25));
            wrapper.drawString("(R)esize", new Vector2(5, 45));
            wrapper.drawString("(B)ackground Color", new Vector2(5, 65));
        }


    }
}
