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
using Project290.GameElements;
using Project290.Inputs;
using Microsoft.Xna.Framework.Graphics;
using Project290.FancyFont;
using Project290.Menus;
using Project290.Screens.Shared;

namespace Project290.Screens.Title
{
    /// <summary>
    /// Used for displaying the game title, description, authors, etc.
    /// </summary>
    public class GameInfoDisplay
    {
        /// <summary>
        /// Used for displaying the background.
        /// </summary>
        private HypercubeDisplay background;

        /// <summary>
        /// The position of the title text.
        /// </summary>
        private Vector2 titlePosition;

        /// <summary>
        /// The title string (fancy!)
        /// </summary>
        private FancyString titleString;

        /// <summary>
        /// The position of the description text.
        /// </summary>
        private Vector2 descriptionPosition;

        /// <summary>
        /// The fancy string for the description of the game.
        /// </summary>
        private FancyString descriptionString;

        /// <summary>
        /// The position of the border image.
        /// </summary>
        private Vector2 titleBorderPosition;

        /// <summary>
        /// The previous game that was selected.
        /// </summary>
        private int previousGameInfoSelected;

        /// <summary>
        /// The maximum length of the title to be displayed.
        /// </summary>
        private float maxTitleWidth;

        /// <summary>
        /// The box art menu so that the options names can be displayed in the top right pane.
        /// </summary>
        private BoxArtMenu boxArtMenu;

        /// <summary>
        /// The string that says "A - Play   X - Intructions"
        /// </summary>
        private FancyString aAndXButtonDisplay;

        /// <summary>
        /// Has the menu reset (has it been deselected)?
        /// </summary>
        private bool menuHasReset;

        /// <summary>
        /// The corresponding screen.
        /// </summary>
        private Screen screen;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInfoDisplay"/> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        /// <param name="boxArtMenu">The box art menu.</param>
        /// <param name="titlePosition">The title position.</param>
        /// <param name="maxTitleWidth">Width of the max title.</param>
        /// <param name="descriptionPosition">The description position.</param>
        /// <param name="authorsPosition">The authors position.</param>
        /// <param name="borderPosition">The border position.</param>
        public GameInfoDisplay(Screen screen, BoxArtMenu boxArtMenu, Vector2 titlePosition, float maxTitleWidth, Vector2 descriptionPosition, Vector2 borderPosition)
        {
            this.screen = screen;
            this.titlePosition = titlePosition;
            this.boxArtMenu = boxArtMenu;
            this.titleString = new FancyString();
            this.descriptionPosition = descriptionPosition;
            this.descriptionString = new FancyString();
            this.titleBorderPosition = borderPosition;
            this.previousGameInfoSelected = 10000;
            this.maxTitleWidth = maxTitleWidth;
            this.menuHasReset = false;
            this.aAndXButtonDisplay = new FancyString();
            this.aAndXButtonDisplay.Create("#A Play       #X Instructions", borderPosition + new Vector2(0, 160), borderPosition + new Vector2(0, 160), 0.35f, Color.White, "defaultFont", 0.971f, true);
            this.background = new HypercubeDisplay(
                new Rectangle((int)(borderPosition.X - 685f / 2f) - 3, (int)(borderPosition.Y - 370f / 2f) - 3, 685 + 6, 370 + 6),
                3,
                screen.random,
                0.96f);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.background.Update();

            if (this.screen.IsOnTop())
            {
                if (GameInfoCollection.CurrentIndex != GameInfoCollection.GameInfos.Count - 1 && this.previousGameInfoSelected < 0)
                {
                    this.aAndXButtonDisplay.Create(
                        "#A Play       #X Instructions",
                        this.titleBorderPosition + new Vector2(0, 160),
                        this.titleBorderPosition,
                        0.35f,
                        Color.White,
                        "defaultFont",
                        0.971f,
                        true);
                }
                else if (GameInfoCollection.CurrentIndex == GameInfoCollection.GameInfos.Count - 1 && this.previousGameInfoSelected >= 0)
                {
                    this.aAndXButtonDisplay.Create(
                        string.Empty,
                        this.titleBorderPosition + new Vector2(0, 160),
                        this.titleBorderPosition,
                        0.35f,
                        Color.White,
                        "defaultFont",
                        0.971f,
                        true);
                }

                if (this.previousGameInfoSelected != GameInfoCollection.CurrentIndex && GameInfoCollection.CurrentIndex != GameInfoCollection.GameInfos.Count - 1)
                {
                    float titleXMeasurement = FontStatic.Get("defaultFont").MeasureString(
                        GameInfoCollection.GameInfos[GameInfoCollection.CurrentIndex].Title).X;
                    float titleScale = Math.Min(1f, this.maxTitleWidth / titleXMeasurement);
                    this.titleString.Create(
                        GameInfoCollection.GameInfos[GameInfoCollection.CurrentIndex].Title,
                        this.titlePosition,
                        this.titleBorderPosition,
                        titleScale,
                        Color.White,
                        "defaultFont",
                        0.99f,
                        true);
                    this.descriptionString.Create(
                        GameInfoCollection.GameInfos[GameInfoCollection.CurrentIndex].Description,
                        this.descriptionPosition,
                        this.titleBorderPosition,
                        0.35f,
                        Color.White,
                        "defaultFont",
                        0.99f,
                        false);
                }
                else if (this.previousGameInfoSelected < 0 && this.previousGameInfoSelected != -this.boxArtMenu.CurrentSelected - 1 || this.menuHasReset)
                {
                    this.menuHasReset = false;
                    this.titleString.Create(
                        this.boxArtMenu[this.boxArtMenu.CurrentSelected].text,
                        this.titlePosition,
                        this.titleBorderPosition,
                        1f,
                        Color.White,
                        "defaultFont",
                        0.99f,
                        true);
                    this.descriptionString.Create(
                        (this.boxArtMenu[this.boxArtMenu.CurrentSelected] as DescriptionMenuEntry).Description,
                        this.descriptionPosition,
                        this.titleBorderPosition,
                        0.35f,
                        Color.White,
                        "defaultFont",
                        0.99f,
                        false);
                }

                if (GameInfoCollection.CurrentIndex == GameInfoCollection.GameInfos.Count - 1)
                {
                    if (this.previousGameInfoSelected >= 0)
                    {
                        this.menuHasReset = true;
                    }

                    this.previousGameInfoSelected = -this.boxArtMenu.CurrentSelected - 1;
                }
                else
                {
                    this.previousGameInfoSelected = GameInfoCollection.CurrentIndex;
                }
            }

            this.titleString.Update();
            this.descriptionString.Update();
            this.aAndXButtonDisplay.Update();
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            Drawer.Draw(
                TextureStatic.Get("titleNameBorder"),
                this.titleBorderPosition,
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin("titleNameBorder"),
                1f,
                SpriteEffects.None,
                0.97f);

            this.background.Draw();
            this.titleString.Draw();
            this.descriptionString.Draw();
            this.aAndXButtonDisplay.Draw();
        }
    }
}
