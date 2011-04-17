using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.GameElements;
using Project290.Games.StupidGame;
using Project290.Menus.MenuDelegates;

namespace Project290.Games.SuperPowerRobots
{
    class LaunchFightScreenDelegate : IMenuDelegate
    {

        private int scoreboardIndex;

        public LaunchFightScreenDelegate(int scoreboardIndex)
            : base()
        {
            this.scoreboardIndex = scoreboardIndex;
        }

        public void Run()
        {
            if (GameWorld.screens.Count > 0)
            {
                GameWorld.screens[GameWorld.screens.Count - 1].Disposed = true;
            }
        }
    }
}
