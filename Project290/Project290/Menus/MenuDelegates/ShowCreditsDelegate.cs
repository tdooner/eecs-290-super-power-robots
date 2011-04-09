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
using Project290.Screens;
using Project290.GameElements;

namespace Project290.Menus.MenuDelegates
{
    /// <summary>
    /// Used for popping up the credits screen.
    /// </summary>
    public class ShowCreditsDelegate : IMenuDelegate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShowCreditsDelegate"/> class.
        /// </summary>
        public ShowCreditsDelegate()
            : base()
        {
        }

        /// <summary>
        /// Runs this instance, performing the action once,
        /// and thereafter exiting the method.
        /// </summary>
        public void Run()
        {
            // Do nothing if there is already a credits screen...
            foreach (Screen screen in GameWorld.screens)
            {
                if (screen is CreditsScreen)
                {
                    return;
                }
            }

            GameWorld.screens.Play(new CreditsScreen());
        }
    }
}
