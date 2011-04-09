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
using Microsoft.Xna.Framework.Graphics;
using Project290.GameElements;
using Project290.Inputs;
using Project290.Screens.Title;
using Microsoft.Xna.Framework.Input;

namespace Project290.Screens
{
    /// <summary>
    /// The Screen the comes up when the game first starts up.
    /// </summary>
    public class StartScreen : Screen
    {
        /// <summary>
        /// The location where press start is drawn.
        /// </summary>
        private Vector2 pressStartLocation;

        /// <summary>
        /// The origin of "Press Start"
        /// </summary>
        private Vector2 pressStartOrigin;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartScreen"/> class.
        /// </summary>
        public StartScreen()
            : base()
        {
            this.pressStartLocation = new Vector2(1920 / 2, 1080 / 2);
            this.pressStartOrigin = FontStatic.Get("defaultFont").MeasureString("Press Start") / 2f;
            this.FadingOut = true;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // In this case, can't use the Controller class because it is not yet mapped to a PlayerIndex.
            for (PlayerIndex playerIndex = PlayerIndex.One; playerIndex <= PlayerIndex.Four; playerIndex++)
            {
                if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.Start) || Keyboard.GetState(playerIndex).IsKeyDown(Keys.Enter))
                {
                    GameWorld.controller.SetPlayerIndex(playerIndex);
                    this.Disposed = true;
                    GameWorld.screens.Play(new TitleScreen());
                    GameWorld.audio.PlaySound("menuClick");
                    break;
                }
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                "Press Start",
                this.pressStartLocation,
                Color.White,
                0f,
                this.pressStartOrigin,
                1f,
                SpriteEffects.None,
                0.5f);
        }
    }
}
