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
using Project290.Storage;
using Project290.GameElements;
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Project290.Inputs;
using Project290.Screens.Shared;

namespace Project290.Screens.Title
{
    /// <summary>
    /// The rectangle box for displaying the scoreboard.
    /// </summary>
    public class ScoreboardDisplay
    {
        /// <summary>
        /// Used for displaying the background.
        /// </summary>
        private HypercubeDisplay background;

        /// <summary>
        /// Where the text gets drawn.
        /// </summary>
        private Vector2 textDrawLocation;

        /// <summary>
        /// Where to draw the sideways "High Scores"
        /// </summary>
        private Vector2 highScoresLogoLocation;

        /// <summary>
        /// Origin of the sideways "High Scores".
        /// </summary>
        private Vector2 highScoresLogoOrigin;

        /// <summary>
        /// Where to draw the outline.
        /// </summary>
        private Vector2 rectangleDrawLocation;

        /// <summary>
        /// Where to draw the Gamertags.
        /// </summary>
        private Vector2 nameDrawLocation;

        /// <summary>
        /// Where to draw the Scores.
        /// </summary>
        private Vector2 scoreDrawLocation;

        /// <summary>
        /// How much spacing vertically between scores?
        /// </summary>
        private float verticleSpacing;

        /// <summary>
        /// The region that cuts off the scrolling scores.
        /// </summary>
        private Viewport scrollableRectangle;

        /// <summary>
        /// The offset to apply to the score display.
        /// </summary>
        private Vector2 scoreOffset;

        /// <summary>
        /// How fast is the score offset moving?
        /// </summary>
        private float scoreOffsetVelocity;

        /// <summary>
        /// How fast is the score offset velocity moving?
        /// </summary>
        private float scoreOffsetAcceloration;

        /// <summary>
        /// The previously highlighted box art.
        /// </summary>
        private int previousHighlighted;

        /// <summary>
        /// A temp vector2.
        /// </summary>
        private Vector2 tempVector2;

        /// <summary>
        /// The position of "No scores have"
        /// </summary>
        private Vector2 noScoresHavePosition;

        /// <summary>
        /// Origin of "No scores have" text.
        /// </summary>
        private Vector2 noScoresHaveOrigin;

        /// <summary>
        /// The position of "been recorded."
        /// </summary>
        private Vector2 beenRecordedPosition;

