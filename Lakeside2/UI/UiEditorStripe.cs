using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    // UI image with hotkeys for editing mode
    // meant to be displayed over a blank UiStripe, left position
    class UiEditorStripe : UiElement
    {
        Texture2D hotkeys;

        public override Vector2 size
        {
            get
            {
                return new Vector2(Game1.INTERNAL_WIDTH, UiStripe.STRIPE_HEIGHT);
            }
        }

        public UiEditorStripe(ContentManager Content)
        {
            hotkeys = Content.Load<Texture2D>("editorhotkeys");
        }

        public override void draw(SpriteBatch spriteBatch, Vector2 location)
        {
            spriteBatch.Draw(hotkeys, location, Color.White);
        }
    }
}
