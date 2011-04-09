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
using Project290.Screens;

namespace Project290.Menus.MenuDelegates
{
    /// <summary>
    /// This kills the pause screen.
    /// </summary>
    public class QuitPauseScreenDelegate : IMenuDelegate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuitPauseScreenDelegate"/> class.
        /// </summary>
        public QuitPauseScreenDelegate()
            : base()
        {
        }

        /// <summary>
        /// This kills the pause screen.
        /// </summary>
        public void Run()
        {
            foreach (Screen screen in GameWorld.screens)
            {
                if (screen is PauseScreen)
                {
                    (screen as PauseScreen).Quit();
                }
            }
        }
    }
}

