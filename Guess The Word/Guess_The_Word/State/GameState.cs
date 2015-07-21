using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Guess_The_Word.Core;
using Guess_The_Word.Game;

/// <summary>
/// Controls all outcomes during the GameState of the application.
/// Basically handles the actual game.
/// </summary>

namespace Guess_The_Word.State
{
    public class GameState : StateBase
    {
        private Texture2D m_background;
        private Texture2D m_GameScreen;
        private Texture2D m_TimerScreen;

        private SpriteFont m_font;
        private SpriteFont g_font;
        
        // Creates the back and guess "button".
        private Rectangle[] r_buttons = new Rectangle[2];

        private Player m_player;
        private Word m_Words;

        private string m_time;

        private string m_guess;
        private bool m_lose;

        private double m_wait;

        private KeyboardState m_pkbState;
        private MouseState m_pmouse;

        /// <summary>
        /// Constructor controls all loaded content into the application.
        /// </summary>
        public GameState()
        {
            m_background = MainGame.Instance.Content.Load<Texture2D>(@"Textures/Background");
            m_GameScreen = MainGame.Instance.Content.Load<Texture2D>(@"Textures/Game_Screen");
            m_TimerScreen = MainGame.Instance.Content.Load<Texture2D>(@"Textures/Timer_Screen");

            m_font = MainGame.Instance.Content.Load<SpriteFont>(@"Fonts/MenuFont");
            g_font = MainGame.Instance.Content.Load<SpriteFont>(@"Fonts/GameFont");

            m_time = string.Empty;
            m_guess = "Guess:";

            m_lose = false;

            m_player = new Player();
            m_Words = new Word();
        }

        /// <summary>
        /// Resets timer, player's guess string, and generates a new word.
        /// </summary>
        private void Reset()
        {
            m_player.UpdateWord(string.Empty);

            m_Words = new Word();
            m_player.m_timer = 30.0f;

            if (m_guess == "Incorrect Guess")
            {
                m_guess = "Guess:";
            }
        }

        /// <summary>
        /// Updates player's guess string. Hooks onto keyboard to recieve keys.
        /// </summary>
        /// <param name="key"></param>
        private void UpdateGuess(Keys key, GameTime gameTime)
        {
            if (key == Keys.Back)
            {
                // Can't accept a backspace key if there are no characters to remove.
                if (m_player.WordAttempt().Length != 0)
                {
                    m_player.UpdateWord(m_player.WordAttempt().Remove(m_player.WordAttempt().Length - 1, 1));
                }
            }
            else if (key == Keys.Space)
            {
                // Can't accept a space if the string length exceeds the word's length.
                if (m_player.WordAttempt().Length == m_Words.OriginalWord().Length)
                {
                    //...
                }
                else
                {
                    m_player.UpdateWord(m_player.WordAttempt().Insert(m_player.WordAttempt().Length, " "));
                }
            }

            else if (key == Keys.Enter)
            {
                TryWord(gameTime);
            }

            else
            {
                if (m_player.WordAttempt().Length == m_Words.OriginalWord().Length)
                {
                    //...
                }
                else
                {
                    if ((key >= Keys.A) && (key <= Keys.Z))
                    {
                        m_player.UpdateWord(m_player.WordAttempt() + key.ToString());
                    }
                    else
                    {
                        //...
                    }
                }
            }
        }

        /// <summary>
        /// Handler for player submitted word.
        /// </summary>
        /// <param name="gameTime"></param>
        /// Uses gameTime to set the incorrect message timings...
        private void TryWord(GameTime gameTime)
        {
            if (m_player.WordAttempt() == m_Words.OriginalWord())
            {
                // Adds points.
                m_player.AddPoints(m_Words.OriginalWord().Length * 5);

                // Adds total words solved count.
                m_player.SolvedWord();

                Reset();
            }
            else
            {
                // Clears incorrect guess.
                m_player.UpdateWord(string.Empty);
                m_guess = "Incorrect Guess";

                m_wait = gameTime.TotalGameTime.TotalMilliseconds + 1500;
            }
        }

        /// <summary>
        /// THE GAME IS NEVER OVER.
        /// </summary>
        private void GameOver()
        {
            //...
        }

        /// <summary>
        /// Controls the visuals and user-inputs of the game.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            MouseState m_mouse = Mouse.GetState();
            KeyboardState m_kbState = Keyboard.GetState();

            m_player.m_timer -= gameTime.ElapsedGameTime.TotalSeconds;
            m_time = string.Format("{0:N0}", m_player.m_timer);

