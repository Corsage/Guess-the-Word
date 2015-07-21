using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Net;
using Guess_The_Word.Core;
using Guess_The_Word.Game;

/// <summary>
/// All outcomes of the SplashState is controlled here.
/// Downloads the required dictionary.
/// </summary>

namespace Guess_The_Word.State
{
    public class SplashState : StateBase
    {
        private Texture2D m_background;
        private SpriteFont m_font;

        private Constants m_constant;
        private BackgroundWorker m_loader = new BackgroundWorker();

        private bool m_files = false;
        private string m_load = "Loading...";

        private string m_hash;

        public SplashState()
        {
            m_background = MainGame.Instance.Content.Load<Texture2D>(@"Textures/Splash_Background");
            m_font = MainGame.Instance.Content.Load<SpriteFont>(@"Fonts/MenuFont");

            m_constant = new Constants();

            LoadDictionary();
        }

        private void DownloadDictionary(object sender, DoWorkEventArgs e)
        {   
            if (File.Exists(m_constant.s_file + @"\Dictionary.txt"))
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(m_constant.s_file + @"/Dictionary.txt"))
                    {
                        m_hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                        stream.Dispose();
                    }
                    md5.Dispose();
                }

                // Both are same file....
                if (m_hash == m_constant.s_hash)
                {
                    //...
                }
                else // Tampering with the dictionary...
                {
                    m_load = "Invalid dictionary... Please wait.";
                    using (WebClient m_wc = new WebClient())
                    {
                        m_wc.DownloadFile(Constants.s_Dictionary, (m_constant.s_file + @"\Dictionary.txt"));
                        m_wc.Dispose();
                    }
                }
            }
            else
            {
                if (!Directory.Exists(m_constant.s_file))
                {
                    Directory.CreateDirectory(m_constant.s_file);
                }

                using (WebClient m_wc = new WebClient())
                {
                    m_wc.DownloadFile(Constants.s_Dictionary, (m_constant.s_file + @"\Dictionary.txt"));
                    m_wc.Dispose();
                }
            }

            var m_linecount = 0;

            using (var m_reader = File.OpenText(m_constant.s_file + @"\Dictionary.txt"))
            {
                while (m_reader.ReadLine() != null)
                {
                    m_linecount++;
                }
                m_reader.Dispose();
            }

            Word.m_lines = m_linecount;
        }

        private void DictionaryComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            m_loader.DoWork -= DownloadDictionary;
            m_loader.RunWorkerCompleted -= DictionaryComplete;
            
            m_load = "Loaded files! Press space to enter...";
            m_files = true;
        }
        
        private void LoadDictionary()
        {
            m_loader.DoWork += new DoWorkEventHandler(DownloadDictionary);
            m_loader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DictionaryComplete);

            m_loader.RunWorkerAsync();
        }

        public override void Update(GameTime gameTime)
        {
            if (m_files)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    MainGame.Instance.m_state = new MenuState();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_background, new Rectangle(0, 0, MainGame.Instance.Window.ClientBounds.Width, MainGame.Instance.Window.ClientBounds.Height), Color.White);
            spriteBatch.DrawString(m_font, m_load, new Vector2((340 - (m_load.Length - 10) * 5), 500), Color.Black);
        }
    }
}