        /// <summary>
        /// Origin of "been recorded" text.
        /// </summary>
        private Vector2 beenRecoredOrigin;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreboardDisplay"/> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        /// <param name="textDrawLocation">The text draw location.</param>
        /// <param name="rectangleDrawLocation">The rectangle draw location.</param>
        public ScoreboardDisplay(Screen screen, Vector2 textDrawLocation, Vector2 rectangleDrawLocation)
        {
            this.textDrawLocation = textDrawLocation;
            this.nameDrawLocation = textDrawLocation + new Vector2(270, 0);
            this.scoreDrawLocation = nameDrawLocation + new Vector2(1150, 0);
            this.highScoresLogoLocation = rectangleDrawLocation + new Vector2(-310, 0);
            this.highScoresLogoOrigin = FontStatic.Get("defaultFont").MeasureString("High Scores") / 2f;
            this.verticleSpacing = 100;
            this.rectangleDrawLocation = rectangleDrawLocation;
            this.scrollableRectangle = new Viewport(
                (int)((rectangleDrawLocation.X - 685 / 2f) * Drawer.GetRatio()), 
                (int)((rectangleDrawLocation.Y - 370 / 2f) * Drawer.GetRatio()),
                (int)(685 * Drawer.GetRatio()),
                (int)(370 * Drawer.GetRatio()));
            this.scoreOffset = Vector2.Zero;
            this.scoreOffsetAcceloration = 0;
            this.scoreOffsetVelocity = 0;
            this.previousHighlighted = -1;
            this.noScoresHavePosition = rectangleDrawLocation + new Vector2(0, -40f);
            this.noScoresHaveOrigin = FontStatic.Get("defaultFont").MeasureString("No scores have") / 2f;
            this.beenRecordedPosition = rectangleDrawLocation + new Vector2(0, 40f);
            this.beenRecoredOrigin = FontStatic.Get("defaultFont").MeasureString("been recorded.") / 2f;

            this.background = new HypercubeDisplay(
                new Rectangle((int)(rectangleDrawLocation.X - 685f / 2f) - 3, (int)(rectangleDrawLocation.Y - 370f / 2f) - 3, 685 + 6, 370 + 6),
                3,
                screen.random,
                0.91f);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.background.Update();

            if (this.previousHighlighted != GameInfoCollection.CurrentIndex)
            {
                this.previousHighlighted = GameInfoCollection.CurrentIndex;
                this.scoreOffset.Y = 0;
                this.scoreOffsetAcceloration = this.scoreOffsetVelocity = 0f;
            }

            Scoreboard tempScoreboard = GameWorld.gameSaver.GetScoreBoard(GameInfoCollection.GameInfos[GameInfoCollection.CurrentIndex].ScoreboardIndex);
            if (tempScoreboard != null)
            {
                bool canMove = tempScoreboard.Count > 10;
                if (canMove)
                {
                    this.scoreOffset.Y += this.scoreOffsetVelocity;
                    this.scoreOffsetVelocity += this.scoreOffsetAcceloration;

                    if (GameWorld.controller.ContainsBool(ActionType.DPadUp))
                    {
                        this.scoreOffsetAcceloration = Math.Min(1f, this.scoreOffsetAcceloration + 0.05f);
                    }
                    if (GameWorld.controller.ContainsBool(ActionType.DPadDown))
                    {
                        this.scoreOffsetAcceloration = Math.Min(-1f, this.scoreOffsetAcceloration - 0.05f);
                    }
                    if ((GameWorld.controller.ContainsBool(ActionType.DPadDown) && GameWorld.controller.ContainsBool(ActionType.DPadUp)) 
                        || (!GameWorld.controller.ContainsBool(ActionType.DPadUp) && !GameWorld.controller.ContainsBool(ActionType.DPadDown)))
                    {
                        this.scoreOffsetAcceloration = this.scoreOffsetVelocity = 0f;
                    }

                    if (this.scoreOffset.Y > 0)
                    {
                        this.scoreOffset.Y = 0;
                        this.scoreOffsetAcceloration = this.scoreOffsetVelocity = 0f;
                    }

                    float maxHeight = (tempScoreboard.Count - 1) * this.verticleSpacing - 880;
                    if (this.scoreOffset.Y < -maxHeight)
                    {
                        this.scoreOffset.Y = -maxHeight;
                        this.scoreOffsetAcceloration = this.scoreOffsetVelocity = 0f;
                    }
                }
                else
                {
                    this.scoreOffset.Y = 0;
                    this.scoreOffsetAcceloration = this.scoreOffsetVelocity = 0f;
                }
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            this.background.Draw();
            Drawer.Draw(
                TextureStatic.Get("TitleNameBorder"),
                this.rectangleDrawLocation,
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin("TitleNameBorder"),
                1f,
                SpriteEffects.None,
                0.92f);

            // Don't draw if a menu entry is selected.
            if (GameInfoCollection.CurrentIndex != GameInfoCollection.GameInfos.Count - 1)
            {
                Scoreboard tempScoreboard = GameWorld.gameSaver.GetScoreBoard(GameInfoCollection.GameInfos[GameInfoCollection.CurrentIndex].ScoreboardIndex);
                if (tempScoreboard != null)
                {
                    Drawer.DrawOutlinedString(
                        FontStatic.Get("defaultFont"),
                        "High Scores",
                        this.highScoresLogoLocation,
                        Color.White,
                        MathHelper.PiOver2,
                        this.highScoresLogoOrigin,
                        0.5f,
                        SpriteEffects.None,
                        0.921f);

                    if (tempScoreboard.Count > 0)
                    {
                        GameWorld.spriteBatch.End();
                        Viewport previousViewport = GameWorld.spriteBatch.GraphicsDevice.Viewport;
                        GameWorld.spriteBatch.GraphicsDevice.Viewport = this.scrollableRectangle;
                        GameWorld.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                        // Find the index of the first entry to be drawn.
                        int firstIndex = (int)Math.Min(Math.Ceiling(Math.Abs((this.textDrawLocation.Y - this.scoreOffset.Y) / this.verticleSpacing)), tempScoreboard.Count - 1);

                        // Find the number of entries to display.                    
                        int entries = (int)Math.Min((1080 / verticleSpacing + 4), tempScoreboard.Count - firstIndex);

                        this.tempVector2.X = 0;
                        this.tempVector2.Y = Math.Max(firstIndex - 3, 0) * this.verticleSpacing;
                        for (int i = Math.Max(firstIndex - 3, 0); i < firstIndex + entries; i++)
                        {
                            Drawer.DrawOutlinedString(
                                FontStatic.Get("defaultFont"),
                                (i + 1).ToString() + ".",
                                this.textDrawLocation + this.scoreOffset + this.tempVector2,
                                Color.White,
                                0f,
                                Vector2.Zero,
                                0.88f,
                                SpriteEffects.None,
                                0.98f);
                            Drawer.DrawOutlinedString(
                                FontStatic.Get("defaultFont"),
                                tempScoreboard[i].Gamertag,
                                this.nameDrawLocation + this.scoreOffset + this.tempVector2,
                                Color.White,
                                0f,
                                Vector2.Zero,
                                0.88f,
                                SpriteEffects.None,
                                0.98f);
                            Drawer.DrawOutlinedString(
                                FontStatic.Get("defaultFont"),
                                Drawer.FormatNumber(((uint)Math.Max(tempScoreboard[i].Scores[GameInfoCollection.CurrentIndex], 0))),
                                this.scoreDrawLocation + this.scoreOffset + this.tempVector2,
                                Color.White,
                                0f,
                                Vector2.Zero,
                                0.88f,
                                SpriteEffects.None,
                                0.98f);
                            this.tempVector2.Y += this.verticleSpacing;
                        }

                        GameWorld.spriteBatch.End();
                        GameWorld.spriteBatch.GraphicsDevice.Viewport = previousViewport;
                        GameWorld.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    }
                    else
                    {
                        Drawer.DrawOutlinedString(
                            FontStatic.Get("defaultFont"),
                            "No scores have",
                            this.noScoresHavePosition,
                            Color.White,
                            0f,
                            this.noScoresHaveOrigin,
                            0.7f,
                            SpriteEffects.None,
                            0.921f);
                        Drawer.DrawOutlinedString(
                            FontStatic.Get("defaultFont"),
                            "been recorded.",
                            this.beenRecordedPosition,
                            Color.White,
                            0f,
                            this.beenRecoredOrigin,
                            0.7f,
                            SpriteEffects.None,
                            0.921f);
                    }
                }
            }
        }
    }
}
