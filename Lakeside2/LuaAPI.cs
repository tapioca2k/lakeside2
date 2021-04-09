using Lakeside2.UI;
using Lakeside2.UI.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2
{
    class LuaAPI
    {
        World world;
        UiSystem ui;
        Player player;
        ContentManager Content;

        public static Vector2 makeVector2(int x, int y)
        {
            return new Vector2(x, y);
        }

        public static Color makeColor(int r, int g, int b)
        {
            return new Color(r, g, b);
        }

        public LuaAPI(World world, UiSystem ui, Player player, ContentManager Content)
        {
            this.world = world;
            this.ui = ui;
            this.player = player;
            this.Content = Content;
        }

        public void pushUiElement(UiElement element, int x, int y)
        {
            ui.pushElement(element, makeVector2(x, y));
        }

        public void makeDialog(params UiScriptNode[] elements)
        {
            for (int i = 1; i < elements.Length; i++)
            {
                elements[i - 1].next = elements[i];
            }

            pushUiElement(new ScriptChain(elements[0]), 0, 0);
        }

        public UiScriptNode Dialog(string text)
        {
            return new UiScriptNode(new UiTextBox(text));
        }

        public UiScriptNode Branch(string text, string option1, string option2, LuaFunction f1, LuaFunction f2)
        {
            return new UiScriptNode(new UiOptionBox(Content, text, option1, option2).addCallback(element =>
            {
                UiOptionBox option = (UiOptionBox)element;
                if (option.selected == 0) f1.Call(new object[0]);
                else if (option.selected == 1) f2.Call(new object[0]);
            }));
        }

    }
}
