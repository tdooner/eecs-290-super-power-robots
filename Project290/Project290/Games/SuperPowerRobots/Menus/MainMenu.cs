using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project290.Menus;
using Project290.Menus.MenuDelegates;
using Project290.GameElements;
using Project290.Inputs;
using Project290.Games.SuperPowerRobots.Menus.MenuDelegates;

namespace Project290.Games.SuperPowerRobots.Menus
{
    class MainMenu : Menu
    {
        public MainMenu(Vector2 position, MenuAction[] actions, float spacing, int scoreBoardIndex)
            : base(position, actions)
        {
            MenuEntry play = new MenuEntry(
                "Play",
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new PlayGameDelegate(scoreBoardIndex))
                },
                position);

            MenuEntry quit = new MenuEntry(
                "Quit",
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new GoToTitleDelegate())
                },
                position + new Vector2(0, 3 * spacing));

            play.UpperMenu = quit;
            play.LowerMenu = quit;

            quit.UpperMenu = play;
            quit.LowerMenu = play;

            this.Add(play);
            this.Add(quit);
        }
    }
}
