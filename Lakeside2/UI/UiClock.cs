using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI
{
    class UiClock : UiTextDisplay
    {
        Func<TimeSpan, string> formatter;

        public UiClock(bool twelvehour = true) : base()
        {
            this.formatter = t =>
            {
                int hours = t.Hours;
                int minutes = t.Minutes;
                string suffix = "";
                if (twelvehour)
                {
                    if (hours < 12) suffix = "AM";
                    else suffix = "PM";

                    hours = hours % 12;
                    if (hours == 0) hours = 12;
                }
                return hours.ToString("D2") + ":" + minutes.ToString("D2") + " " + suffix;
            };
        }

        public UiClock(string format) : base()
        {
            this.formatter = t =>
            {
                return t.ToString(format);
            };
        }

        public override void update(double dt)
        {
            this.text = formatter.Invoke(TimeOfDay.getTime());
        }


    }
}
