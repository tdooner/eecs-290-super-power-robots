using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.GameElements;
using Project290.Menus;
using Project290.Menus.MenuDelegates;
using Project290.Screens;

namespace Project290.Games.SuperPowerRobots.Menus.MenuDelegates
{
    class PlayGameDelegate : IMenuDelegate
    {
        private int scoreBoardIndex;

        public PlayGameDelegate(int scoreBoardIndex)
            : base()
        {
            this.scoreBoardIndex = scoreBoardIndex;
        }

        public void Run()
        {
            if (GameWorld.screens.Count > 0)
            {
                GameWorld.screens[GameWorld.screens.Count - 1].Disposed = true;
            }
            GameWorld.screens.Play(new IntroScreen(this.scoreBoardIndex));
        }

    }
}
