using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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

        public NPC npc;

        public UiNpcEditor(NPC npc)
        {
            this.npc = npc;
            setBackground(Color.White);
        }

        public override void onInput(InputHandler input)
        {
            if (input.isAnyKeyPressed(Keys.N, Keys.Enter, Keys.Escape))
            {
                finished = true;
            }
            else if (input.isKeyPressed(Keys.T))
            {
                // TODO open NPC texture picker
            }
            else if (input.isKeyPressed(Keys.S) && this.npc != null)
            {
                system.pushElement(new UiTextInput("Script: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "") this.npc.setScript(input.text);
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.D) && this.npc != null)
            {
                system.pushElement(new UiTextInput("Sure? (Y/N): ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text == "Y")
                    {
                        // TODO delete npc?
                        finished = true;
                    }
                }), Vector2.Zero);
            }
        }


        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            if (npc != null) npc.drawRaw(wrapper, new Vector2(5, 5));
            wrapper.drawString("(T)exture: " + ((npc != null) ? npc.filename : "N/A"), new Vector2(25, 5));
            wrapper.drawString("(S)cript: " + ((npc != null) ? UiTextDisplay.TextOrNull(npc.script) : "N/A"), new Vector2(5, 25));
            wrapper.drawString("(D)elete", new Vector2(5, 45));
        }
    }
}
