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
using Project290.Inputs;
using Project290.Menus.MenuDelegates;

namespace Project290.Menus
{
    /// <summary>
    /// The menu displayed on the box art for the box art that is a menu.
    /// </summary>
    public class BoxArtMenu : Menu
    {
        /// <summary>
        /// The verticle spacing between entries.
        /// </summary>
        private float spacing;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxArtMenu"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="actions">The actions.</param>
        /// <param name="spacing">The spacing.</param>
        public BoxArtMenu(Vector2 position, MenuAction[] actions, float spacing)
            : base(position, actions)
        {
            this.spacing = spacing;

            VolumeControlDisplayEntry music = new VolumeControlDisplayEntry(
                "Music Volume",
                "Press Left and Right to set the music\nvolume for the game.",
                new MenuAction[] { },
                position,
                new Vector2(1342, 400),
                1f,
                0.99f,
                true,
                true);

            VolumeControlDisplayEntry sound = new VolumeControlDisplayEntry(
                "Sound Volume",
                "Press Left and Right to set the sound\nvolume for the game.",
                new MenuAction[] { },
                position + new Vector2(0, spacing),
                new Vector2(1342, 400),
                1f,
                0.99f,
                false,
                true);

            DescriptionMenuEntry storage = new DescriptionMenuEntry(
                "Storage Device",
                "Press #A to change the Storage Device used\nfor saving scores. No device selection menu\nwill be displayed if you only have one\ndevice connected.",
                new MenuAction[] 
                { 
                    new MenuAction(ActionType.Select, new ChangeStorageDeviceDelegate())
                },
                position + new Vector2(0, 2 * spacing));

            DescriptionMenuEntry credits = new DescriptionMenuEntry(
                "Credits",
                "Press #A to see the names of the developers.",
                new MenuAction[] 
                { 
                    new MenuAction(ActionType.Select, new ShowCreditsDelegate())
                },
                position + new Vector2(0, 3 * spacing));

            DescriptionMenuEntry quit = new DescriptionMenuEntry(
                "Exit",
                "Press #A to exit the game.",
                new MenuAction[] 
                { 
                    new MenuAction(ActionType.Select, new QuitGameDeleage())
                },
                position + new Vector2(0, 4 * spacing));

            // Do not "loop around". 
            music.LowerMenu = sound;

            sound.UpperMenu = music;
            sound.LowerMenu = storage;

            storage.UpperMenu = sound;
            storage.LowerMenu = credits;

            credits.UpperMenu = storage;
            credits.LowerMenu = quit;

            quit.UpperMenu = credits;

            this.Add(music);
            this.Add(sound);
            this.Add(storage);
            this.Add(credits);
            this.Add(quit);
        }
    }
}