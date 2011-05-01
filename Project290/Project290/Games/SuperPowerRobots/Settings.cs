using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project290.Games.SuperPowerRobots
{
    class Settings
    {
        /// <summary>
        /// Number of pixels per meter. (Currently 100 pixels = 1 meter)
        /// </summary>
        public static float PixelsPerMeter = 100f;

        /// <summary>
        /// Number of Meters per Pixel. (Currently 1/100 meter = 1 pixel)
        /// </summary>
        public static float MetersPerPixel = 1 / PixelsPerMeter;
    }
}
