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
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Project290.Screens.Shared
{
    /// <summary>
    /// Used for displaying hypercubes.
    /// </summary>
    public class HypercubeDisplay
    {
        /// <summary>
        /// The bounding rectangle for the hypercubes and background.
        /// </summary>
        public Rectangle DisplayRectangle;

        /// <summary>
        /// Some hypercubes to draw.
        /// </summary>
        private List<Hypercube> hypercubes;

        /// <summary>
        /// The layer depth.
        /// </summary>
        private float layerDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="HypercubeDisplay"/> class.
        /// </summary>
        /// <param name="displayRectangle">The display rectangle.</param>
        /// <param name="hypercubeCount">The number of hypercubes.</param>
        /// <param name="random">The PRNG.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public HypercubeDisplay(Rectangle displayRectangle, int hypercubeCount, Random random, float layerDepth)
        {
            this.DisplayRectangle = displayRectangle;            
            this.layerDepth = layerDepth;
            this.hypercubes = new List<Hypercube>();
            Rectangle bounceRectangle = new Rectangle(this.DisplayRectangle.X, this.DisplayRectangle.Y, this.DisplayRectangle.Width, this.DisplayRectangle.Height);
            bounceRectangle.Inflate(this.DisplayRectangle.Height, this.DisplayRectangle.Width);

            for (int i = 0; i < hypercubeCount; i++)
            {
                float greyscale = MathHelper.Lerp(0f, 1f, (i + 1f) / (hypercubeCount + 2f));
                this.hypercubes.Add(new Hypercube(
                    new Vector2(displayRectangle.Center.X, displayRectangle.Center.Y),
                    new Color(0, 0, greyscale, 1f),
                    this.DisplayRectangle,
                    bounceRectangle,
                    random,
                    this.layerDepth + 0.001f * (i + 1)));
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            foreach (Hypercube hypercube in this.hypercubes)
            {
                hypercube.Update();
            }
        }

        /// <summary>
        /// Sets the rectangle to the x, y specified.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void Set(float x, float y)
        {
            this.DisplayRectangle.X = (int)Math.Round(x - this.DisplayRectangle.Width / 2f);
            this.DisplayRectangle.Y = (int)Math.Round(y - this.DisplayRectangle.Height / 2f);
            foreach (Hypercube hypercube in this.hypercubes)
            {
                hypercube.Set(x, y);
            }
        }

        /// <summary>
        /// Sets the layer depth.
        /// </summary>
        /// <param name="layerDepth">The layer depth.</param>
        public void SetLayerDepth(float layerDepth)
        {
            this.layerDepth = layerDepth;
            for (int i = 0; i < this.hypercubes.Count; i++)
            {
                this.hypercubes[i].LayerDepth = layerDepth + 0.001f * (i + 1);
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            Drawer.Draw(
                TextureStatic.Get("gradient"),
                this.DisplayRectangle,
                null,
                Color.Blue,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                this.layerDepth);
            foreach (Hypercube hypercube in this.hypercubes)
            {
                hypercube.Draw();
            }
        }
    }
}
