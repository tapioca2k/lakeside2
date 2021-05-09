using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI.Editor
{
    class UiLayerEditor : UiElement
    {
        public override Vector2 size => Vector2.Zero;
        public List<string> layers;

        UiList layerList;
        UiList commandList;
        bool adding, deleting;
        bool selecting => adding || deleting;

        public UiLayerEditor(ContentManager Content, List<string> layers)
        {
            this.layers = layers;
            layerList = new UiList(Content, layers.ToArray());
            commandList = new UiList(Content, new string[3] { "Add", "Delete", "Exit" });
            commandList.addCallback(element =>
            {
                commandList.finished = false;
                switch (commandList.selected)
                {
                    case 0: adding = true; layerList.setStrings(getLayerStrings().ToArray()); break;
                    case 1: deleting = true; layerList.setStrings(getLayerStrings().ToArray()); break;
                    case 2: finished = true; break;
                }
            });
            layerList.addCallback(element =>
            {
                layerList.finished = false;
                if (layerList.selected != -1)
                {
                    if (adding)
                    {
                        system.pushElement(new UiTextInput("Filename: ").addCallback(input =>
                        {
                            UiTextInput textInput = (UiTextInput)input;
                            if (textInput.text != null)
                            {
                                layers.Insert(layerList.selected, textInput.text);
                                layerList.setStrings(layers.ToArray());
                                adding = false;
                            }
                        }), Vector2.Zero);
                    }
                    else if (deleting)
                    {
                        layers.RemoveAt(layerList.selected);
                        layerList.setStrings(layers.ToArray());
                        deleting = false;
                    }
                }
            });
        }

        List<string> getLayerStrings()
        {
            List<string> l = new List<string>(layers);
            if (adding) l.Add(" ");
            return l;
        }

        public override void update(double dt)
        {
            base.update(dt);
            layerList.update(dt);
            commandList.update(dt);
        }

        public override void onInput(InputHandler input)
        {
            base.onInput(input);
            if (selecting)
            {
                layerList.onInput(input);
            }
            else
            {
                commandList.onInput(input);
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            commandList.draw(new SBWrapper(wrapper, Vector2.Zero));
            layerList.draw(new SBWrapper(wrapper, new Vector2(75, 0)));
        }
    }
}
