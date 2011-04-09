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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Project290.Inputs
{
    /// <summary>
    /// This is used for rumbling the physical controller.
    /// </summary>
    public class RumblePack
    {
        /// <summary>
        /// The duration, in ticks, left to rumble the controller.
        /// </summary>
        private long duration;

        /// <summary>
        /// This is for storing the player index locally.
        /// </summary>
        private PlayerIndex playerIndexStore;

        /// <summary>
        /// Is it rumbling?
        /// </summary>
        private bool rumbling;

        /// <summary>
        /// Gets or sets the index of the player corresponding to this rumble pack.
        /// </summary>
        /// <value>The index of the player.</value>
        public PlayerIndex playerIndex
        {
            get
            {
                return this.playerIndexStore;
            }

            set
            {
                for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
                {
                    GamePad.SetVibration(index, 0f, 0f);
                }

                this.rumbling = false;
                this.playerIndexStore = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RumblePack"/> class.
        /// </summary>
        /// <param name="playerIndex">The playerIndex.</param>
        public RumblePack(PlayerIndex playerIndex)
        {
            this.duration = 0L;
            this.playerIndex = playerIndex;
            this.rumbling = false;
        }

        /// <summary>
        /// Rumbles the controller for the specified forces for the
        /// specified duration in ticks.
        /// </summary>
        /// <param name="left">The left motor force.</param>
        /// <param name="right">The right motor force.</param>
        /// <param name="duration">The duration in ticks.</param>
        public void Rumble(float left, float right, long duration)
        {
            GamePad.SetVibration(this.playerIndex, left, right);

            // Don't change to GameClock
            this.duration = duration + DateTime.Now.Ticks;
            this.rumbling = true;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            // Don't change to GameClock
            if (this.rumbling && DateTime.Now.Ticks > this.duration)
            {
                GamePad.SetVibration(this.playerIndex, 0f, 0f);
                this.rumbling = false;
            }
        }
    }
}
