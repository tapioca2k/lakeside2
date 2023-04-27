using Lakeside2.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI.Editor
{
    class UiNpcEditor : UiElement
    {
        public override Vector2 size
        {
            get
            {
                return new Vector2(160, 160);
            }
        }

        ContentManager Content;
        public NPC npc;
        public bool delete;

        public UiNpcEditor(ContentManager Content, NPC npc)
        {
            this.Content = Content;
            this.npc = npc;
            this.delete = false;
            setBackground(Game1.BG_COLOR, false);
        }

        public override void onInput(InputHandler input)
        {
            if (input.isAnyKeyPressed(Keys.Enter, Keys.Escape))
            {
                finished = true;
            }
            else if (input.isKeyPressed(Keys.T))
            {
                system.pushElement(new UiEntityPicker(Content).addCallback(element =>
                {
                    UiEntityPicker picker = (UiEntityPicker)element;
                    NPC n = (NPC)picker.selected;
                    if (this.npc == null) this.npc = new NPC(Content, n.filename, "", false, picker.GetHashCode() + "", picker.GetHashCode() + "");
                    else this.npc.setTexture(Content, n.filename);
                }), new Point(160, 0));
            }
            else if (input.isKeyPressed(Keys.N) && this.npc != null)
            {
                system.pushElement(new UiTextInput("Name: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    string proposed = input.text;
                    if (proposed != "") this.npc.realName = input.text;
                }), Point.Zero);
            }
            else if (input.isKeyPressed(Keys.C) && this.npc != null)
            {
                system.pushElement(new UiTextInput("Codename: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    string proposed = input.text;
                    if (proposed != "") this.npc.setName(input.text);
                }), Point.Zero);
            }
            else if (input.isKeyPressed(Keys.S) && this.npc != null)
            {
                system.pushElement(new UiTextInput("Script: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "") this.npc.setScript(input.text);
                }), Point.Zero);
            }
            else if (input.isKeyPressed(Keys.L) && this.npc != null)
            {
                this.npc.locked = !this.npc.locked;
            }
            else if (input.isKeyPressed(Keys.D) && this.npc != null)
            {
                system.pushElement(new UiTextInput("Sure? (Y/N): ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text.ToLower() == "y")
                    {
                        this.delete = true;
                        finished = true;
                    }
                }), Point.Zero);
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            if (npc != null) npc.draw(wrapper, new Vector2(5, 5));
            wrapper.drawString("(T)exture: " + ((npc != null) ? npc.filename : "N/A"), new Vector2(25, 5));
            wrapper.drawString("(N)ame: " + ((npc != null) ? npc.realName : "N/A"), new Vector2(5, 25));
            wrapper.drawString("(C)odename: " + ((npc != null) ? UiTextDisplay.TextOrNull(npc.name) : "N/A"), new Vector2(5, 45));
            wrapper.drawString("(S)cript: " + ((npc != null) ? UiTextDisplay.TextOrNull(npc.script) : "N/A"), new Vector2(5, 65));
            wrapper.drawString("(L)ocked: " + ((npc != null) ? UiTextDisplay.YesOrNo(npc.locked) : "N/A"), new Vector2(5, 85));
            wrapper.drawString("(D)elete", new Vector2(5, 105));
        }
    }
}
