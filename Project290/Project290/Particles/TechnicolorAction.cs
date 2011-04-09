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
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Project290.Particles
{
    /// <summary>
    /// DO NOT CREATE AN INSTANCE OF THIS. USE TechnicolorAction.Instance.
    /// </summary>
    public class TechnicolorAction : ParticleAction
    {
        /// <summary>
        /// Gets the instance. This should be passed to the Particle object.
        /// </summary>
        public static TechnicolorAction Instance { get; private set; }

        /// <summary>
        /// The duration of the particle.
        /// </summary>
        const float Duration = 100000000f;

        /// <summary>
        /// Creates this instance.
        /// </summary>
        public static void Create()
        {
            if (Instance == null)
            {
                Instance = new TechnicolorAction();
            }
        }

        /// <summary>
        /// Updates the specified particle. This should NOT do physics, but everything else. Assume
        /// that the physics is performed before this is called.
        /// </summary>
        /// <param name="particle">The particle.</param>
        public void Update(Particle particle)
        {
            if (particle.CreationTime + Duration < GameClock.Now)
            {
                particle.Dispose();
            }
            else
            {
                if (particle.Position.Y > 1080 || particle.Position.Y < 0)
                {
                    particle.Velocity.Y = -particle.Velocity.Y;
                }

                if (particle.Position.X > 1920 || particle.Position.X < 0)
                {
                    particle.Velocity.X = -particle.Velocity.X;
                }

                float alpha = 0.75f * (1f - Math.Abs(1f - 2f * MathHelper.Lerp(1f, 0f, (GameClock.Now - particle.CreationTime) / Duration)));

                // Why does this work? See: http://en.wikipedia.org/wiki/HSL_and_HSV#Converting_to_RGB
                long offset = particle.CreationTime % (long)Duration;
                float hue = 6f * offset / Duration;
                float x = 1 - Math.Abs(hue % 2 - 1);

                if (hue < 1)
                {
                    particle.color.R = (byte)(255 * alpha);
                    particle.color.G = (byte)(x * 255 * alpha);
                    particle.color.B = 0;
                    particle.color.A = (byte)(255 * alpha);
                }
                else if (hue < 2)
                {
                    particle.color.R = (byte)(x * 255 * alpha);
                    particle.color.G = (byte)(255 * alpha);
                    particle.color.B = 0;
                    particle.color.A = (byte)(255 * alpha);
                }
                else if (hue < 3)
                {
                    particle.color.R = 0;
                    particle.color.G = (byte)(255 * alpha);
                    particle.color.B = (byte)(x * 255 * alpha);
                    particle.color.A = (byte)(255 * alpha);
                }
                else if (hue < 4)
                {
                    particle.color.R = 0;
                    particle.color.G = (byte)(x * 255 * alpha);
                    particle.color.B = (byte)(255 * alpha);
                    particle.color.A = (byte)(255 * alpha);
                }
                else if (hue < 5)
                {
                    particle.color.R = (byte)(x * 255 * alpha);
                    particle.color.G = 0;
                    particle.color.B = (byte)(255 * alpha);
                    particle.color.A = (byte)(255 * alpha);
                }
                else
                {
                    particle.color.R = (byte)(255 * alpha);
                    particle.color.G = 0;
                    particle.color.B = (byte)(x * 255 * alpha);
                    particle.color.A = (byte)(255 * alpha);
                }

                // Finally, set the scale as the inverse of transparency to get a "fade-out" effect.
                // Note that the scale is capped at 3 so that the graphics card does not catch on fire.
                if (alpha != 0)
                {
                    particle.Scale = Math.Min(1f / alpha, 3f);
                }
                else
                {
                    particle.Scale = 3f;
                }
            }
        }

        /// <summary>
        /// Draws the specified particle.
        /// </summary>
        /// <param name="particle">The particle.</param>
        public void Draw(Particle particle)
        {
            Drawer.Draw(
                TextureStatic.Get("particle3"),
                particle.Position,
                null,
                particle.color,
                particle.Rotation,
                TextureStatic.GetOrigin("particle3"),
                particle.Scale,
                SpriteEffects.None,
                0.11f + 0.088f * (1f - (GameClock.Now - particle.CreationTime) / Duration));
        }
    }
}
