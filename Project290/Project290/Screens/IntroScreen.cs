using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Clock;
using Project290.GameElements;
using Project290.Screens.Title;
using System.Threading;

namespace Project290.Screens
{
    /// <summary>
    /// This is a Screen specific for a game (as opposed to title or pause screen).
    /// </summary>
    public class IntroScreen : Screen
    {
        public IntroScreen()
            : base()
        {
            this.FadingOut = false;
        }


        /// <summary>
        /// Call this method to end the game.
        /// </summary>
        internal virtual void IntroOver()
        {
            // Make sure this is on top so this doesn't get called multiple times
            if (this.IsOnTop())
            {
                GameWorld.screens.Add(new GameScreen(1));
            }
        }
    }
}
