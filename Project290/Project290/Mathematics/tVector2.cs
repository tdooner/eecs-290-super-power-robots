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

namespace Project290.Mathematics
{
    /// <summary>
    /// This is a transitioning Vector2. This is used like a tfloat.
    /// </summary>
    public class tVector2
    {
        /// <summary>
        /// Value for X for the Vector2.
        /// </summary>
        /// <value>The X value.</value>
        public tfloat X { get; private set; }

        /// <summary>
        /// Value for Y for the Vector2.
        /// </summary>
        /// <value>The Y value.</value>
        public tfloat Y { get; private set; }

        /// <summary>
        /// The temp vector so that new Vector2 values don't need to be created.
        /// </summary>
        private Vector2 tempV2;

        /// <summary>
        /// Initializes a new instance of the <see cref="tVector2"/> class.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public tVector2(float x, float y)
        {
            this.X = new tfloat(x);
            this.Y = new tfloat(y);
            this.tempV2 = new Vector2();
        }

        /// <summary>
        /// Goes to the specified x,y target.
        /// </summary>
        /// <param name="xTarget">The x target.</param>
        /// <param name="yTarget">The y target.</param>
        public void GoTo(float xTarget, float yTarget)
        {
            this.GoTo(xTarget, yTarget, 1);
        }

        /// <summary>
        /// Goes to the specified x,y target.
        /// </summary>
        /// <param name="xTarget">The x target.</param>
        /// <param name="yTarget">The y target.</param>
        /// <param name="duration">The duration in seconds.</param>
        public void GoTo(float xTarget, float yTarget, float duration)
        {
            this.X.GoTo(xTarget, duration);
            this.Y.GoTo(yTarget, duration);
        }

        /// <summary>
        /// Goes to the specified x,y target.
        /// </summary>
        /// <param name="xTarget">The x target.</param>
        /// <param name="yTarget">The y target.</param>
        /// <param name="duration">The duration in seconds.</param>
        /// <param name="useRealTime">if set to <c>true</c> [use real time].</param>
        public void GoTo(float xTarget, float yTarget, float duration, bool useRealTime)
        {
            this.X.GoTo(xTarget, duration, useRealTime);
            this.Y.GoTo(yTarget, duration, useRealTime);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is transitioning.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is transitioning; otherwise, <c>false</c>.
        /// </value>
        public bool IsTransitioning
        {
            get
            {
                return this.X.IsTransitioning || this.Y.IsTransitioning;
            }
        }

        /// <summary>
        /// Sets the specified Vector2 value.
        /// </summary>
        /// <param name="xValue">The x value.</param>
        /// <param name="yValue">The y value.</param>
        public void Set(float xValue, float yValue)
        {
            this.X.SetValue(xValue);
            this.Y.SetValue(yValue);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.X.Update();
            this.Y.Update();
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        public Vector2 Target
        {
            get
            {
                this.tempV2.X = this.X.Target;
                this.tempV2.Y = this.Y.Target;
                return tempV2;
            }
        }

        /// <summary>
        /// Gets the value corresponding to this Vector2.
        /// </summary>
        /// <value>The value.</value>
        public Vector2 Value
        {
            get
            {
                this.tempV2.X = this.X.Value;
                this.tempV2.Y = this.Y.Value;
                return tempV2;
            }
        }
    }
}
