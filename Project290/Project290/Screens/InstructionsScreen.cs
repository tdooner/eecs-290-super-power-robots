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
using Project290.GameElements;
using Project290.Inputs;
using Microsoft.Xna.Framework.Graphics;

namespace Project290.Screens
{
    /// <summary>
    /// Used to display game instructions.
    /// </summary>
    public class InstructionsScreen : Screen
    {
        /// <summary>
        /// The name of the image to draw.
        /// </summary>
        private string imageName;

        /// <summary>
        /// The position of the image.
        /// </summary>
        private tVector2 position;

        /// <summary>
        /// Should this be disposed whe the transition stops?
        /// </summary>
        private bool shouldDispose;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionsScreen"/> class.
        /// </summary>
        /// <param name="imageName">Name of the image containing the instructions.</param>
        public InstructionsScreen(string imageName)
            : base()
        {
            this.imageName = imageName;
            this.position = new tVector2(1920f / 2f - 2000f, 1080f / 2f);
            this.position.GoTo(1920f / 2f, 1080f / 2f, 0.3f);
            this.shouldDispose = false;
            this.FadingOut = true;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (this.shouldDispose && !this.position.IsTransitioning)
            {
                this.Disposed = true;
            }

            if (GameWorld.controller.ContainsBool(ActionType.GoBack))
            {
                if (!this.shouldDispose)
                {
                    this.position.GoTo(1920f / 2f + 2000f, 1080f / 2f, 0.3f);
                    this.shouldDispose = true;
                    GameWorld.audio.PlaySound("menuGoBack");
                }
            }

            this.position.Update();
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            Drawer.Draw(
                TextureStatic.Get("instructionBorder"),
                this.position.Value,
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin("instructionBorder"),
                1f,
                SpriteEffects.None,
                0.4f);
            Drawer.Draw(
                TextureStatic.Get(this.imageName),
                this.position.Value,
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin(this.imageName),
                1f,
                SpriteEffects.None,
                0.5f);
        }
    }
}
