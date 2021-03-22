using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    // probbaly just for debug
    class UiPlayerLocationDisplay : UiTextDisplay
    {
        Player player;
        public UiPlayerLocationDisplay(SpriteFont font, Player player) : base(font, "----------")
        {
            this.player = player;
        }

        public override void update(double dt)
        {
            this.text = player.tileLocation.ToString();
        }

    }
}
