using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Clock;
using Project290.GameElements;
using Project290.Screens.Title;
using System.Threading;
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project290.Games.SuperPowerRobots.Entities;
using Project290.Games.SuperPowerRobots;
using Project290.Physics.Dynamics;
using Project290.Games.SuperPowerRobots.Controls;
using Project290.Inputs;
using Project290.Games.SuperPowerRobots.Menus;
using Project290.Menus;
using Project290.Games.SuperPowerRobots.Menus.MenuDelegates;

namespace Project290.Screens
{
    /// <summary>
    /// This is a Screen specific for a game (as opposed to title or pause screen).
    /// </summary>
    public class LoadOutScreen : Screen
    {
        private int count;
        private int[] weapons;
        private LoadOutMenu menu;
        private Vector2 textDrawPosition;
        private Vector2 textDrawOrigin;

        public LoadOutScreen(int count, int[] weapons)
            : base()
        {
            this.menu = new LoadOutMenu(
                new Vector2(0, -60),
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, 0, weapons)),
                },
            60f,
            count,
            weapons);
        }


        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
                /*Drawer.Draw(
                    TextureStatic.Get(),
                    new Vector2(1920f / 2f, 1080f / 2f),
                    null,
                    Color.White,
                    0f,
                    TextureStatic.GetOrigin(),
                    1f,
                    SpriteEffects.None,
                    0.5f);*/
            }
        
    }
}
