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
using Project290.GameElements;
using Microsoft.Xna.Framework;
using Project290.Menus;
using Project290.Clock;
using Project290.Menus.MenuDelegates;
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Project290.Inputs;
using Project290.Mathematics;
using Project290.Screens.Shared;

namespace Project290.Screens
{
    /// <summary>
    /// This instanciates a new Pause Screen.
    /// </summary>
    public class PauseScreen : Screen
    {
        /// <summary>
        /// This is the menu used for the pause screen.
        /// </summary>
        private PauseMenu menu;

        /// <summary>
        /// Where to write "Paused", relative to the background
        /// </summary>
        private Vector2 textDrawPosition;

        /// <summary>
        /// The center of the word "Paused".
        /// </summary>
        private Vector2 textDrawOrigin;

        /// <summary>
        /// The position of the background.
        /// </summary>
        private tVector2 position;

        /// <summary>
        /// Used for displaying the background.
        /// </summary>
        private HypercubeDisplay background;

        /// <summary>
        /// Lag the draw time by a split second to prevent stuff from being drawn where it shouldn't...
        /// </summary>
        private long drawLagTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="PauseScreen"/> class.
        /// </summary>
        public PauseScreen()
            : base()
        {
            this.menu = new PauseMenu(
                new Vector2(0, -60),
                new MenuAction[]
                { 
                    new MenuAction(ActionType.GoBack, new QuitPauseScreenDelegate()),
                },
                75);

            this.textDrawPosition = new Vector2(0, -225);
            this.textDrawOrigin = FontStatic.Get("defaultFont").MeasureString("Paused") / 2f;
            this.position = new tVector2(1920f / 2f - 2000f, 1080f / 2f);
            this.position.GoTo(1920f / 2f, 1080f / 2f, 0.3f, true);
            this.drawLagTime = DateTime.Now.Ticks + 1000000;
            this.background = new HypercubeDisplay(
                new Rectangle((int)(this.position.Value.X - 584f / 2f) - 3, (int)(this.position.Value.Y - 700f / 2f) - 3, 584 + 6, 700 + 6),
                3,
                this.random,
                0.2f);
        }

        /// <summary>
        /// Quits this instance.
        /// </summary>
        public void Quit()
        {
            if (!this.FadingOut)
            {
                this.FadingOut = true;
                this.position.GoTo(1920f / 2f + 2000f, 1080f / 2f, 0.3f, true);
                GameWorld.audio.PlaySound("menuGoBack");
            }
        }

        /// <summary>
        /// Updates this instance. This makes sure that GameClock is paused,
        /// and it also updates the menu.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Only pause the gameclock if the screen is not fading out.
            if (!this.FadingOut)
            {
                GameClock.Pause();
            }
            else
            {
                GameClock.Unpause();
            }

            this.position.Update();

            // If it is fading out and the pause menu is done moving, then dispose.
            if (this.FadingOut && !this.position.IsTransitioning)
            {
                GameClock.Unpause();
                this.Disposed = true;
            }

            this.menu.position = this.position.Value;
            this.menu.Update();
            this.background.Set(this.position.X.Value, this.position.Y.Value);
            this.background.Update();
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            // Write "Paused" at the top-center of the background.
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                "Paused",
                this.position.Value + this.textDrawPosition,
                Color.White,
                0f,
                this.textDrawOrigin,
                1.2f,
                SpriteEffects.None,
                0.99f);

            // Draw the frame
            Drawer.Draw(
                TextureStatic.Get("BoxArtHolder"),
                this.position.Value,
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin("BoxArtHolder"),
                1f,
                SpriteEffects.None,
                0.3f);

            if (DateTime.Now.Ticks > this.drawLagTime)
            {
                this.menu.Draw();
            }

            this.background.Draw();
        }
    }
}
