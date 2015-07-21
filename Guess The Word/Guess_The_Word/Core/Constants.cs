using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Guess_The_Word;

namespace Guess_The_Word.Core
{
    public class Constants
    {
        public const int Width = 800;
        public const int Height = 600;

        public const string s_Copyright = "Created by: Jayss8";
        public static Vector2 v_Copyright = new Vector2(300, 550);

        public const string s_Dictionary = "https://raw.githubusercontent.com/first20hours/google-10000-english/master/google-10000-english.txt"; // Dictionary Words download
        public string s_file = Directory.GetCurrentDirectory() + @"\Dictionary";
        public string s_hash = "2FCD395A423C4F38B41310CBAF4C1130";

        public const string s_help = "A scrambled word will appear,\ntry to unscramble the word!";
        public const string s_about = "Created by: Jayss8 (Jay C.)\n        ICS3UR-A\n          2015";

        public static bool m_music = false;

        public static void Music()
        {
            MediaPlayer.IsMuted = m_music;
            if (m_music)
            {
                MainGame.Instance.m_sound = MainGame.Instance.Content.Load<Texture2D>(@"Textures/Volume_Off");
            }
            else
            {
                MainGame.Instance.m_sound = MainGame.Instance.Content.Load<Texture2D>(@"Textures/Volume_On");
            }
        }
    }
}
