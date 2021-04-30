using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    public static class Flags
    {
        public const int NO_VALUE = -1;

        static Dictionary<string, int> flags;
        static Dictionary<string, string> strings;

        static Flags()
        {
            flags = new Dictionary<string, int>();
            strings = new Dictionary<string, string>();
        }

        public static Dictionary<string, int> getAllFlags()
        {
            return flags;
        }

        public static Dictionary<string, string> getAllStrings()
        {
            return strings;
        }

        public static int getIntFlag(string name)
        {
            if (!flags.ContainsKey(name)) return NO_VALUE;
            else return flags[name];
        }

        public static void setIntFlag(string name, int value)
        {
            if (flags.ContainsKey(name)) flags[name] = value;
            else flags.Add(name, value);
        }

        public static bool getBooleanFlag(string name)
        {
            return getIntFlag(name) > 0;
        }

        public static void setBooleanFlag(string name, bool value)
        {
            setIntFlag(name, value ? 1 : 0);
        }

        public static string getStringFlag(string name)
        {
            if (!strings.ContainsKey(name)) return null;
            else return strings[name];
        }

        public static void setStringFlag(string name, string value)
        {
            if (strings.ContainsKey(name)) strings[name] = value;
            else strings.Add(name, value);
        }


    }
}
