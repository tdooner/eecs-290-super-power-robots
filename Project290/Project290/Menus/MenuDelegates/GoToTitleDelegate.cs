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
using Project290.Screens.Title;
using Project290.Screens;
using Project290.Clock;
using Project290.Screens.Background;

namespace Project290.Menus.MenuDelegates
{
    /// <summary>
    /// This pops the most recent screen off of the screen
    /// stack of GameWorld.
    /// </summary>
    public class GoToTitleDelegate : IMenuDelegate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoToTitleDelegate"/> class.
        /// </summary>
        public GoToTitleDelegate()
            : base()
        {
        }

        /// <summary>
        /// Runs this instance. This pops the most recent screen off of the screen
        /// stack of GameWorld.
        /// </summary>
        public void Run()
        {
            foreach (Screen screen in GameWorld.screens)
            {
                screen.Disposed = true;
            }

            GameClock.Reset();
            GameWorld.screens.Play(new TitleScreen());
        }
    }
}
