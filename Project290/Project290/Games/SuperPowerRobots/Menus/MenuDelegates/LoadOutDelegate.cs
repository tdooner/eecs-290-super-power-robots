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
        int[] weapons;
        int count;

         public LoadOutDelegate(int count, int type, int[] weapons)
            :base()
        {
            this.count = count;
            this.weapons = weapons;
            weapons[count] = type;
            count++;
        }

        public void Run()
        {
            if (count == 3)
            {
                GameWorld.screens[GameWorld.screens.Count - 1].Disposed = true;
            }
            GameWorld.screens.Play(new LoadOutScreen(count, weapons));
        }
    }
}
