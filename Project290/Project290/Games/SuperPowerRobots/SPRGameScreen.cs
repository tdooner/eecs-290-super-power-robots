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
using Project290.Physics.Dynamics;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Factories;
using Project290.Games.SuperPowerRobots.Entities; 

namespace Project290.Games.SuperPowerRobots
{
    class SPRGameScreen : GameScreen
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
        private long gameOverTime, previousGameTime;

        private SPRWorld sprWorld;

        private int currentLevel;

        public ScoreKeeper scoreKeeper;
        
        // DEBUG!!
        Body wall_e;
        World fantastica;
        // </DEBUG!!>

        /// <summary>
        /// Initializes a new instance of the <see cref="StupidGameScreen"/> class.
        /// </summary>
        /// <param name="scoreboardIndex">The game-specific index into the scoreboard.</param>
        public SPRGameScreen(int scoreboardIndex)
            : base(scoreboardIndex)
        {
            // Tom's messing around with the physics engine!

            currentLevel = 0;
            scoreKeeper = new ScoreKeeper(true);

            previousGameTime = GameClock.Now;
            fantastica = new World(Vector2.Zero);
            Physics.Settings.MaxPolygonVertices = 30; // Defaults to 8? What are we, running on a TI-83 or something?
            Physics.Settings.EnableDiagnostics = false;
            this.sprWorld = new SPRWorld(fantastica, currentLevel);

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

            this.sprWorld.Update((GameClock.Now - previousGameTime) / 10000000f);
            
            fantastica.Step((GameClock.Now - previousGameTime) / 10000000f);

			previousGameTime = GameClock.Now;

            if (this.sprWorld.m_isGameOver == true)
            {
                ScoreKeeper.AddMoney(this.sprWorld.WinReward());
                // Any type of stuff to be done after the bout is over shall go here.
                // Wooo!
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            this.sprWorld.Draw();
        }
    }
}
