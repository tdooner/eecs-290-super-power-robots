using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Screens;
using Microsoft.Xna.Framework;
using Project290.Rendering;
using Project290.GameElements;
using Project290.Inputs;
using Microsoft.Xna.Framework.Graphics;
using Project290.Clock;

namespace Project290.Games.StupidGame
{
    /// <summary>
    /// The screen for the stupid game. This is where all the action happens.
    /// </summary>
    public class StupidGameScreen : GameScreen
    {
        /// <summary>
        /// The message on the screen.
        /// </summary>
        private string display;

        /// <summary>
        /// Where to display display :p
        /// </summary>
        private Vector2 displayPosition;

        /// <summary>
        /// The origin of display
        /// </summary>
        private Vector2 displayOrigin;

        /// <summary>
        /// Where to draw the score.
        /// </summary>
        private Vector2 scorePosition;

        /// <summary>
        /// Where to draw the count-down timer.
        /// </summary>
        private Vector2 countDownTimerPosition;

        /// <summary>
        /// The time (in ticks since the start of the game) to end it.
        /// </summary>
        private long gameOverTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="StupidGameScreen"/> class.
        /// </summary>
        /// <param name="scoreboardIndex">The game-specific index into the scoreboard.</param>
        public StupidGameScreen(int scoreboardIndex)
            : base(scoreboardIndex)
        {
            // Here should be the construction of all objects and the setting of objects that do not change.
            // The Reset method should be used to set objects that do change.
            this.display = "Mash A. Isn't this a fun game?";
            this.displayPosition = new Vector2(1920 / 2, 300); // Remember, screen resolution is guarenteed to be 1920*1080
            this.displayOrigin = FontStatic.Get("defaultFont").MeasureString(this.display) / 2f;
            this.scorePosition = new Vector2(1920 / 2, 500);
            this.countDownTimerPosition = new Vector2(1920 / 2, 700);

            // Always call reset at the end of the constructor!
            this.Reset();
        }

        /// <summary>
        /// Resets this instance. This will be called when a new game is played. Reset all of
        /// your objects here so that you do not need to reallocate the memory for them.
        /// </summary>
        internal override void Reset()
        {
            base.Reset();
            
            this.gameOverTime = GameClock.Now + 10000000 * 10; // 10 Seconds.
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Check to see if it's game over.
            if (GameClock.Now > this.gameOverTime)
            {
                this.GameOver();
                return;
            }

            if (GameWorld.controller.ContainsBool(ActionType.AButtonFirst))
            {
                this.Score += 917;
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
                this.display,
                this.displayPosition,
                Color.White,
                0f,
                this.displayOrigin,
                1f,
                SpriteEffects.None,
                1f);

            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                Drawer.FormatNumber(this.Score),
                this.scorePosition,
                Color.White,
                0f,
                FontStatic.Get("defaultFont").MeasureString(this.Score.ToString()) / 2f,
                1f,
                SpriteEffects.None,
                1f);

            string secondsRemaining = Math.Max((Math.Ceiling((this.gameOverTime - GameClock.Now) / 10000000f)), 0).ToString();
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                secondsRemaining,
                this.countDownTimerPosition,
                Color.White,
                0f,
                FontStatic.Get("defaultFont").MeasureString(secondsRemaining) / 2f,
                1f,
                SpriteEffects.None,
                1f);
        }
    }
}
