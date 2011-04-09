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
using Project290.Screens.Title;
using Microsoft.Xna.Framework.Input;

namespace Project290.Screens
{
    /// <summary>
    /// This instanciates a new Game Over Screen.
    /// </summary>
    public class GameOverScreen : Screen
    {
        /// <summary>
        /// (Real) time that an action can be performed.
        /// </summary>
        private long actionAvailableTime;

        /// <summary>
        /// Where to write "Paused", relative to the background
        /// </summary>
        private Vector2 textDrawPosition;

        /// <summary>
        /// The center of the word "Paused".
        /// </summary>
        private Vector2 textDrawOrigin;

        /// <summary>
        /// Where to draw "score"
        /// </summary>
        private Vector2 scoreLogoPosition;

        /// <summary>
        /// Origin of "score"
        /// </summary>
        private Vector2 scoreLogoOrigin;

        /// <summary>
        /// Position of the actual score.
        /// </summary>
        private Vector2 scorePosition;

        /// <summary>
        /// Origin of the actual score.
        /// </summary>
        private Vector2 scoreOrigin;
        
        /// <summary>
        /// Position of "A"
        /// </summary>
        private Vector2 aButtonPosition;

        /// <summary>
        /// Position of "B"
        /// </summary>
        private Vector2 bButtonPosition;

        /// <summary>
        /// Distance from the button what it does.
        /// </summary>
        private Vector2 actionXOffset;

        /// <summary>
        /// Origin of a button.
        /// </summary>
        private Vector2 buttonOrigin;

        /// <summary>
        /// The position of the background.
        /// </summary>
        private tVector2 position;

        /// <summary>
        /// Origin of A button action text.
        /// </summary>
        private Vector2 aActionOrigin;

        /// <summary>
        /// Origin of B button action text.
        /// </summary>
        private Vector2 bActionOrigin;

        /// <summary>
        /// The color for the buttons and text.
        /// </summary>
        private Color buttonColor;

        /// <summary>
        /// Used for displaying the background.
        /// </summary>
        private HypercubeDisplay background;

        /// <summary>
        /// The score.
        /// </summary>
        private uint score;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOverScreen"/> class.
        /// </summary>
        public GameOverScreen(uint score)
            : base()
        {
            this.score = score;
            this.actionAvailableTime = DateTime.Now.Ticks + 30000000;
            GameClock.Pause();

            this.textDrawPosition = new Vector2(0, -125);
            this.textDrawOrigin = FontStatic.Get("defaultFont").MeasureString("Game Over") / 2f;
            this.scoreLogoPosition = new Vector2(0, -20f);
            this.scoreLogoOrigin = FontStatic.Get("defaultFont").MeasureString("Score") / 2f;
            this.scorePosition = new Vector2(0, 50f);
            this.scoreOrigin = FontStatic.Get("defaultFont").MeasureString(Drawer.FormatNumber(this.score)) / 2f;
            this.aButtonPosition = new Vector2(-240f, 150f);
            this.bButtonPosition = new Vector2(100f, 150f);
            this.actionXOffset = new Vector2(50f, 0);
            this.buttonOrigin = FontStatic.Get("ControllerFont").MeasureString(")") / 2f;
            this.aActionOrigin = new Vector2(0, FontStatic.Get("defaultFont").MeasureString("Replay").Y / 2);
            this.bActionOrigin = new Vector2(0, FontStatic.Get("defaultFont").MeasureString("Quit").Y / 2);
            this.buttonColor = Color.Transparent;

            this.position = new tVector2(1920f / 2f - 2000f, 1080f / 2f);
            this.position.GoTo(1920f / 2f, 1080f / 2f, 0.3f, true);
            this.background = new HypercubeDisplay(
                new Rectangle((int)(this.position.Value.X - 685f / 2f) - 3, (int)(this.position.Value.Y - 370f / 2f) - 3, 685 + 6, 370 + 6),
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
            this.position.Update();

            // If it is fading out and the pause menu is done moving, then dispose.
            if (this.FadingOut && !this.position.IsTransitioning)
            {
                this.Disposed = true;
            }

            this.background.Set(this.position.X.Value, this.position.Y.Value);
            this.background.Update();

            if (DateTime.Now.Ticks > this.actionAvailableTime)
            {
                buttonColor.A = buttonColor.B = buttonColor.G = buttonColor.R
                    = (byte)(MathHelper.Clamp((DateTime.Now.Ticks - this.actionAvailableTime) * (255f / 5000000f), 0, 255));

                if (!this.FadingOut)
                {
                    if (GameWorld.controller.ContainsBool(ActionType.Select))
                    {
                        this.Quit();
                        foreach (Screen screen in GameWorld.screens)
                        {
                            if (screen is GameScreen)
                            {
                                (screen as GameScreen).Reset();
                            }
                        }
                    }
                    else if (GameWorld.controller.ContainsBool(ActionType.GoBack))
                    {
                        this.Quit();
                        foreach (Screen screen in GameWorld.screens)
                        {
                            if (screen is GameScreen)
                            {
                                screen.Disposed = true;
                            }
                        }

                        // Insert the title screen under this screen, so we can see this screen going
                        // off to the right.
                        GameWorld.screens.Insert(GameWorld.screens.Count - 1, new TitleScreen());
                        GameClock.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            // Write "Game Over" at the top-center of the background. And all the other stuff...
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                "Game Over",
                this.position.Value + this.textDrawPosition,
                Color.White,
                0f,
                this.textDrawOrigin,
                1.2f,
                SpriteEffects.None,
                0.99f);
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                "Score",
                this.position.Value + this.scoreLogoPosition,
                Color.White,
                0f,
                this.scoreLogoOrigin,
                0.4f,
                SpriteEffects.None,
                0.99f);
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                Drawer.FormatNumber(this.score),
                this.position.Value + this.scorePosition,
                Color.White,
                0f,
                this.scoreOrigin,
                0.9f,
                SpriteEffects.None,
                0.99f);
            Drawer.DrawControllerSymbol(
                Buttons.A,
                this.position.Value + this.aButtonPosition,
                this.buttonColor,
                0f,
                this.buttonOrigin,
                0.65f,
                SpriteEffects.None,
                0.99f);
            Drawer.DrawControllerSymbol(
                Buttons.B,
                this.position.Value + this.bButtonPosition,
                this.buttonColor,
                0f,
                this.buttonOrigin,
                0.65f,
                SpriteEffects.None,
                0.99f);
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                "Replay",
                this.position.Value + this.aButtonPosition + this.actionXOffset,
                this.buttonColor,
                0f,
                this.aActionOrigin,
                0.65f,
                SpriteEffects.None,
                0.99f);
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                "Exit",
                this.position.Value + this.bButtonPosition + this.actionXOffset,
                this.buttonColor,
                0f,
                this.bActionOrigin,
                0.65f,
                SpriteEffects.None,
                0.99f);

            // Draw the frame
            Drawer.Draw(
                TextureStatic.Get("TitleNameBorder"),
                this.position.Value,
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin("TitleNameBorder"),
                1f,
                SpriteEffects.None,
                0.3f);

            this.background.Draw();
        }
    }
}
