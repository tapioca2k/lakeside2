using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lakeside2.UI
{
    // TODO add proper info about each file, preview image, etc
    class UiSavePicker : UiList
    {
        public const string NO_FILES = "No saved games.";
        public const string CREATE_NEW_FILE = "Create new file.";

        public UiSavePicker(ContentManager Content, bool hasNewOption) : base(Content, new string[0])
        {
            List<string> saves = new List<string>(Directory.EnumerateFiles("save/"));
            List<string> stripped = new List<string>();
            for (int i = 0; i < saves.Count; i++)
            {
                if (!saves[i].Contains("dummy.txt"))
                {
                    stripped.Add(Path.GetFileNameWithoutExtension(saves[i]));
                }
            }
            if (hasNewOption) stripped.Add(CREATE_NEW_FILE);
            if (stripped.Count == 0) stripped.Add(NO_FILES);

            this.setStrings(stripped.ToArray());
        }
    }
}
