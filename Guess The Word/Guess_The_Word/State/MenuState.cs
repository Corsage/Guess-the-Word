using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guess_The_Word;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Guess_The_Word.Core;

/// <summary>
/// All outcomes of the MenuState in the application is controlled here.
/// </summary>

namespace Guess_The_Word.State
{
    public class MenuState : StateBase
    {
        Texture2D m_background;
        Texture2D m_logo;

        Rectangle[] r_menuItems = new Rectangle[4]; // Array from menuItems [4].
        Rectangle r_back; // Rectangle controlling the back "button".

        /// <summary>
        /// Holds all the possible Options within the MenuState.
        /// </summary>
        private string[] menuItems = new string[] {
            "PLAY",
            "HELP",
            "ABOUT",
            "EXIT"
        };

        private bool b_help = false; // Help Screen
        private bool b_about = false; // About Screen

        private SpriteFont m_font; // Menu Items
        private SpriteFont f_font; // Footer

        /// <summary>
        /// Constructor loads the content needed, into the application.
        /// </summary>
        public MenuState()
        {
            m_background = MainGame.Instance.Content.Load<Texture2D>(@"Textures/Background");
            m_logo = MainGame.Instance.Content.Load<Texture2D>(@"Textures/Logo");

            m_font = MainGame.Instance.Content.Load<SpriteFont>(@"Fonts/MenuFont");
            f_font = MainGame.Instance.Content.Load<SpriteFont>(@"Fonts/DefaultFont");
        }

        /// <summary>
        /// Controls what happens when the application is in its MenuState.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            MouseState m_mouse = Mouse.GetState();

            if ((!b_help) && (!b_about))
            {
                if ((m_mouse.LeftButton == ButtonState.Pressed) && (r_menuItems[0].Contains(m_mouse.X, m_mouse.Y)))
                {
                    //m_logo.Dispose();

                    // Runs the Game.
                    MainGame.Instance.m_state = new GameState();
                }
                else if ((m_mouse.LeftButton == ButtonState.Pressed) && (r_menuItems[1].Contains(m_mouse.X, m_mouse.Y)))
                {
                    // Help selected.
                    b_help = true;
                }
                else if ((m_mouse.LeftButton == ButtonState.Pressed) && (r_menuItems[2].Contains(m_mouse.X, m_mouse.Y)))
                {
                    // About selected.
                    b_about = true;
                }
                else if ((m_mouse.LeftButton == ButtonState.Pressed) && (r_menuItems[3].Contains(m_mouse.X, m_mouse.Y)))
                {
                    // Exits the Game.
                    MainGame.Instance.Exit();
                }
            }
            else if ((b_help) && (m_mouse.LeftButton == ButtonState.Pressed) && (r_back.Contains(m_mouse.X, m_mouse.Y)))
            {
                // Turns help "screen" off.
                b_help = false;
            }
            else if ((b_about) && (m_mouse.LeftButton == ButtonState.Pressed) && (r_back.Contains(m_mouse.X, m_mouse.Y)))
            {
                // Turns about "screen" off.
                b_about = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Background and Logo stay static throughout the MenuState.
            spriteBatch.Draw(m_background, new Rectangle(0, 0, MainGame.Instance.Window.ClientBounds.Width, MainGame.Instance.Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(m_logo, new Rectangle(65, 30, 672, 130), Color.White);

            var t = new Texture2D(MainGame.Instance.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });

            if ((b_help) || (b_about))
            {
                if (b_help)
                {
                    // Shows help dialog (from Constants).
                    spriteBatch.DrawString(f_font, Constants.s_help, new Vector2(250, 250), Color.Black);
                }

                else
                {
                    // Shows about dialog (from Constants).
                    spriteBatch.DrawString(f_font, Constants.s_about, new Vector2(255, 250), Color.Black)   ;
                }

                // For both scenarios, a back "button" is created to go back to the main menu.
                Vector2 m_back = m_font.MeasureString("Back");
                r_back = new Rectangle(350, 472, (int)m_back.X + 20, (int)m_back.Y + 4);
                spriteBatch.Draw(t, r_back, Color.Black);
                spriteBatch.DrawString(m_font, "Back", new Vector2(360, 475), Color.White);
            }
            else
            {
                // Each menu item is created with it's own rectangle/text.
                for (int i = 0; i < menuItems.Length; i++)
                {
                    Vector2 m_text = m_font.MeasureString(menuItems[i]);

                    if (i != 2)
                    {
                        // Formating... "ABOUT" has more letters than the rest.
                        r_menuItems[i] = new Rectangle(340, ((60 * i) + 247), (int)m_text.X + 20, (int)m_text.Y + 4);

                        spriteBatch.Draw(t, r_menuItems[i], Color.Black);
                        spriteBatch.DrawString(m_font, menuItems[i], new Vector2(350, ((60 * i) + 250)), Color.White);
                    }
                    else
                    {
                        // Formatting for "ABOUT" has different X placements.
                        r_menuItems[i] = new Rectangle(327, ((60 * i) + 247), (int)m_text.X + 20, (int)m_text.Y + 4);

                        spriteBatch.Draw(t, r_menuItems[i], Color.Black);
                        spriteBatch.DrawString(m_font, menuItems[i], new Vector2(337, ((60 * i) + 250)), Color.White);
                    }
                }

                // l0lz copyright
                spriteBatch.DrawString(f_font, Constants.s_Copyright, Constants.v_Copyright, Color.Black);
            }
        }
    }
}
