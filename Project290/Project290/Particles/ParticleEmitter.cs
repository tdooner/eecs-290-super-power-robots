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

namespace Project290.Particles
{
    /// <summary>
    /// Used for emitting particles.
    /// </summary>
    public interface ParticleEmitter
    {
        /// <summary>
        /// When should the next particle be launched?
        /// </summary>
        long NextParticleTime { get; set; }

        /// <summary>
        /// The delay between particles, in ticks.
        /// </summary>
        long ParticleDelay { get; set; }

        /// <summary>
        /// The action that draws and updates a particle.
        /// </summary>
        ParticleAction Action { get; set; }

        /// <summary>
        /// The particles to add a particle to.
        /// </summary>
        ParticleContainer particles { get; set; }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <param name="position">The position of this emitter.</param>
        void Update(Vector2 position);
    }
}
