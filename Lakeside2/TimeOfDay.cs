using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    public static class TimeOfDay
    {
        const long DAY = 1000 * 60 * 60 * 24;

        static double t; // ms
        static TimeOfDay()
        {
            t = 0;
        }

        public static void addMillis(double m)
        {
            t += m;
            if (t > DAY) t -= DAY;
        }
        public static void addSeconds(double s)
        {
            addMillis(s * 1000);
        }

        public static void addMinutes(double m)
        {
            addSeconds(m * 60);
        }

        public static void addHours(double h)
        {
            addMinutes(h * 60);
        }

        public static TimeSpan getTime()
        {
            return TimeSpan.FromMilliseconds(t);
        }

        public static double getMillis()
        {
            return t;
        }

        public static void restart()
        {
            t = 0;
        }

        public static void setTime(double hours, double minutes, double seconds, double millis)
        {
            restart();
            addHours(hours);
            addMinutes(minutes);
            addSeconds(seconds);
            addMillis(millis);
        }

    }
}
