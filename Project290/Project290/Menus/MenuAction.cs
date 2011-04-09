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
using Project290.GameElements;
using Project290.Inputs;

namespace Project290.Menus
{
    /// <summary>
    /// This is a link between the pressing of some button on the controller
    /// and performing some action through a delegate.
    /// </summary>
    public class MenuAction
    {
        /// <summary>
        /// Gets the type of the action corresponding to this menu action.
        /// </summary>
        /// <value>The type of the action.</value>
        public ActionType actionType { get; private set; }

        /// <summary>
        /// This is the delegate holding the action or set of actions to be performed.
        /// </summary>
        private IMenuDelegate menuDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuAction"/> class.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <param name="menuDelegate">The menu delegate.</param>
        public MenuAction(ActionType actionType, IMenuDelegate menuDelegate)
        {    
            this.actionType = actionType;
            this.menuDelegate = menuDelegate;
        }

        /// <summary>
        /// Tries to run the delegate. This should be called every cycle, as
        /// this will check the controller to see if the corresponding action
        /// has been performed. If so, and if the delegate is not null, then
        /// the delegated action is performed.
        /// </summary>
        public void TryRunDelegate()
        {
            if (this.menuDelegate != null)
            {
                if (GameWorld.controller.ContainsBool(this.actionType))
                {
                    GameWorld.audio.PlaySound("menuClick");
                    this.menuDelegate.Run();
                }
            }
        }
    }
}