            if ((m_mouse.LeftButton == ButtonState.Pressed) && (m_pmouse.LeftButton == ButtonState.Released) && (r_buttons[0].Contains(m_mouse.X, m_mouse.Y)))
            {
                //m_GameScreen.Dispose();
                //m_TimerScreen.Dispose();

                MainGame.Instance.m_state = new MenuState();
            }
            else if ((m_mouse.LeftButton == ButtonState.Pressed) && (m_pmouse.LeftButton == ButtonState.Released) && (r_buttons[1].Contains(m_mouse.X, m_mouse.Y)))
            {
                TryWord(gameTime);
            }

            if ((m_lose) && (m_player.m_timer <= 0.0f))
            {
                m_lose = false;
                Reset();
            }
            else if ((!m_lose) && (m_player.m_timer <= 0.0f))
            {
                m_lose = true;
                m_player.m_timer = 5.0f;

            }
            else if ((!m_lose) && (m_player.m_timer <= 10.0f))
            {
                m_time = string.Format("{0:N1}", m_player.m_timer);
            }

            Keys[] m_KeysPressed = m_kbState.GetPressedKeys();

            // Checks if any of the previous update's keys are no longer pressed.
            foreach (Keys key in m_KeysPressed)
            {
                if (m_kbState.IsKeyDown(key) && m_pkbState.IsKeyUp(key))
                {
                    UpdateGuess(key, gameTime);
                }
            }
            
            if ((!m_lose) && (m_wait <= gameTime.TotalGameTime.TotalMilliseconds))
            {
                m_guess = "Guess:";
            }

            m_pkbState = m_kbState;
            m_pmouse = m_mouse;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_background, new Rectangle(0, 0, MainGame.Instance.Window.ClientBounds.Width, MainGame.Instance.Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(m_TimerScreen, new Rectangle(0, 0, MainGame.Instance.Window.ClientBounds.Width, MainGame.Instance.Window.ClientBounds.Height), Color.White);

            var t = new Texture2D(MainGame.Instance.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });

            if (!m_lose)
            {
                spriteBatch.Draw(m_GameScreen, new Rectangle(0, 0, MainGame.Instance.Window.ClientBounds.Width, MainGame.Instance.Window.ClientBounds.Height), Color.White);

                // Draws the scrambled word onto the screen.
                spriteBatch.DrawString(g_font, m_Words.RandomizedWord(), new Vector2((380 - (m_Words.RandomizedWord().Length * 11)), 240), Color.Black);

                // Timer... When under 10 seconds, the decimal makes it not centered with the box bg, so the length is a fix.
                spriteBatch.DrawString(g_font, m_time, new Vector2((635 - ((m_time.Length - 2) * 10)), 60), Color.Black);

                // Scores...
                spriteBatch.DrawString(m_font, "Score: " + m_player.PlayerScore(), new Vector2(90, 50), Color.Black);
                spriteBatch.DrawString(m_font, "Words Solved: " + m_player.PlayerWords(), new Vector2(90, 110), Color.Black);

                // Draws Guess...
                spriteBatch.DrawString(m_font, m_player.WordAttempt(), new Vector2((385 - (m_player.WordAttempt().Length - 2) * 7), 447), Color.Black);

                // Guess text...
                if (m_guess == "Incorrect Guess")
                {
                    spriteBatch.DrawString(g_font, m_guess, new Vector2((320 - (m_guess.Length - 6) * 13), 365), Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(g_font, m_guess, new Vector2(320, 365), Color.Black);
                }

                // Draws submit button...
                Vector2 m_submit = m_font.MeasureString("Try Word");
                r_buttons[1] = new Rectangle(315, 497, (int)m_submit.X + 80, (int)m_submit.Y + 4);
                spriteBatch.Draw(t, r_buttons[1], Color.Black);
                spriteBatch.DrawString(m_font, "Try Word", new Vector2(355, 500), Color.White);

            }
            else
            {
                spriteBatch.DrawString(m_font, "The word was...", new Vector2(300, 200), Color.Black);
                spriteBatch.DrawString(g_font, m_Words.OriginalWord(), new Vector2((385 - (m_Words.OriginalWord().Length * 17)), 240), Color.Black);

                spriteBatch.DrawString(g_font, m_time, new Vector2((635 - ((m_time.Length - 2) * 10)), 60), Color.Red);
            }

            // Back button...
            Vector2 m_back = m_font.MeasureString("Back");
            r_buttons[0] = new Rectangle(690, 547, (int)m_back.X + 20, (int)m_back.Y + 4);
            spriteBatch.Draw(t, r_buttons[0], Color.Black);
            spriteBatch.DrawString(m_font, "Back", new Vector2(700, 550), Color.White);
        }
    }
}
