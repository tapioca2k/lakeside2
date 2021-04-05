using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lakeside2.UI.Editor
{
    class UiEntityPicker : UiTexturePicker
    {
        public UiEntityPicker(ContentManager Content) : base(Content)
        {
        }

        public override List<IDrawable> populateList(ContentManager Content)
        {
            List<IDrawable> npcs = new List<IDrawable>();
            foreach (string file in Directory.EnumerateFiles("Content/entities"))
            {
                if (Path.GetExtension(file) != ".json") // ignore animation files
                {
                    string cleaned = Path.GetFileNameWithoutExtension(file);
                    npcs.Add(new NPC(Content, cleaned, ""));
                }
            }
            return npcs;
        }
    }
}
