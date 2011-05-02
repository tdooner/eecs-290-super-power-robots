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
        LoadOutScreen parent;

         public LoadOutDelegate(int count, string name, LoadOutScreen l)
            :base()
        {
            thecount = count;
            chosen = name;
            parent = l;
        }

        public void Run()
        {
            parent.save(chosen);
            /*if (count == 3)
            {
                GameWorld.screens[GameWorld.screens.Count - 1].Disposed = true;
            }
            GameWorld.screens.Play(new LoadOutScreen(0));*/
        }
    }
}
