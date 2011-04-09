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
using Project290.Mathematics;
using Project290.GameElements;
using Project290.Inputs;
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Project290.Menus
{
    /// <summary>
    /// Used for both controlling volume and displaying the volume.
    /// </summary>
    public class VolumeControlDisplayEntry : DescriptionMenuEntry
    {
        /// <summary>
        /// The center of drawing.
        /// </summary>
        private Vector2 displayPosition;

        /// <summary>
        /// The scale of the largest number.
        /// </summary>
        private float scale;

        /// <summary>
        /// Layer depth for drawing numbers.
        /// </summary>
        private float numberLayerDepth;

        /// <summary>
        /// Is this for music volume? Else, sound volume.
        /// </summary>
        private bool musicVolume;

        /// <summary>
        /// The positions of all number values in [0, 10]
        /// </summary>
        private List<tVector2> positions;

        /// <summary>
        /// The scales of all number values in [0, 10]
        /// </summary>
        private List<tfloat> scales;

        /// <summary>
        /// The number origins.
        /// </summary>
        private List<Vector2> origins;

        /// <summary>
        /// Was this highlighted on last update?
        /// </summary>
        private bool previouslyHighlighed;

        /// <summary>
        /// Ignores the offset position when drawing.
        /// </summary>
        private bool ignoreOffsetPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeControlDisplayEntry"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="description">The description.</param>
        /// <param name="actions">The actions.</param>
        /// <param name="position">The position.</param>
        /// <param name="numberDisplayPosition">The number display position.</param>
        /// <param name="scale">The scale of the largest number.</param>
        /// <param name="numberLayerDepth">The number layer depth.</param>
        /// <param name="musicVolume">Is this for music volume? Else, sound volume.</param>
        /// <param name="ignoreOffsetPosition">if set to <c>true</c> [ignore offset position].</param>
        public VolumeControlDisplayEntry(
            string text,
            string description,
            MenuAction[] actions,
            Vector2 position,
            Vector2 numberDisplayPosition,
            float scale,
            float numberLayerDepth,
            bool musicVolume,
            bool ignoreOffsetPosition)
            : base(text, description, actions, position)
        {
            this.displayPosition = numberDisplayPosition;
            this.scale = scale;
            this.numberLayerDepth = numberLayerDepth;
            this.musicVolume = musicVolume;
            this.ignoreOffsetPosition = ignoreOffsetPosition;
            this.previouslyHighlighed = false;
            this.positions = new List<tVector2>();
            this.scales = new List<tfloat>();
            for (int i = 0; i <= 10; i++)
            {
                this.positions.Add(new tVector2(numberDisplayPosition.X, numberDisplayPosition.Y));
                this.scales.Add(new tfloat(0));
            }

            this.origins = new List<Vector2>();
            for (int i = 0; i <= 10; i++)
            {
                this.origins.Add(FontStatic.Get("defaultFont").MeasureString(i.ToString()) / 2f);
            }
        }

        /// <summary>
        /// Updates the specified highlighted.
        /// </summary>
        /// <param name="highlighted">Specifies whether or not this menu entry
        /// is the highlighted menu entry in the list of menu entries.</param>
        public override void Update(bool highlighted)
        {
            base.Update(highlighted);

            if (highlighted)
            {
                if (!this.previouslyHighlighed)
                {
                    this.previouslyHighlighed = true;
                    this.UpdateTValues();
                }

                if (GameWorld.controller.ContainsBool(ActionType.SelectionRight))
                {
                    this.ChangeVolume(1);
                    this.UpdateTValues();
                }
                if (GameWorld.controller.ContainsBool(ActionType.SelectionLeft))
                {
                    this.ChangeVolume(-1);
                    this.UpdateTValues();
                }
            }
            else
            {
                if (previouslyHighlighed)
                {
                    this.previouslyHighlighed = false;
                    this.Disappear();
                }
            }

            foreach (tVector2 tv2 in this.positions)
            {
                tv2.Update();
            }

            foreach (tfloat tf in this.scales)
            {
                tf.Update();
            }
        }

        /// <summary>
        /// Updates the T values (positions and scales) based on volume.
        /// </summary>
        private void UpdateTValues()
        {
            int currentVolume = this.GetVolume();

            this.positions[currentVolume].GoTo(this.displayPosition.X, this.displayPosition.Y, 0.2f, true);
            this.scales[currentVolume].GoTo(this.scale, 0.2f, true);
            this.positions[(currentVolume - 1 + 11) % 11].GoTo(this.displayPosition.X - 70f * this.scale, this.displayPosition.Y, 0.2f, true);
            this.scales[(currentVolume - 1 + 11) % 11].GoTo(this.scale * 0.5f, 0.2f, true);
            this.positions[(currentVolume + 1) % 11].GoTo(this.displayPosition.X + 70f * this.scale, this.displayPosition.Y, 0.2f, true);
            this.scales[(currentVolume + 1) % 11].GoTo(this.scale * 0.5f, 0.2f, true);
            this.positions[(currentVolume - 2 + 11) % 11].GoTo(this.displayPosition.X - 85f * this.scale, this.displayPosition.Y, 0.2f, true);
            this.scales[(currentVolume - 2 + 11) % 11].GoTo(0f, 0.2f, true);
            this.positions[(currentVolume + 2) % 11].GoTo(this.displayPosition.X + 85f * this.scale, this.displayPosition.Y, 0.2f, true);
            this.scales[(currentVolume + 2) % 11].GoTo(0f, 0.2f, true);

            // "clean up" all others...
            for (int i = 0; i <= 10; i++)
            {
                if (i != currentVolume
                    & i != (currentVolume - 1 + 11) % 11
                    & i != (currentVolume - 2 + 11) % 11
                    & i != (currentVolume + 1 + 11) % 11
                    & i != (currentVolume + 2 + 11) % 11)
                {
                    this.positions[i].Set(this.displayPosition.X, this.displayPosition.Y);
                    this.scales[i].SetValue(0f);
                }                    
            }
        }

        /// <summary>
        /// Has all scales go to 0.
        /// </summary>
        private void Disappear()
        {
            foreach (tfloat tf in this.scales)
            {
                tf.GoTo(0, 0.1f, true);
            }
        }

        /// <summary>
        /// Gets the volume in the range [0, 10].
        /// </summary>
        /// <returns>The volume in the range [0, 10].</returns>
        private int GetVolume()
        {
            if (this.musicVolume)
            {
                return (int)MathHelper.Clamp((float)Math.Round(GameWorld.audio.MusicVolume * 10f), 0, 10);
            }
            else
            {
                return (int)MathHelper.Clamp((float)Math.Round(GameWorld.audio.SoundVolume * 10f), 0, 10);
            }
        }

        /// <summary>
        /// Changes the volume by the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        private void ChangeVolume(int amount)
        {
            if (this.musicVolume)
            {
                GameWorld.audio.MusicVolume = ((this.GetVolume() + 11 + amount) % 11) / 10f;
            }
            else
            {
                GameWorld.audio.SoundVolume = ((this.GetVolume() + 11 + amount) % 11) / 10f;
            }

            if (amount > 0)
            {
                GameWorld.audio.PlaySound("volumeControlUp");
            }
            else if (amount < 0)
            {
                GameWorld.audio.PlaySound("volumeControlDown");
            }
        }

        /// <summary>
        /// Draws the menu entry at the specified position.
        /// </summary>
        /// <param name="offsetPosition">The offset position.</param>
        /// <param name="highlighted">Specifies whether or not this menu entry
        /// is the highlighted menu entry in the list of menu entries.</param>
        public override void Draw(Vector2 offsetPosition, bool highlighted)
        {
            base.Draw(offsetPosition, highlighted);

            for (int i = 0; i <= 10; i++)
            {
                if (this.scales[i].Value != 0)
                {
                    Drawer.DrawOutlinedString(
                        FontStatic.Get("defaultFont"),
                        i.ToString(),
                        this.positions[i].Value + (this.ignoreOffsetPosition ? Vector2.Zero : offsetPosition),
                        Color.White,
                        0f,
                        this.origins[i],
                        this.scales[i].Value,
                        SpriteEffects.None,
                        this.numberLayerDepth);
                }
            }
        }
    }
}