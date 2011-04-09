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
using Project290.Menus.MenuDelegates;

namespace Project290.Screens.Title
{
    /// <summary>
    /// Used for representing a game's desription, box art, authors, and scoreboard index.
    /// </summary>
    public class GameInfo
    {
        /// <summary>
        /// Gets the title of the game.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the box art (name of the Texture2D).
        /// </summary>
        public string BoxArt { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the authors.
        /// </summary>
        public string Authors { get; private set; }

        /// <summary>
        /// Gets the index of the scoreboard.
        /// </summary>
        /// <value>
        /// The index of the scoreboard.
        /// </value>
        public int ScoreboardIndex { get; private set; }

        /// <summary>
        /// The name of the 1450 * 800 image constain "how to play"
        /// </summary>
        public string InstructionsImageName { get; private set; }

        /// <summary>
        /// Used for launching the game when this is selected.
        /// </summary>
        private IMenuDelegate GameCreationDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInfo"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="boxArt">The box art (name of the Texture2D).</param>
        /// <param name="description">The description.</param>
        /// <param name="authors">The authors.</param>
        /// <param name="instructionsImageName">The name of the 1450 * 800 image constain "how to play".</param>
        /// <param name="scoreboardIndex">Index of the scoreboard.</param>
        /// <param name="creationDelegate">The creation delegate.</param>
        public GameInfo(string title, string boxArt, string description, string authors, string instructionsImageName, int scoreboardIndex, IMenuDelegate creationDelegate)
        {
            this.Title = title;
            this.BoxArt = boxArt;
            this.Description = description;
            this.Authors = authors;
            this.ScoreboardIndex = scoreboardIndex;
            this.GameCreationDelegate = creationDelegate;
            this.InstructionsImageName = instructionsImageName;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            if (this.GameCreationDelegate != null)
            {
                this.GameCreationDelegate.Run();
            }
        }
    }
}
