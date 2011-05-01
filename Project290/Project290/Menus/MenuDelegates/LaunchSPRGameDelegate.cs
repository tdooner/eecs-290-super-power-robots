using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.GameElements;
using Project290.Games.StupidGame;
using Project290.Games.SuperPowerRobots;

namespace Project290.Menus.MenuDelegates
{
    /// <summary>
    /// This pops the most recent screen off of the screen
    /// stack of GameWorld and lauches a new instance of StupidGameScreen.
    /// </summary>
    public class LaunchSPRGameDelegate : IMenuDelegate
    {
        /// <summary>
        /// The score board index.
        /// </summary>
        private int scoreBoardIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaunchStupidGameDelegate"/> class.
        /// </summary>
        /// <param name="scoreBoardIndex">Index into the score board.</param>
        public LaunchSPRGameDelegate(int scoreBoardIndex)
            : base()
        {
            this.scoreBoardIndex = scoreBoardIndex;
        }

        /// <summary>
        /// Runs this instance. This pops the most recent screen off of the screen
        /// stack of GameWorld and launches a new instance of StupidGameScreen.
        /// </summary>
        public void Run()
        {
            if (GameWorld.screens.Count > 0)
            {
                GameWorld.screens[GameWorld.screens.Count - 1].Disposed = true;
            }

            GameWorld.screens.Play(new SPRMainScreen(this.scoreBoardIndex)); // The number passed here must be unique per game.
        }
    }
}
