using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guess_The_Word.State
{
    enum States
    {
        SplashScreen,
        MenuScreen,
        GameScreen
    };

    public abstract class StateBase
    {
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
