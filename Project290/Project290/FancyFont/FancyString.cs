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

namespace Project290.FancyFont
{
    /// <summary>
    /// A list of fancy characters.
    /// </summary>
    public class FancyString : List<FancyCharacter>
    {
        /// <summary>
        /// The characters that are exploading.
        /// </summary>
        private List<FancyCharacter> exploadingChars;

        /// <summary>
        /// Initializes a new instance of the <see cref="FancyString"/> class.
        /// </summary>
        public FancyString()
        {
            this.exploadingChars = new List<FancyCharacter>();
        }

        /// <summary>
        /// Recreates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="position">The position of the first character.</param>
        /// <param name="exploadPosition">The expload position.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="color">The color.</param>
        /// <param name="fontName">Name of the font.</param>
        /// <param name="layerDepth">The layer depth.</param>
        /// <param name="centered">If true, the text will be centered at position. If false, position will be the top-left of the text.</param>
        public void Create(string text, Vector2 position, Vector2 exploadPosition, float scale, Color color, string fontName, float layerDepth, bool centered)
        {
            foreach (FancyCharacter character in this)
            {
                this.exploadingChars.Add(character);
                character.Expload(exploadPosition);
            }

            this.Clear();
            if (text.Length == 0)
            {
                return;
            }

            position += scale * FontStatic.Get(fontName).MeasureString(text[0].ToString()) / 2f; 
            if (centered)
            {
                position -= scale * FontStatic.Get(fontName).MeasureString(text) / 2f;
            }

            float initialX = position.X;
            long timeOffset = 10000000;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != '\n')
                {
                    if (text[i] == '#')
                    {
                        i++;
                        this.Add(new FancyCharacter(Drawer.GetControllerSymbolMap(text[i]), position, scale * 1.2f, color, "controllerFont", timeOffset, layerDepth));
                        position.X += 0.5f * FontStatic.Get("controllerFont").MeasureString(Drawer.GetControllerSymbolMap(text[i]).ToString()).X * scale;
                    }
                    else
                    {
                        this.Add(new FancyCharacter(text[i], position, scale, color, fontName, timeOffset, layerDepth));
                        position.X += 0.5f * FontStatic.Get(fontName).MeasureString(text[i].ToString()).X * scale;
                    }
                }

                if (i < text.Length - 1 && text[i + 1] != '\n')
                {
                    position.X += 0.5f * FontStatic.Get(fontName).MeasureString(text[i + 1].ToString()).X * scale;
                }

                if (text[i] == '\n')
                {
                    position.X = initialX;
                    position.Y += FontStatic.Get(fontName).MeasureString(" ").Y * scale * 0.75f;
                    timeOffset = 10000000;
                }

                timeOffset += 200000;
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            foreach (FancyCharacter character in this)
            {
                character.Update();
            }

            for (int i = this.exploadingChars.Count - 1; i >= 0; i--)
            {
                this.exploadingChars[i].Update();

                if (this.exploadingChars[i].Disposed)
                {
                    this.exploadingChars.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            foreach (FancyCharacter character in this)
            {
                character.Draw();
            }

            foreach (FancyCharacter character in this.exploadingChars)
            {
                character.Draw();
            }
        }
    }
}
