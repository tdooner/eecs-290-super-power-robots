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

namespace Project290.Particles
{
    /// <summary>
    /// Bounces around the screen and emitts different colored particles which fade away smoothly.
    /// </summary>
    public class ParticleBouncer : CircularParticleEmitter
    {
        /// <summary>
        /// The location of the bouncer.
        /// </summary>
        private Vector2 location;

        /// <summary>
        /// The velocity.
        /// </summary>
        private Vector2 velocity;
        
        /// <summary>
        /// The display screen.
        /// </summary>
        private Screen screen;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleBouncer"/> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        /// <param name="particleDelay">The particle delay (in ticks).</param>
        /// <param name="action">The action.</param>
        public ParticleBouncer(Screen screen, long particleDelay, float particleLaunchVelocity, ParticleAction action)
            : base(particleDelay, particleLaunchVelocity, action, screen.Particles)
        {
            this.screen = screen;
            this.location = new Vector2(1920, 1080) / 2f;
            this.velocity = new Vector2(9, 5.7f);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            if (this.location.Y > 1080 || this.location.Y < 0)
            {
                this.velocity.Y = -this.velocity.Y;
            }

            if (this.location.X > 1920 || this.location.X < 0)
            {
                this.velocity.X = -this.velocity.X;
            }

            this.location += this.velocity;

            base.Update(this.location);
        }
    }
}