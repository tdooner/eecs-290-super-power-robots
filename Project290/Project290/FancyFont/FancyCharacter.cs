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
using Project290.Rendering;
using Project290.Clock;

namespace Project290.FancyFont
{
    /// <summary>
    /// A fancy character that can ExPlOaD.
    /// </summary>
    public class FancyCharacter
    {
        /// <summary>
        /// The character to display.
        /// </summary>
        public char character;

        /// <summary>
        /// The position of the character.
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// Is this disposed and should this be removed from existance?
        /// </summary>
        public bool Disposed;

        /// <summary>
        /// THe origin of the character.
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// The font name.
        /// </summary>
        private string font;

        /// <summary>
        /// The scale of the character to draw.
        /// </summary>
        private float scale;

        /// <summary>
        /// The color.
        /// </summary>
        private Color color;

        /// <summary>
        /// The layer depth
        /// </summary>
        private float layerDepth;

        /// <summary>
        /// The velocity of this character.
        /// </summary>
        private Vector2 velocity;

        /// <summary>
        /// The applied acceloration of this character.
        /// </summary>
        private Vector2 acceloration;

        /// <summary>
        /// The rotational velocity to apply.
        /// </summary>
        private float rotationalVelocity;

        /// <summary>
        /// The current rotation.
        /// </summary>
        private float rotation;

        /// <summary>
        /// The creating time (game clock).
        /// </summary>
        private long creationTime;

        /// <summary>
        /// When to start the startup effect.
        /// </summary>
        private long startupEffectTime;

        /// <summary>
        /// A temp var.
        /// </summary>
        private Color tempColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FancyCharacter"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="position">The position.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="color">The color.</param>
        /// <param name="fontName">Name of the font.</param>
        /// <param name="startupEffectTimeOffset">The startup effect time offset.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public FancyCharacter(char character, Vector2 position, float scale, Color color, string fontName, long startupEffectTimeOffset, float layerDepth)
        {
            this.character = character;
            this.position = position;
            this.font = fontName;
            this.origin = FontStatic.Get(fontName).MeasureString(character.ToString()) / 2f;
            this.scale = scale;
            this.color = color;
            this.layerDepth = layerDepth;
            this.Disposed = false;
            this.velocity = Vector2.Zero;
            this.acceloration = Vector2.Zero;
            this.rotationalVelocity = 0;
            this.rotation = 0;
            this.creationTime = GameClock.Now;
            this.startupEffectTime = this.creationTime + startupEffectTimeOffset;
            this.tempColor = new Color();
        }

        /// <summary>
        /// Exploads the character from the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public void Expload(Vector2 location)
        {
            this.layerDepth += 0.001f;
            this.acceloration = new Vector2(0, 1);

            // Get the difference vector between the current position and the exploading point
            Vector2 diff = this.position - location;

            // Use this to apply an initial impulse and rotational speed.
            float temp = diff.Length() * 0.02f;
            this.velocity = diff / (temp * temp);
            this.rotationalVelocity = diff.X * 0.001f;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            // Set this to be disposed if it is outside the screen rectangle (plus a buffer of 500). Don't worry about the top.
            if (this.position.X < -500 || this.position.X > 2420 || this.position.Y > 1580)
            {
                this.Disposed = true;
                return;
            }

            this.position += this.velocity;
            this.velocity += this.acceloration;
            this.rotation += this.rotationalVelocity;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            float alpha = 1f;
            if (GameClock.Now - this.creationTime < 3000000f)
            {
                alpha = MathHelper.Clamp((GameClock.Now - this.creationTime) / 3000000f, 0, 1);
            }

            tempColor.A = (byte)(this.color.A * alpha);
            tempColor.B = (byte)(this.color.B * alpha);
            tempColor.G = (byte)(this.color.G * alpha);
            tempColor.R = (byte)(this.color.R * alpha);

            float tempScale = this.scale;
            if (GameClock.Now >= this.startupEffectTime - 2500000 && GameClock.Now <= this.startupEffectTime + 2500000)
            {
                tempScale *= Math.Abs((GameClock.Now - this.startupEffectTime) / 2500000f) * 0.4f + 0.6f;
            }

            if (this.font.ToLower().Equals("controllerfont"))
            {
                Drawer.DrawString(
                    FontStatic.Get(this.font),
                    this.character.ToString(),
                    this.position,
                    tempColor,
                    this.rotation,
                    this.origin,
                    tempScale,
                    SpriteEffects.None,
                    this.layerDepth);
            }
            else
            {
                Drawer.DrawOutlinedString(
                    FontStatic.Get(this.font),
                    this.character.ToString(),
                    this.position,
                    tempColor,
                    this.rotation,
                    this.origin,
                    tempScale,
                    SpriteEffects.None,
                    this.layerDepth);
            }
        }
    }
}
