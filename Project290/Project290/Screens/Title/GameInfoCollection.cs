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

namespace Project290.Screens.Title
{
    /// <summary>
    /// This class is used for statically holding all GameInfo objects, one per game.
    /// </summary>
    public static class GameInfoCollection
    {
        /// <summary>
        /// The list of game info data.
        /// </summary>
        public static List<GameInfo> GameInfos = new List<GameInfo>();

        /// <summary>
        /// Gets the current index (most recent) into the list, for display purposes,
        /// and also so that when returning to the title screen, the most recently
        /// played game is still highlighted.
        /// </summary>
        /// <value>
        /// The current index.
        /// </value>
        public static int CurrentIndex { get; private set; }

        /// <summary>
        /// Shifts the current index by the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public static void ShiftIndex(int amount)
        {
            if (amount != 0)
            {
                GameWorld.audio.PlaySound("boxArtScroll");
            }

            CurrentIndex += amount;
            CurrentIndex = ((CurrentIndex + GameInfos.Count) % GameInfos.Count);
        }

        /// <summary>
        /// Lauches the game that is currently selected.
        /// </summary>
        public static void LauchGame()
        {
            GameInfos[CurrentIndex].StartGame();
        }
    }
}
