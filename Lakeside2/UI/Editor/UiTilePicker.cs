using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Lakeside2.UI.Editor
{
    class UiTilePicker : UiTexturePicker
    {
        public UiTilePicker(ContentManager Content) : base(Content)
        {
        }

        public override List<IDrawable> populateList(ContentManager Content)
        {
            List<IDrawable> tiles = new List<IDrawable>();
            foreach (string file in Directory.EnumerateFiles("Content/tiles"))
            {
                string cleaned = Path.GetFileNameWithoutExtension(file);
                tiles.Add(new Tile(Content, cleaned));
            }
            return tiles;
        }
        
    }
}
