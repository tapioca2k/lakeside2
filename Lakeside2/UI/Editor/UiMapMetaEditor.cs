using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

        ContentManager Content;
        TileMap map;

        public UiMapMetaEditor(ContentManager Content, TileMap map)
        {
            this.Content = Content;
            this.map = map;
            setBackground(Game1.BG_COLOR, false);
        }

        public override void onInput(InputHandler input)
        {
            if (input.isAnyKeyPressed(Keys.M, Keys.Enter, Keys.Escape))
            {
                finished = true;
            }
            else if (input.isKeyPressed(Keys.N)) // rename map
            {
                system.pushElement(new UiTextInput("Name: ").addCallback((element) =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "") map.name = input.text;
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.R)) // resize map
            {
                system.pushElement(new UiTextInput("Width: ").addCallback((element) => 
                {
                    UiTextInput widthInput = (UiTextInput)element;
                    int newWidth = map.width;
                    if (widthInput.text != "") newWidth = int.Parse(widthInput.text);
                    system.pushElement(new UiTextInput("Height: ").addCallback((element2) =>
                    {
                        UiTextInput heightInput = (UiTextInput)element2;
                        int newHeight = map.height;
                        if (heightInput.text != "") newHeight = int.Parse(heightInput.text);
                        map.resize(Content, newWidth, newHeight);

                    }), new Vector2(0, 20));
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.B))
            {
                // push elements in reverse order
                system.pushElement(new UiTextInput("B: ").addCallback((element) =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "") map.color.B = Convert.ToByte(input.text);
                }), new Vector2(0, 40));
                system.pushElement(new UiTextInput("G: ").addCallback((element) =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "") map.color.G = Convert.ToByte(input.text);
                }), new Vector2(0, 20));
                system.pushElement(new UiTextInput("R: ").addCallback((element) =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "") map.color.R = Convert.ToByte(input.text);
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.S))
            {
                system.pushElement(new UiTextInput("X: ").addCallback((element) =>
                {
                    UiTextInput xi = (UiTextInput)element;
                    int x = (int)map.playerStart.X;
                    if (xi.text != "") x = int.Parse(xi.text);
                    system.pushElement(new UiTextInput("Y: ").addCallback((element2) =>
                    {
                        UiTextInput yi = (UiTextInput)element2;
                        int y = (int)map.playerStart.Y;
                        if (yi.text != "") y = int.Parse(yi.text);
                        map.playerStart = new Vector2(x, y);

                    }), new Vector2(0, 20));
                }), Vector2.Zero);
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            wrapper.drawString(map.filename, new Vector2(5, 5));
            wrapper.drawString("(N)ame: " + map.name, new Vector2(5, 25));
            wrapper.drawString("Size: " + new Vector2(map.width, map.height), new Vector2(5, 45));
            wrapper.drawString("(R)esize (" + map.width + "," + map.height + ")", new Vector2(5, 65));
            wrapper.drawString("(B)ackground color: ", new Vector2(5, 85));
            wrapper.drawString("" + Vector3.Multiply(map.color.ToVector3(), 255), new Vector2(25, 105), map.color);
            wrapper.drawString("(S)tart: " + map.playerStart, new Vector2(5, 125));
        }


    }
}
