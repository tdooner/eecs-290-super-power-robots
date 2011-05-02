using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Menus;
using Microsoft.Xna.Framework;
using Project290.Inputs;
using Project290.Games.SuperPowerRobots.Menus.MenuDelegates;

namespace Project290.Games.SuperPowerRobots.Menus
{
    class LoadOutMenu : Menu
    {
        public LoadOutMenu(Vector2 position, MenuAction[] actions, float spacing,  int count, int[] weapons)
            :base(position, actions)
        {
            MenuEntry gun = new MenuEntry(
                "Gun",
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, 0, weapons))
                },
                position);

            MenuEntry axe = new MenuEntry(
                "Shield",
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, 1, weapons))
                },
                position);

            MenuEntry shield = new MenuEntry(
                "Shield",
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, 0, weapons))
                },
                position);
        }
    }
}

ActionType.Select, new LoadOutDelegate(count, 0, weapons)
