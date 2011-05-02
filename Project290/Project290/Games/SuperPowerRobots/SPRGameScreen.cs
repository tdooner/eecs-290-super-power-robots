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
    public class SPRGameScreen : GameScreen
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

        private int m_scoreboardIndex;
        private bool addedEndScreen = false;
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
            this.m_scoreboardIndex = scoreboardIndex;

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

        public void NextLevel()
        {
            currentLevel += 1;
            fantastica = new World(Vector2.Zero);
            Physics.Settings.MaxPolygonVertices = 30; // Defaults to 8? What are we, running on a TI-83 or something?
            Physics.Settings.EnableDiagnostics = false;
            addedEndScreen = false;
            this.sprWorld = new SPRWorld(fantastica, currentLevel);
        }

        /// <summary>
        /// Resets this instance. This will be called when a new game is played. Reset all of
        /// your objects here so that you do not need to reallocate the memory for them.
        /// </summary>
        internal override void Reset()
        {
            base.Reset();
            currentLevel = 0;
            scoreKeeper = new ScoreKeeper(true);
            previousGameTime = GameClock.Now;
            fantastica = new World(Vector2.Zero);
            Physics.Settings.MaxPolygonVertices = 30; // Defaults to 8? What are we, running on a TI-83 or something?
            Physics.Settings.EnableDiagnostics = false;
            Battle.nextAllies = "";
            this.sprWorld = new SPRWorld(fantastica, currentLevel);

            this.gameOverTime = GameClock.Now + 10000000 * 10; // 10 Seconds.
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (this.sprWorld.m_hasLost && addedEndScreen == false)
            {
                GameWorld.screens.Play(new GameOverScreen((uint)ScoreKeeper.score));
                addedEndScreen = true;
            }
            if (this.sprWorld.m_isGameOver == true && addedEndScreen == false)
            {
                if (currentLevel > 5)
                {
                    GameWorld.screens.Play(new SPRWinScreen());
                }
                else
                {
                    GameWorld.screens.Play(new LoadOutScreen(m_scoreboardIndex, this));
                }
                addedEndScreen = true;
            }
            if (addedEndScreen)
                return;
            this.sprWorld.Update((GameClock.Now - previousGameTime) / 10000000f);
            
            fantastica.Step((GameClock.Now - previousGameTime) / 10000000f);

            previousGameTime = GameClock.Now;
            
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
