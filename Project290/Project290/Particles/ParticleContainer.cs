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
using Project290.Clock;

namespace Project290.Particles
{
    /// <summary>
    /// Used for storing a large number of Particles. This efficiently handles storing and updating them.
    /// </summary>
    public class ParticleContainer
    {
        /// <summary>
        /// The active particles in the Particles Array.
        /// </summary>
        public Queue<Particle> Particles { get; private set; }
        
        /// <summary>
        /// Used for getting the next particle to add.
        /// </summary>
        private Stack<Particle> DeletedParticles;

        /// <summary>
        /// The max particles.
        /// </summary>
        private int maxParticles;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleContainer"/> class.
        /// </summary>
        /// <param name="maxParticles">The maximum number of particles.</param>
        public ParticleContainer(int maxParticles)
        {
            this.Particles = new Queue<Particle>(maxParticles);
            this.maxParticles = maxParticles;
            this.DeletedParticles = new Stack<Particle>(maxParticles);

            // By default maxParticles / 2 "blank" particles are added, so that the game will not lag at first.
            for (int i = 0; i < maxParticles / 2f; i++)
            {
                this.Add(0, 0, 0, 0, Color.White, 0, 0, 0, null);
            }

            this.Reset();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            foreach (Particle particle in this.Particles)
            {
                particle.Dispose();
            }
        }

        /// <summary>
        /// Adds a Particle.
        /// </summary>
        /// <param name="xPosition">The x position.</param>
        /// <param name="yPosition">The y position.</param>
        /// <param name="xVelocity">The x velocity.</param>
        /// <param name="yVelocity">The y velocity.</param>
        /// <param name="color">The color to overlay the drawn image with.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="actionUniqueIdentifier">This is used to identify what "type" in a custom action this particle belongs to.
        /// For example, you may want to have one action which could draw one of several different
        /// images. You can set this value to indicate which image to draw. This does not have
        /// to be used.</param>
        /// <param name="action">The action. This should be a reference to a STATIC object, for memory sake.</param>
        /// <returns>
        /// False if the particle was not added, true if it was.
        /// </returns>
        public bool Add(float xPosition, float yPosition, float xVelocity, float yVelocity, Color color, float scale, float rotation, byte actionUniqueIdentifier, ParticleAction action)
        {
            if (this.Particles.Count < this.maxParticles)
            {
                if (this.DeletedParticles.Count == 0)
                {
                    this.Particles.Enqueue(new Particle(xPosition, yPosition, xVelocity, yVelocity, color, scale, rotation, actionUniqueIdentifier, action));
                }
                else
                {
                    Particle newParticle = this.DeletedParticles.Pop();
                    newParticle.Reset(xPosition, yPosition, xVelocity, yVelocity, color, scale, rotation, actionUniqueIdentifier, action);
                    this.Particles.Enqueue(newParticle);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a Particle.
        /// </summary>
        /// <param name="xPosition">The x position.</param>
        /// <param name="yPosition">The y position.</param>
        /// <param name="xVelocity">The x velocity.</param>
        /// <param name="yVelocity">The y velocity.</param>
        /// <param name="color">The color to overlay the drawn image with.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="rotationSpeed">The rotation speed.</param>
        /// <param name="actionUniqueIdentifier">This is used to identify what "type" in a custom action this particle belongs to.
        /// For example, you may want to have one action which could draw one of several different
        /// images. You can set this value to indicate which image to draw. This does not have
        /// to be used.</param>
        /// <param name="action">The action. This should be a reference to a STATIC object, for memory sake.</param>
        /// <returns>
        /// False if the particle was not added, true if it was.
        /// </returns>
        public bool Add(float xPosition, float yPosition, float xVelocity, float yVelocity, Color color, float scale, float rotation, float rotationSpeed, byte actionUniqueIdentifier, ParticleAction action)
        {
            if (this.Particles.Count < this.maxParticles)
            {
                if (this.DeletedParticles.Count == 0)
                {
                    this.Particles.Enqueue(new Particle(xPosition, yPosition, xVelocity, yVelocity, color, scale, rotation, rotationSpeed, actionUniqueIdentifier, action));
                }
                else
                {
                    Particle newParticle = this.DeletedParticles.Pop();
                    newParticle.Reset(xPosition, yPosition, xVelocity, yVelocity, color, scale, rotation, rotationSpeed, actionUniqueIdentifier, action);
                    this.Particles.Enqueue(newParticle);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Applies the acceloration to all particles.
        /// </summary>
        /// <param name="acceloration">The acceloration.</param>
        public void ApplyAcceloration(ref Vector2 acceloration)
        {
            foreach (Particle particle in this.Particles)
            {
                particle.ApplyAcceloration(ref acceloration);
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.CheckToSeeIfGameClockReset();

            int queueCount = this.Particles.Count;
            for (int i = 0; i < queueCount; i++)
            {
                Particle current = this.Particles.Dequeue();
                current.Update();

                if (current.Disposed)
                {
                    this.DeletedParticles.Push(current);
                }
                else
                {
                    this.Particles.Enqueue(current);
                }
            }
        }

        /// <summary>
        /// Checks to see if game clock reset.
        /// </summary>
        private void CheckToSeeIfGameClockReset()
        {
            // If the gameclock reset, delete all particles.
            if (GameClock.Now < 500000)
            {
                this.Reset();
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            foreach (Particle particle in this.Particles)
            {
                particle.Draw();
            }
        }
    }
}
