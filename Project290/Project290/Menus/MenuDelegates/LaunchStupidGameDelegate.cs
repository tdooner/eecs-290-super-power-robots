//////__     __               _                 _     _               _   
//////\ \   / /              | |               | |   | |             | |  
////// \ \_/ /__  _   _   ___| |__   ___  _   _| | __| |  _ __   ___ | |_ 
//////  \   / _ \| | | | / __| '_ \ / _ \| | | | |/ _` | | '_ \ / _ \| __|
//////   | | (_) | |_| | \__ \ | | | (_) | |_| | | (_| | | | | | (_) | |_ 
//////   |_|\___/ \__,_| |___/_| |_|\___/ \__,_|_|\__,_| |_| |_|\___/ \__|
//////      _                              _   _     _     _ 
//////     | |                            | | | |   (_)   | |
//////  ___| |__   __ _ _ __   __ _  ___  | |_| |__  _ ___| |
////// / __| '_ \ / _` | '_ \ / _` |/ _ \ | __| '_ \| / __| |
//////| (__| | | | (_| | | | | (_| |  __/ | |_| | | | \__ \_|
////// \___|_| |_|\__,_|_| |_|\__, |\___|  \__|_| |_|_|___(_)
//////                         __/ |                         
//////                        |___/                          

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.GameElements;
using Project290.Games.StupidGame;

namespace Project290.Menus.MenuDelegates
{
    /// <summary>
    /// This pops the most recent screen off of the screen
    /// stack of GameWorld and lauches a new instance of StupidGameScreen.
    /// </summary>
    public class LaunchStupidGameDelegate : IMenuDelegate
    {
        /// <summary>
        /// The score board index.
        /// </summary>
        private int scoreBoardIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaunchStupidGameDelegate"/> class.
        /// </summary>
        /// <param name="scoreBoardIndex">Index into the score board.</param>
        public LaunchStupidGameDelegate(int scoreBoardIndex)
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

            GameWorld.screens.Play(new StupidGameScreen(this.scoreBoardIndex)); // The number passed here must be unique per game.
        }
    }
}
