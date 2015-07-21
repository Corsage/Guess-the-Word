using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Guess_The_Word.State;
using Guess_The_Word.Core;

namespace Guess_The_Word
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public static MainGame Instance { get; private set; }

        public StateBase m_state;

        MouseState m_pmouse;

        public Song m_bgm;
        public Texture2D m_sound;
        Rectangle r_sound = new Rectangle(15, 535, 64, 64);

        GraphicsDeviceManager m_graphics;
        SpriteBatch m_spriteBatch;

        public MainGame()
        {
            Instance = this;
            
            m_graphics = new GraphicsDeviceManager(this);
            m_graphics.IsFullScreen = false;
            m_graphics.PreferredBackBufferWidth = Constants.Width;
            m_graphics.PreferredBackBufferHeight = Constants.Height;
            m_graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "Guess the Word";
            this.IsMouseVisible = true;

            m_state = new SplashState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            // m_bgm = Content.Load<Song>(@"Music/background_music");
            m_sound = Content.Load<Texture2D>(@"Textures/Volume_On");
            m_bgm = Content.Load<Song>(@"Music/background_music");

            MediaPlayer.Play(MainGame.Instance.m_bgm);
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            m_state.Update(gameTime);

            MouseState m_mouse = Mouse.GetState();
            if ((m_mouse.LeftButton == ButtonState.Pressed) && (m_pmouse.LeftButton == ButtonState.Released) && (r_sound.Contains(m_mouse.X, m_mouse.Y)))
            {
                Constants.m_music ^= true;
                Constants.Music();
            }

            m_pmouse = m_mouse;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_spriteBatch.Begin();

            m_state.Draw(m_spriteBatch);

            m_spriteBatch.Draw(m_sound, r_sound, Color.White);

            m_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
