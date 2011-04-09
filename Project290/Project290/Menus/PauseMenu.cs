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
using Project290.Menus.MenuDelegates;
using Project290.GameElements;
using Project290.Inputs;

namespace Project290.Menus
{
    /// <summary>
    /// An instance of a pause menu. This will support the options
    /// to resume the game, restart the level, go to the options menu,
    /// buy the game (if in trial mode), go to the title sceen, and 
    /// quit the game.
    /// </summary>
    public class PauseMenu : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PauseMenu"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="actions">The actions.</param>
        /// <param name="spacing">The spacing.</param>
        public PauseMenu(Vector2 position, MenuAction[] actions, float spacing)
            : base(position, actions)
        {
            MenuEntry resume = new MenuEntry(
                "Resume",
                new MenuAction[] 
                { 
                    new MenuAction(ActionType.Select, new QuitPauseScreenDelegate()) 
                },
                position);

            VolumeControlDisplayEntry music = new VolumeControlDisplayEntry(
                "Music Volume",
                string.Empty,
                new MenuAction[] { },
                position + new Vector2(0, spacing),
                new Vector2(0, 300),
                0.9f,
                0.9f,
                true,
                false);

            VolumeControlDisplayEntry sound = new VolumeControlDisplayEntry(
                "Sound Volume",
                string.Empty,
                new MenuAction[] { },
                position + new Vector2(0, 2 * spacing),
                new Vector2(0, 300),
                0.9f,
                0.9f,
                false,
                false);

            MenuEntry quit = new MenuEntry(
                "Exit",
                new MenuAction[] 
                { 
                    new MenuAction(ActionType.Select, new GoToTitleDelegate())
                },
                position + new Vector2(0, 3 * spacing));

            resume.UpperMenu = quit;
            resume.LowerMenu = music;

            music.UpperMenu = resume;
            music.LowerMenu = sound;

            sound.UpperMenu = music;
            sound.LowerMenu = quit;

            quit.UpperMenu = sound;
            quit.LowerMenu = resume;

            this.Add(resume);
            this.Add(music);
            this.Add(sound);
            this.Add(quit);
        }
    }
}
