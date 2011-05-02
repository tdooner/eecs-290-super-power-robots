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
        public LoadOutMenu(Vector2 position, MenuAction[] actions, int count)
            :base(position, actions)
        {
            MenuEntry gun = new MenuEntry(
                "Gun",
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, "Gun"))
                },
                position);

            MenuEntry axe = new MenuEntry(
                "Melee",
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, "Melee"))
                },
                position + new Vector2(0, 80));

            MenuEntry shield = new MenuEntry(
                "Shield",
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, "Shield"))
                },
                position + new Vector2(0,160));

            gun.UpperMenu = shield;
            gun.LowerMenu = axe;
            axe.UpperMenu = gun;
            axe.LowerMenu = shield;
            shield.UpperMenu = axe;
            shield.LowerMenu = gun;

            this.Add(gun);
            this.Add(axe);
            this.Add(shield);
        }
    }
}
