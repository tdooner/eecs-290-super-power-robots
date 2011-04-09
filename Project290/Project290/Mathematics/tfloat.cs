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

namespace Project290.Mathematics
{
    /// <summary>
    /// This is a Transitioning float, hence the name tfloat. This acts like
    /// a regualar floating point number, but you can call GoTo with the target
    /// value and duration in seconds to have the value linearly transition
    /// there in that amount of time. The value of the float is in the "Value"
    /// variable, and this can be read and reset at any time.
    /// </summary>
    public class tfloat
    {
        /// <summary>
        /// Gets or sets the value corresponding to this tfloat.
        /// </summary>
        /// <value>The value.</value>
        public float Value { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the value is currently transitioning.
        /// </summary>
        public bool IsTransitioning { get; private set; }

        /// <summary>
        /// This is for keeping record of the target value.
        /// </summary>
        public float Target { get; private set; }

        /// <summary>
        /// This is for keeping record of the previous value, so
        /// that the formula for linear interpolation can be used.
        /// </summary>
        private float previous;

        /// <summary>
        /// The duration of the linear interpolation.
        /// </summary>
        private long duration;

        /// <summary>
        /// The time at which the goto method was called.
        /// </summary>
        private long launchTime;

        /// <summary>
        /// Use the real clock or game clock? Use real clock only when paused, and game clock otherwise.
        /// </summary>
        private bool useRealTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="tfloat"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public tfloat(float value)
        {
            this.Value = value;
            this.Target = value;
            this.previous = value;
            this.useRealTime = false;
        }

        /// <summary>
        /// Goes to the specified target in the specified
        /// amount of time.
        /// </summary>
        /// <param name="target">The target to change this tfloat to.</param>
        /// <param name="duration">The amount of time, in seconds, to take in transitioning.</param>
        /// <param name="useRealTime">if set to <c>true</c> [use real time].</param>
        public void GoTo(float target, float duration, bool useRealTime)
        {
            this.previous = this.Value;
            this.Target = target;
            this.duration = (long)(duration * 10000000);
            this.useRealTime = useRealTime;
            if (!useRealTime)
            {
                this.launchTime = GameClock.Now;
            }
            else
            {
                this.launchTime = DateTime.Now.Ticks;
            }
        }

        /// <summary>
        /// Goes to the specified target in the specified
        /// amount of time.
        /// </summary>
        /// <param name="target">The target to change this tfloat to.</param>
        /// <param name="duration">The amount of time, in seconds, to take in transitioning.</param>
        public void GoTo(float target, float duration)
        {
            this.GoTo(target, duration, false);
        }

        /// <summary>
        /// Goes to the specified target in exactly 1 second.
        /// </summary>
        /// <param name="target">The target to change the tfloat to.</param>
        public void GoTo(float target)
        {
            this.GoTo(target, 1f);
        }

        /// <summary>
        /// Determines if the tfloat is at the target value.
        /// </summary>
        /// <returns>
        /// True if the value of the tfloat is at the target value
        /// and false otherwise.
        /// </returns>
        public bool AtTarget()
        {
            return this.Target == this.Value;
        }

        /// <summary>
        /// Updates this instance. This must be called every cycle
        /// or else the tfloat will not transition as expected.
        /// </summary>
        public void Update()
        {
            if (this.Value == this.Target)
            {
                this.IsTransitioning = false;
                return;
            }

            if (!this.IsTransitioning)
            {
                this.IsTransitioning = true;
            }

            if (!useRealTime)
            {
                this.Value = MathHelper.Lerp(this.previous, this.Target, MathHelper.Clamp((GameClock.Now - this.launchTime) / (float)this.duration, 0f, 1f));
            }
            else
            {
                this.Value = MathHelper.Lerp(this.previous, this.Target, MathHelper.Clamp((DateTime.Now.Ticks - this.launchTime) / (float)this.duration, 0f, 1f));
            }
        }

        /// <summary>
        /// Forces the value to be at the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetValue(float value)
        {
            this.Value = value;
            this.Target = value;
        }
    }
}
