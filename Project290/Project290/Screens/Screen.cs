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
using Project290.Particles;
using Project290.Physics.Dynamics;
using Project290.GameElements;

namespace Project290.Screens
{
    /// <summary>
    /// This is a placeholder public class in which screens should inherit from.
    /// It contains methods for Updating, Drawing, and Resetting.
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Screen"/> is disposed.
        /// If this is set to true, the Screen Stack will automatically take it out of the stack.
        /// Just set this value to true and do nothing else if you want to delete the screen.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed { get; set; }

        /// <summary>
        /// Gets the random number generator for this screen.
        /// </summary>
        /// <value>The random number generator.</value>
        public Random random { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [fading out].
        /// If a screen is fading out, the screen under it on the stack
        /// will get updated. Otherwise, the screen under it on the stack
        /// will not get updated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [fading out]; otherwise, <c>false</c>.
        /// </value>
        public bool FadingOut { get; set; }

        /// <summary>
        /// The particle container for all particles launched on this screen. This is null unless set.
        /// </summary>
        public ParticleContainer Particles { get; internal set; }

        /// <summary>
        /// Gets the physical world for Farseer stuff. This is null unless set.
        /// </summary>
        public World PhysicalWorld { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        public Screen()
        {
            this.random = new Random();
            this.Disposed = false;
            this.FadingOut = false;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public virtual void Update()
        {
            if (this.Particles != null)
            {
                this.Particles.Update();
            }

            if (this.PhysicalWorld != null)
            {
                this.PhysicalWorld.Step(0.01f);
            }
        }

        /// <summary>
        /// Determines whether this instance is on the top of the screen stack.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is on top; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOnTop()
        {
            if (GameWorld.screens == null || GameWorld.screens.Count == 0)
            {
                return false;
            }

            return GameWorld.screens[GameWorld.screens.Count - 1].Equals(this);
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public virtual void Draw()
        {
            if (this.Particles != null)
            {
                this.Particles.Draw();
            }
        }
    }
}