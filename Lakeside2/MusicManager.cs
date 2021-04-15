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
    public class MusicManager
    {
        ContentManager Content;
        Dictionary<string, SoundEffect> songs;
        SoundEffectInstance current;
        public float volume;

        public MusicManager(ContentManager c)
        {
            Content = c;
            songs = new Dictionary<string, SoundEffect>();
            volume = 1f;
        }

        public bool loadSong(string name)
        {
            if (songs.ContainsKey(name)) return true;
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

        public void stopSong()
        {
            if (current != null) current.Stop();
        }

        public bool playSong(string name, bool loop = true)
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

        public void playSfx(string name)
        {
            SoundEffect se = Content.Load<SoundEffect>(name);
            se.Play();
        }
    }
}