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

namespace Project290.Clock
{
    /// <summary>
    /// GameClock is used to statically keep track of the
    /// game time. It can be called from anywhere without
    /// any references needed.
    /// </summary>
    static public class GameClock
    {
        /// <summary>
        /// Determines if the game is paused. If so, the 
        /// clock will not "tick".
        /// </summary>
        private static bool paused = false;

        /// <summary>
        /// The total time elapsed since the start of the
        /// game. This does not could paused time.
        /// </summary>
        private static long timeElapsed = 0;

        /// <summary>
        /// A variable used to compute the timeElapsed variable
        /// by computing the dt.
        /// </summary>
        private static long lastTime = DateTime.Now.Ticks;

        /// <summary>
        /// Gets or sets the total number of 100-nanoseconds that
        /// have elapsed in the game. This value can be
        /// modified, but beware that doing so will likely
        /// cause an unforeseen error.
        /// </summary>
        /// <value>
        /// The total time elapsed in the game.
        /// </value>
        public static long Now
        {
            get
            {
                return timeElapsed;
            }

            set
            {
                timeElapsed = value;
            }
        }

        /// <summary>
        /// Updates this instance. This needs to be called every
        /// game cycle.
        /// </summary>
        public static void Update()
        {
            if (!paused)
            {
                timeElapsed += DateTime.Now.Ticks - lastTime;
            }

            lastTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Determines whether this instance is paused.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is paused; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPaused()
        {
            return paused;
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public static void Pause()
        {
            paused = true;
        }

        /// <summary>
        /// Unpauses this instance.
        /// </summary>
        public static void Unpause()
        {
            paused = false;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public static void Reset()
        {
            timeElapsed = 0;
            Unpause();
        }
    }
}