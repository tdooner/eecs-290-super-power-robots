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
using Project290.Mathematics;

namespace Project290.Screens.Shared
{
    /// <summary>
    /// A hypercube!
    /// </summary>
    public class Hypercube : List<HypercubeNode>
    {
        /// <summary>
        /// Layer depth of all lines.
        /// </summary>
        public float LayerDepth { get; set; }

        /// <summary>
        /// The color of the hypercube.
        /// </summary>
        private Color color;

        /// <summary>
        /// The rectangle to use to clip the lines.
        /// </summary>
        private Rectangle drawingClipRectangle;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Hypercube"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        /// <param name="drawingClipRectangle">The drawing clip rectangle.</param>
        /// <param name="bounceRectangle">The bounce rectangle.</param>
        /// <param name="random">The PRNG.</param>
        public Hypercube(Vector2 position, Color color, Rectangle drawingClipRectangle, Rectangle bounceRectangle, Random random, float layerDepth)
            : base()
        {
            this.drawingClipRectangle = drawingClipRectangle;
            this.color = color;
            this.LayerDepth = layerDepth;
            for (int i = 0; i < 16; i++)
            {
                this.Add(new HypercubeNode(
                    position,
                    new Vector2((float)(2 * (random.NextDouble() * 2 - 1)), (float)(2 * (random.NextDouble() * 2 - 1))),
                    bounceRectangle));
            }

            this[0].Nodes.Add(this[1]);
            this[0].Nodes.Add(this[2]);
            this[0].Nodes.Add(this[4]);
            this[0].Nodes.Add(this[10]);
            this[1].Nodes.Add(this[3]);
            this[1].Nodes.Add(this[11]);
            this[1].Nodes.Add(this[5]);
            this[2].Nodes.Add(this[3]);
            this[2].Nodes.Add(this[6]);
            this[2].Nodes.Add(this[8]);
            this[3].Nodes.Add(this[7]);
            this[3].Nodes.Add(this[9]);
            this[4].Nodes.Add(this[5]);
            this[4].Nodes.Add(this[6]);
            this[4].Nodes.Add(this[14]);
            this[5].Nodes.Add(this[7]);
            this[5].Nodes.Add(this[15]);
            this[6].Nodes.Add(this[7]);
            this[6].Nodes.Add(this[12]);
            this[7].Nodes.Add(this[13]);
            this[8].Nodes.Add(this[9]);
            this[8].Nodes.Add(this[10]);
            this[8].Nodes.Add(this[12]);
            this[9].Nodes.Add(this[11]);
            this[9].Nodes.Add(this[13]);
            this[10].Nodes.Add(this[11]);
            this[10].Nodes.Add(this[14]);
            this[11].Nodes.Add(this[15]);
            this[12].Nodes.Add(this[13]);
            this[12].Nodes.Add(this[14]);
            this[13].Nodes.Add(this[15]);
            this[14].Nodes.Add(this[15]);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            foreach (HypercubeNode node in this)
            {
                node.Update();
            }
        }

        /// <summary>
        /// Sets the rectangles to the specified x, y.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void Set(float x, float y)
        {
            this.drawingClipRectangle.X = (int)Math.Round(x - this.drawingClipRectangle.Width / 2f);
            this.drawingClipRectangle.Y = (int)Math.Round(y - this.drawingClipRectangle.Height / 2f);
            foreach (HypercubeNode node in this)
            {
                node.Set(x, y);
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            foreach (HypercubeNode node in this)
            {
                foreach (HypercubeNode node2 in node.Nodes)
                {
                    Vector2 v1 = node.Position;
                    Vector2 v2 = node2.Position;
                    if (RectangleLineClipper.ClipLine(ref v1, ref v2, ref this.drawingClipRectangle))
                    {
                        Drawer.DrawLine(v1, v2, 2f, this.LayerDepth, this.color);
                    }
                }
            }
        }
    }
}
