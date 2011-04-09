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

namespace Project290.Storage
{
    /// <summary>
    /// This is an object used to repersent a player (by Gamertag) and his score.
    /// </summary>
    public class PlayerScore
    {
        /// <summary>
        /// Gets the gamertag.
        /// </summary>
        /// <value>The gamertag.</value>
        public string Gamertag { get; private set; }

        /// <summary>
        /// Gets the scores.
        /// </summary>
        /// <value>The scores.</value>
        public int[] Scores { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerScore"/> class.
        /// </summary>
        /// <param name="gamertag">The gamertag.</param>
        /// <param name="scores">The scores.</param>
        public PlayerScore(string gamertag, int[] scores)
        {
            this.Gamertag = gamertag;
            this.Scores = scores;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(int columnIndex)
        {
            StringBuilder toReturn = new StringBuilder();
            toReturn.Append(this.Gamertag.PadRight(18, ' '));
            toReturn.Append(Math.Max(this.Scores[columnIndex], 0));
            return toReturn.ToString();
        }
    }
}
