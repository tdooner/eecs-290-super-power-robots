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

namespace Project290.Screens.Shared
{
    /// <summary>
    /// Used for representing a node in a hypercube.
    /// </summary>
    public class HypercubeNode
    {
        /// <summary>
        /// Gets the position of the node.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The neighboring nodes.
        /// </summary>
        public List<HypercubeNode> Nodes { get; private set; }

        /// <summary>
        /// The velocity.
        /// </summary>
        private Vector2 velocity;
        
        /// <summary>
        /// What the nodes bounce off of.
        /// </summary>
        private Rectangle bouncingBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="HypercubeNode"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="bouncingBox">The bouncing box.</param>
        public HypercubeNode(Vector2 position, Vector2 velocity, Rectangle bouncingBox)
        {
            this.Position = position;
            this.velocity = velocity;
            this.bouncingBox = bouncingBox;
            this.Nodes = new List<HypercubeNode>();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.Position += this.velocity;
            if (this.Position.X < this.bouncingBox.Left || this.Position.X > this.bouncingBox.Right)
            {
                this.velocity.X = -this.velocity.X;
                this.Position.X = MathHelper.Clamp(this.Position.X, this.bouncingBox.Left, this.bouncingBox.Right);
            }
            if (this.Position.Y < this.bouncingBox.Top || this.Position.Y > this.bouncingBox.Bottom)
            {
                this.velocity.Y = -this.velocity.Y;
                this.Position.Y = MathHelper.Clamp(this.Position.Y, this.bouncingBox.Top, this.bouncingBox.Bottom);
            }
        }

        /// <summary>
        /// Sets the rectangle centers to the specified x, y.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void Set(float x, float y)
        {
            this.Position.X += x - this.bouncingBox.Center.X;
            this.Position.Y += y - this.bouncingBox.Center.Y;

            this.bouncingBox.X = (int)Math.Round(x - this.bouncingBox.Width / 2f);
            this.bouncingBox.Y = (int)Math.Round(y - this.bouncingBox.Height / 2f);
        }
    }
}
