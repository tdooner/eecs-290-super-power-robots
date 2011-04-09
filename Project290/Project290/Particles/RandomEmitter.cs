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
using Project290.Screens;
using Project290.Clock;

namespace Project290.Particles
{
    /// <summary>
    /// Emits a particle randomly on the screen.
    /// </summary>
    public class RandomEmitter : CircularParticleEmitter
    {
        /// <summary>
        /// The display screen.
        /// </summary>
        private Screen screen;

        /// <summary>
        /// The location of the emitter.
        /// </summary>
        private Vector2 location;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomEmitter"/> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        /// <param name="particleDelay">The particle delay (in ticks).</param>
        /// <param name="action">The action.</param>
        public RandomEmitter(Screen screen, long particleDelay, float particleLaunchVelocity, ParticleAction action)
            : base(particleDelay, particleLaunchVelocity, action, screen.Particles)
        {
            this.location = new Vector2();
            this.screen = screen;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.location.X = (float)(this.screen.random.NextDouble() * 1920);
            this.location.Y = (float)(this.screen.random.NextDouble() * 1080);

            base.Update(this.location);
        }
    }
}
