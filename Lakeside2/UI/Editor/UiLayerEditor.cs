using Lakeside2.WorldMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI.Editor
{
    class UiLayerEditor : UiElement
    {
        enum Mode
        {
            Adding, Deleting, SetBase, Inactive
        };

        public override Vector2 size => Vector2.Zero;

        Overworld o;
        public List<string> layers;

        UiList layerList;
        UiList commandList;
        Mode mode = Mode.Inactive;
        bool selecting => mode != Mode.Inactive;

        public UiLayerEditor(ContentManager Content, Overworld o)
        {
            this.o = o;
            this.layers = o.meta.layers;
            layerList = new UiList(Content, layers.ToArray());
            commandList = new UiList(Content, new string[4] { "Add", "Delete", "Set Base", "Exit" });
            commandList.addCallback(element =>
            {
                commandList.finished = false;
                switch (commandList.selected)
                {
                    case 0: mode = Mode.Adding; genLayerStrings(); layerList.selected = 0; break;
                    case 1: mode = Mode.Deleting; genLayerStrings(); layerList.selected = 0; break;
                    case 2: mode = Mode.SetBase; genLayerStrings(); layerList.selected = o.meta.baseLayer; break;
                    case 3: finished = true; break;
                }
            });
            layerList.addCallback(element =>
            {
                layerList.finished = false;
                if (layerList.selected != -1)
                {
                    if (mode == Mode.Adding)
                    {
                        system.pushElement(new UiTextInput("Filename: ").addCallback(input =>
                        {
                            UiTextInput textInput = (UiTextInput)input;
                            if (textInput.text != null)
                            {
                                layers.Insert(layerList.selected, textInput.text);
                                if (layerList.selected >= o.meta.baseLayer) o.meta.baseLayer++;
                                layerList.setStrings(layers.ToArray());
                                o.reloadLayers();
                            }
                        }), Point.Zero);
                    }
                    else if (mode == Mode.Deleting)
                    {
                        layers.RemoveAt(layerList.selected);
                        if (layerList.selected <= o.meta.baseLayer) o.meta.baseLayer--;
                        layerList.setStrings(layers.ToArray());
                        o.reloadLayers();
                    }
                    else if (mode == Mode.SetBase)
                    {
                        o.meta.baseLayer = layerList.selected;
                        o.reloadLayers();
                    }
                    mode = Mode.Inactive;
                }
            });
        }

        void genLayerStrings()
        {
            List<string> l = new List<string>(layers);
            if (mode == Mode.Adding) l.Add(" ");
            layerList.setStrings(l.ToArray());
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
            layerList.draw(new SBWrapper(wrapper, new Vector2(100, 0)));
        }
    }
}
