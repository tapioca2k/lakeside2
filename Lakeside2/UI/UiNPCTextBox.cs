using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiNPCTextBox : UiTextBox
    {
        public UiNPCTextBox(NPC npc, string text) : base(npc.realName + ":\n" + text, true)
        {
            // TODO character portraits would be really cool
        }
    }
}
