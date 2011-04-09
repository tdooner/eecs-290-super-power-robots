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
    /// Used for clipping lines with a rectangle.
    /// </summary>
    public static class RectangleLineClipper
    {
        /// <summary>
        /// Clips the specified line. Returns false if this does not lie within
        /// the specified rectangle. Otherwise, modifies the points.
        /// </summary>
        /// <param name="pointA">The point A.</param>
        /// <param name="pointB">The point B.</param>
        /// <returns>The clipped line as modified points, or false if there is no line to return.</returns>
        public static bool ClipLine(ref Vector2 pointA, ref Vector2 pointB, ref Rectangle clippingRectangle)
        {
            // First check to see if it cannot be clipped.
            if (!CheckLine(ref pointA, ref pointB, ref clippingRectangle))
            {
                return false;
            }

            // Clip against each edge.
            if (pointA.X > clippingRectangle.Left && pointB.X < clippingRectangle.Left)
            {
                pointB.Y = MathHelper.Lerp(pointA.Y, pointB.Y, (pointA.X - clippingRectangle.Left) / (pointA.X - pointB.X));
                pointB.X = clippingRectangle.Left;
            }
            if (pointB.X > clippingRectangle.Left && pointA.X < clippingRectangle.Left)
            {
                pointA.Y = MathHelper.Lerp(pointB.Y, pointA.Y, (pointB.X - clippingRectangle.Left) / (pointB.X - pointA.X));
                pointA.X = clippingRectangle.Left;
            }
            if (pointA.X < clippingRectangle.Right && pointB.X > clippingRectangle.Right)
            {
                pointB.Y = MathHelper.Lerp(pointB.Y, pointA.Y, (pointB.X - clippingRectangle.Right) / (pointB.X - pointA.X));
                pointB.X = clippingRectangle.Right;
            }
            if (pointB.X < clippingRectangle.Right && pointA.X > clippingRectangle.Right)
            {
                pointA.Y = MathHelper.Lerp(pointA.Y, pointB.Y, (pointA.X - clippingRectangle.Right) / (pointA.X - pointB.X));
                pointA.X = clippingRectangle.Right;
            }
            if (pointA.Y > clippingRectangle.Top && pointB.Y < clippingRectangle.Top)
            {
                pointB.X = MathHelper.Lerp(pointA.X, pointB.X, (pointA.Y - clippingRectangle.Top) / (pointA.Y - pointB.Y));
                pointB.Y = clippingRectangle.Top;
            }
            if (pointB.Y > clippingRectangle.Top && pointA.Y < clippingRectangle.Top)
            {
                pointA.X = MathHelper.Lerp(pointB.X, pointA.X, (pointB.Y - clippingRectangle.Top) / (pointB.Y - pointA.Y));
                pointA.Y = clippingRectangle.Top;
            }
            if (pointA.Y < clippingRectangle.Bottom && pointB.Y > clippingRectangle.Bottom)
            {
                pointB.X = MathHelper.Lerp(pointB.X, pointA.X, (pointB.Y - clippingRectangle.Bottom) / (pointB.Y - pointA.Y));
                pointB.Y = clippingRectangle.Bottom;
            }
            if (pointB.Y < clippingRectangle.Bottom && pointA.Y > clippingRectangle.Bottom)
            {
                pointA.X = MathHelper.Lerp(pointA.X, pointB.X, (pointA.Y - clippingRectangle.Bottom) / (pointA.Y - pointB.Y));
                pointA.Y = clippingRectangle.Bottom;
            }

            // Perform a final check to see if it cannot be clipped.
            return CheckLine(ref pointA, ref pointB, ref clippingRectangle);
        }

        /// <summary>
        /// Checks the line.
        /// </summary>
        /// <param name="pointA">The point A.</param>
        /// <param name="pointB">The point B.</param>
        /// <param name="clippingRectangle">The clipping rectangle.</param>
        /// <returns>False if the line will be fully clipped. True if it might be partically clipped.</returns>
        private static bool CheckLine(ref Vector2 pointA, ref Vector2 pointB, ref Rectangle clippingRectangle)
        {
            if (pointA.X == pointB.X && pointA.Y == pointB.Y)
            {
                return false;
            }

            if (pointA.X <= clippingRectangle.Left && pointB.X <= clippingRectangle.Left)
            {
                return false;
            }

            if (pointA.X >= clippingRectangle.Right && pointB.X >= clippingRectangle.Right)
            {
                return false;
            }

            if (pointA.Y <= clippingRectangle.Top && pointB.Y <= clippingRectangle.Top)
            {
                return false;
            }

            if (pointA.Y >= clippingRectangle.Bottom && pointB.Y >= clippingRectangle.Bottom)
            {
                return false;
            }

            return true;
        }
    }
}
