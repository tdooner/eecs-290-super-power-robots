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
using Project290.Inputs;
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Project290.Storage;
using Project290.Particles;
using Project290.Clock;

namespace Project290.Screens.Title
{
    /// <summary>
    /// The title screen.
    /// </summary>
    public class TitleScreen : Screen
    {
        /// <summary>
        /// For displaying the box arts.
        /// </summary>
        private BoxArtDisplay boxArtDisplay;

        /// <summary>
        /// Display for the game info.
        /// </summary>
        private GameInfoDisplay gameInfoDisplay;

        /// <summary>
        /// Where to draw the scoreboard.
        /// </summary>
        private ScoreboardDisplay ScoreBoardDisplay;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleScreen"/> class.
        /// </summary>
        public TitleScreen()
            : base()
        {
            GameClock.Reset();
            this.boxArtDisplay = new BoxArtDisplay(this, new Vector2(528, 590), new Vector2(-50, -100));
            this.gameInfoDisplay = new GameInfoDisplay(this, this.boxArtDisplay.boxArtMenu, new Vector2(1342, 195), 668, new Vector2(1013, 240), new Vector2(1342, 325));
            this.ScoreBoardDisplay = new ScoreboardDisplay(this, new Vector2(180, 10), new Vector2(1342, 755));
            GameInfoCollection.ShiftIndex(-GameInfoCollection.CurrentIndex);
            this.FadingOut = true;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Check if user is selecting "up" or "down". Only do this if this is on the top of the screen stack...
            if (this.IsOnTop())
            {
                if (GameWorld.controller.ContainsBool(ActionType.SelectionUp) 
                    || GameWorld.controller.ContainsBool(ActionType.SelectionLeft) && GameInfoCollection.CurrentIndex != GameInfoCollection.GameInfos.Count - 1)
                {
                    if (!GameWorld.controller.ContainsBool(ActionType.DPadUp) && !GameWorld.controller.ContainsBool(ActionType.DPadDown))
                    {
                        this.boxArtDisplay.Shift(-1);
                    }
                }
                if (GameWorld.controller.ContainsBool(ActionType.SelectionDown)
                    || GameWorld.controller.ContainsBool(ActionType.SelectionRight) && GameInfoCollection.CurrentIndex != GameInfoCollection.GameInfos.Count - 1)
                {
                    if (!GameWorld.controller.ContainsBool(ActionType.DPadUp) && !GameWorld.controller.ContainsBool(ActionType.DPadDown))
                    {
                        this.boxArtDisplay.Shift(1);
                    }
                }

                if (GameInfoCollection.CurrentIndex != GameInfoCollection.GameInfos.Count - 1
                    && GameWorld.controller.ContainsBool(ActionType.Select))
                {
                    this.Disposed = true;
                    GameInfoCollection.LauchGame();
                    GameWorld.audio.PlaySound("menuClick");
                }

                if (GameWorld.controller.ContainsBool(ActionType.GoBack))
                {
                    GameInfoCollection.ShiftIndex(-GameInfoCollection.CurrentIndex);
                    this.Disposed = true;
                    GameWorld.screens.Play(new StartScreen());
                    GameWorld.audio.PlaySound("menuGoBack");
                    return;
                }

                if (GameInfoCollection.CurrentIndex != GameInfoCollection.GameInfos.Count - 1
                    && GameWorld.controller.ContainsBool(ActionType.XButton))
                {
                    // Only pop up an instructions screen if there is no instructions screen.
                    bool found = false;
                    foreach (Screen screen in GameWorld.screens)
                    {
                        if (screen is InstructionsScreen)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        GameWorld.screens.Play(new InstructionsScreen(GameInfoCollection.GameInfos[GameInfoCollection.CurrentIndex].InstructionsImageName));
                        GameWorld.audio.PlaySound("menuClick");
                    }
                }
            }
            
            this.boxArtDisplay.Update();
            this.gameInfoDisplay.Update();
            this.ScoreBoardDisplay.Update();
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            this.boxArtDisplay.Draw();
            this.ScoreBoardDisplay.Draw();
            this.gameInfoDisplay.Draw();
        }
    }
}
