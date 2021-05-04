using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakeside2
{
    // juggles soundeffects like they're songs
    public static class MusicManager
    {
        static ContentManager Content;
        static Dictionary<string, SoundEffect> songs;
        static SoundEffectInstance current;
        public static float volume;

        static MusicManager()
        {
            songs = new Dictionary<string, SoundEffect>();
            volume = 1f;
        }

        public static void init(ContentManager Content)
        {
            MusicManager.Content = Content;
        }

        public static bool loadSong(string name)
        {
            if (Content == null) return false;
            else if (songs.ContainsKey(name)) return true;
            else
            {
                try
                {
                    songs.Add(name, Content.Load<SoundEffect>(name));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static void stopSong()
        {
            if (current != null) current.Stop();
        }

        public static bool playSong(string name, bool loop = true)
        {

            if (!songs.ContainsKey(name))
            {
                if (!loadSong(name)) return false;
            }

            if (current != null) current.Stop();
            current = songs[name].CreateInstance();
            current.Volume = volume;
            current.IsLooped = loop;
            current.Play();

            return true;
        }

        public static void playSfx(string name)
        {
            if (Content == null) return;
            SoundEffect se = Content.Load<SoundEffect>(name);
            se.Play();
        }

        public static double getSfxLength(string name)
        {
            if (Content == null) return -1;
            SoundEffect se = Content.Load<SoundEffect>(name);
            return se.Duration.TotalMilliseconds;
        }
    }
}