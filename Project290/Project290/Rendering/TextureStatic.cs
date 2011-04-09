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
using Microsoft.Xna.Framework.Graphics;
using Project290.Rendering;
using Project290.GameElements;
using Microsoft.Xna.Framework;

namespace Project290.Rendering
{
    /// <summary>
    /// This is a static public class for storing and receiving static
    /// Texture2Ds from ImageType enums.
    /// </summary>
    public static class TextureStatic
    {
        /// <summary>
        /// The lookup table of images to match with names.
        /// </summary>
        private static Dictionary<string, Texture2D> images = new Dictionary<string,Texture2D>();

        /// <summary>
        /// The origins of each image, for quick reference.
        /// </summary>
        private static Dictionary<string, Vector2> origins = new Dictionary<string, Vector2>();
        
        /// <summary>
        /// Loads a Texture2D, and matches it to the specified image name for easy use later.
        /// </summary>
        public static void Load(string imageName, string directory)
        {
            if (!images.ContainsKey(imageName.ToLower()))
            {
                images.Add(imageName.ToLower(), GameWorld.content.Load<Texture2D>(directory));
                origins.Add(imageName.ToLower(), new Vector2(images[imageName.ToLower()].Width / 2f, images[imageName.ToLower()].Height / 2f));
            }
        }

        /// <summary>
        /// Gets the specified image type, and returns the corresponding Texture2D.
        /// </summary>
        /// <param name="imageType">Type of the image.</param>
        /// <returns>A Texture2D corresponding to the specified image type.</returns>
        public static Texture2D Get(string imageType)
        {
            if (images.ContainsKey(imageType.ToLower()))
            {
                return images[imageType.ToLower()];
            }

            return null;
        }

        /// <summary>
        /// Gets the origin of the image.
        /// </summary>
        /// <param name="imageType">Type of the image.</param>
        /// <returns>The origin of the image.</returns>
        public static Vector2 GetOrigin(string imageType)
        {
            if (origins.ContainsKey(imageType.ToLower()))
            {
                return origins[imageType.ToLower()];
            }

            return Vector2.Zero;
        }
    }
}
