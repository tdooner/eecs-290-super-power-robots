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
using Microsoft.Xna.Framework;

namespace Project290.Particles
{
    /// <summary>
    /// Launches particles in a radial fashion.
    /// </summary>
    public class CircularParticleEmitter : ParticleEmitter
    {
        /// <summary>
        /// When should the next particle be launched?
        /// </summary>
        public long NextParticleTime { get; set; }

        /// <summary>
        /// The delay between particles, in ticks.
        /// </summary>
        public long ParticleDelay { get; set; }

        /// <summary>
        /// The action that draws and updates a particle.
        /// </summary>
        public ParticleAction Action { get; set; }

        /// <summary>
        /// The particles to add a particle to.
        /// </summary>
        public ParticleContainer particles { get; set; }

        /// <summary>
        /// The launch velocity of a particle.
        /// </summary>
        private float velocity;

        /// <summary>
        /// A PRNG.
        /// </summary>
        private Random random;

        /// <summary>
        /// The previous position is recorded for smootly lerping.
        /// </summary>
        private Vector2 previousPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularParticleEmitter"/> class.
        /// </summary>
        /// <param name="particleDelay">The particle delay.</param>
        /// <param name="velocity">The velocity of a particle that is launched.</param>
        /// <param name="action">The action that draws and updates a particle.</param>
        /// <param name="particles">The particles to add a particle to.</param>
        public CircularParticleEmitter(long particleDelay, float velocity, ParticleAction action, ParticleContainer particles)
        {
            this.ParticleDelay = particleDelay;
            this.NextParticleTime = GameClock.Now + particleDelay;
            this.velocity = velocity;
            this.Action = action;
            this.particles = particles;
            this.random = new Random();
            this.previousPosition = new Vector2(float.MinValue);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <param name="position">The position of this emitter.</param>
        public void Update(Vector2 position)
        {
            // Check to see if game clock has been reset.
            if (GameClock.Now < 500000)
            {
                // Reset this to 0 so that the emitter will not stop emitting particles. Note
                // that this is not necessary unless a background screen is using particles,
                // because the GameClock should not be reset mid-game, ever.
                this.NextParticleTime = 0;
            }

            if (this.NextParticleTime < GameClock.Now && this.previousPosition.X != float.MinValue)
            {
                int particlesToRelease = (int)((GameClock.Now - this.NextParticleTime) / (float)this.ParticleDelay);

                for (int i = 0; i < particlesToRelease; i++)
                {
                    float velcoity = (float)(this.random.NextDouble() * this.velocity);
                    float angle = (float)(this.random.NextDouble() * MathHelper.TwoPi);
                    float lerpValue = i / (float)particlesToRelease;
                    if (!this.particles.Add(
                        MathHelper.Lerp(position.X, this.previousPosition.X, lerpValue),
                        MathHelper.Lerp(position.Y, this.previousPosition.Y, lerpValue),
                        (float)(velcoity * Math.Cos(angle)),
                        (float)(velcoity * Math.Sin(angle)),
                        Color.Transparent,      // Doesn't matter for this action.
                        1f,                     // Default
                        0f,                     // Default
                        (float)this.random.NextDouble() * 0.01f - 0.005f,
                        (byte)this.random.Next(2),
                        this.Action))
                    {
                        break;
                    }
                }
                this.NextParticleTime += this.ParticleDelay * particlesToRelease;
            }

            this.previousPosition = position;
        }
    }
}
