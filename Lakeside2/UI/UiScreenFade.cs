using Microsoft.Xna.Framework;
using NLua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiScreenFade : UiElement
    {
        public override Vector2 size => new Vector2(Game1.INTERNAL_WIDTH, Game1.INTERNAL_HEIGHT - (fullscreen ? 0 : UiStripe.STRIPE_HEIGHT));

        double t = 0;
        float alpha = 0;
        float[] alphas = new float[3] { 0.33f, 0.8f, 1 };
        Action midpoint;
        bool fadingOut = true;
        public bool fullscreen = true;

        public UiScreenFade(LuaFunction midpoint)
        {
            this.midpoint = () => { midpoint.Call(new object[0]); };
        }

        public UiScreenFade(Action midpoint)
        {
            this.midpoint = midpoint;
        }

        public override void setUiSystem(UiSystem system)
        {
            base.setUiSystem(system);
            // fade should not cover UiStripe
            fullscreen = !system.hasStripe;
        }

        public override void update(double dt)
        {
            t += dt;
            if (t >= 1.55) finished = true;
            else if (t >= 1.5) alpha = alphas[0];
            else if (t >= 1.45) alpha = alphas[1];
            else if (t >= 0.66)
            {
                alpha = alphas[2];
                if (fadingOut)
                {
                    midpoint.Invoke();
                    fadingOut = false; // only call midpoint once
                }
            }
            else if (t >= 0.33) alpha = alphas[1];
            else alpha = alphas[0];
        }

        public override void draw(SBWrapper wrapper)
        {
            Color color = new Color(0, 0, 0, alpha);
            wrapper.drawRectangle(size, color);
        }
    }
}
