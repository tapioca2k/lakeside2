using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    class Animation
    {
        public double[][] durations { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        double t = 0;
        int selected = 0;
        int frame = 0;

        public void update(double dt)
        {
            t += dt;
            if (t > durations[selected][frame])
            {
                frame++;
                t = 0;
                if (frame >= durations[selected].Length) frame = 0;
            }
        }

        public void setAnimation(int n, bool reset = false)
        {
            if (n < durations.Length)
            {
                selected = n;
                if (reset)
                {
                    t = 0;
                    frame = 0;
                }
            }
        }

        public Rectangle getFrame()
        {
            int sx = frame * (width + 1);
            int sy = selected * (height + 1);
            return new Rectangle(sx, sy, width, height);
        }

    }
}
