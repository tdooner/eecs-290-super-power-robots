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
using Project290.Clock;
using Project290.GameElements;
using Project290.Screens.Title;
using System.Threading;

namespace Project290.Screens
{
    /// <summary>
    /// This is a Screen specific for a game (as opposed to title or pause screen).
    /// </summary>
    public class GameScreen : Screen
    {
        /// <summary>
        /// The index for this game into the score board.
        /// </summary>
        public int ScoreboardIndex { get; private set; }

        /// <summary>
        /// The current score of the game.
        /// </summary>
        public uint Score { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameScreen"/> class.
        /// </summary>
        public GameScreen(int scoreboardIndex)
            : base()
        {
            this.ScoreboardIndex = scoreboardIndex;
            this.FadingOut = true;
        }

        /// <summary>
        /// Resets this instance. This will be called when a new game is played. Reset all of
        /// your objects here so that you do not need to reallocate the memory for them.
        /// </summary>
        internal virtual void Reset()
        {
            this.Score = 0;
            GameClock.Reset();
        }

        /// <summary>
        /// Call this method to end the game.
        /// </summary>
        internal virtual void GameOver()
        {
            // Make sure this is on top so this doesn't get called multiple times
            if (this.IsOnTop())
            {
                if (GameWorld.controller.gamer != null && GameWorld.gameSaver != null)
                {
                    new Thread(this.SaveScore).Start();
                }

                GameWorld.screens.Add(new GameOverScreen(this.Score));
            }
        }


        /// <summary>
        /// Saves the player's score. This is async.
        /// </summary>
        private void SaveScore()
        {
            try
            {
                if (GameWorld.gameSaver.GetScore(GameWorld.controller.gamer.Gamertag, this.ScoreboardIndex) < (int)this.Score)
                {
                    GameWorld.gameSaver.SetScore(GameWorld.controller.gamer.Gamertag, (int)this.Score, this.ScoreboardIndex);
                    GameWorld.gameSaver.Write();
                }
            }
            catch { }
        }
    }
}
