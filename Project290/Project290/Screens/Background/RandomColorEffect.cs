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
using Microsoft.Xna.Framework.Graphics;
using Project290.Clock;
using Project290.Rendering;

namespace Project290.Screens.Background
{
    /// <summary>
    /// Used for displaying a background color effect.
    /// </summary>
    public class RandomColorEffect
    {
        /// <summary>
        /// The number of ticks added to GameClock.Now to create "random" movement.
        /// </summary>
        private long timeOffset;
        
        /// <summary>
        /// Where this is.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The rotation of this.
        /// </summary>
        private float rotation;

        /// <summary>
        /// The color to display this as.
        /// </summary>
        private Color color;
        
        /// <summary>
        /// The transparency.
        /// </summary>
        private float maxAlpha;

        /// <summary>
        /// The layer depth.
        /// </summary>
        private float layerDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomColorEffect"/> class.
        /// </summary>
        /// <param name="alpha">The alpha transparency.</param>
        /// <param name="layerDepth">The layer depth.</param>
        /// <param name="random">The random number generator.</param>
        public RandomColorEffect(byte alpha, float layerDepth, Random random)
        {
            this.maxAlpha = (float)alpha / 255f;
            this.timeOffset = (long)(random.NextDouble() * 123456789876);
            this.position = new Vector2();
            this.color = new Color();
            this.layerDepth = layerDepth;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            float now = ((GameClock.Now + this.timeOffset) / 1234567898f);
            float sinNow = (float)Math.Sin(now);
            this.rotation = (float)(Math.Cos(now * sinNow)) * MathHelper.Pi + MathHelper.Pi;
            this.position.X = (float)Math.Cos(now * Math.Sin(0.7236743534 * now)) * 1920f / 2f + 1920f / 2f;
            this.position.Y = (float)Math.Cos(now * Math.Cos(now)) * 1080f / 2f + 1080f / 2f;
            float alpha = (float)((sinNow * 0.5f + 0.5f) * this.maxAlpha);

            long offset = (GameClock.Now + this.timeOffset) % 123456789L;
            float hue = 6f * offset / 123456789f;
            float x = 1 - Math.Abs(hue % 2 - 1);
            if (hue < 1)
            {
                this.color.R = (byte)(255 * alpha);
                this.color.G = (byte)(x * 255 * alpha);
                this.color.B = 0;
                this.color.A = (byte)(255 * alpha);
            }
            else if (hue < 2)
            {
                this.color.R = (byte)(x * 255 * alpha);
                this.color.G = (byte)(255 * alpha);
                this.color.B = 0;
                this.color.A = (byte)(255 * alpha);
            }
            else if (hue < 3)
            {
                this.color.R = 0;
                this.color.G = (byte)(255 * alpha);
                this.color.B = (byte)(x * 255 * alpha);
                this.color.A = (byte)(255 * alpha);
            }
            else if (hue < 4)
            {
                this.color.R = 0;
                this.color.G = (byte)(x * 255 * alpha);
                this.color.B = (byte)(255 * alpha);
                this.color.A = (byte)(255 * alpha);
            }
            else if (hue < 5)
            {
                this.color.R = (byte)(x * 255 * alpha);
                this.color.G = 0;
                this.color.B = (byte)(255 * alpha);
                this.color.A = (byte)(255 * alpha);
            }
            else
            {
                this.color.R = (byte)(255 * alpha);
                this.color.G = 0;
                this.color.B = (byte)(x * 255 * alpha);
                this.color.A = (byte)(255 * alpha);
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            Drawer.Draw(
                TextureStatic.Get("checkers"),
                this.position,
                null,
                this.color,
                this.rotation,
                TextureStatic.GetOrigin("checkers"),
                4f,
                SpriteEffects.None,
                this.layerDepth);
        }
    }
}
