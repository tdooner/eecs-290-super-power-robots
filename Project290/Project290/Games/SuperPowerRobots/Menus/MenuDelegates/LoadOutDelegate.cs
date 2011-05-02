using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Menus;
using Project290.Menus.MenuDelegates;
using Project290.GameElements;

namespace Project290.Games.SuperPowerRobots.Menus.MenuDelegates
{
    class LoadOutDelegate : IMenuDelegate
    {
        int thecount;
        string chosen;

         public LoadOutDelegate(int count, string name)
            :base()
        {
            thecount = count;
            chosen = name;
        }

        public void Run()
        {
            LoadOutScreen.save(thecount, chosen);
            /*if (count == 3)
            {
                GameWorld.screens[GameWorld.screens.Count - 1].Disposed = true;
            }
            GameWorld.screens.Play(new LoadOutScreen(0));*/
        }
    }
}
