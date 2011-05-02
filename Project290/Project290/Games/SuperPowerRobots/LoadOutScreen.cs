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
using Project290.Screens;

namespace Project290.Games.SuperPowerRobots
{
    /// <summary>
    /// This is a Screen specific for a game (as opposed to title or pause screen).
    /// </summary>
    public class LoadOutScreen : GameScreen
    {
        private int count;
        private LoadOutMenu menu;
        private Vector2 textDrawPosition;
        private Vector2 textDrawOrigin;

        private static string[] weapons;
        private string yWeapons;
        private string xWeapons;
        private string bWeapons;
        private string aWeapons;
        private Bot bot;

        public LoadOutScreen(int scoreboardIndex)
            : base(scoreboardIndex)
        {
            weapons = new string[4];
            this.menu = new LoadOutMenu(
                new Vector2(0, -60),
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, "Gun")),
                },
            count);
            this.menu.position = new Vector2(1920f / 2f, 1080f / 2f);

            Dictionary<string, string> a = new Dictionary<string, string>();
            a.Add("Gun", "gun");
            a.Add("Melee", "shield");
            a.Add("Shield", "shield");
            // let's assume certain things are selected...
            xWeapons = "Gun";
            yWeapons = "Melee";
            aWeapons = "Gun";
            bWeapons = "Shield";

            Battle.nextAllies = "<Allies>\n  <Bot>\n    <AI>HumanAI</AI>\n    <Health>500</Health>\n    <Texture>4SideFriendlyRobot</Texture>\n    <PositionX>400</PositionX>\n    <PositionY>400</PositionY>\n    <Weapon>\n      <Type>" + a[xWeapons] + "</Type>\n<Texture>" + xWeapons + "</Texture>\n      <Health>300</Health>\n      <Power>10</Power>\n    </Weapon>\n    <Weapon>\n      <Type>" + a[yWeapons] + "</Type>\n      <Texture>" + yWeapons + "</Texture>\n      <Health>300</Health>\n      <Power>10</Power>\n    </Weapon>\n    <Weapon>\n       <Type>" + a[aWeapons] + "</Type>\n<Texture>" + aWeapons + "</Texture>\n      <Health>300</Health>\n      <Power>10</Power>\n    </Weapon>\n    <Weapon>\n       <Type>" + a[bWeapons] + "</Type>\n<Texture>" + bWeapons + "</Texture>\n      <Health>300</Health>\n      <Power>10</Power>\n    </Weapon>\n  </Bot>\n  <Level>0</Level>\n  <HasWatchedIntro>0</HasWatchedIntro>\n</Allies>\n";
        }


        public override void Update()
        {
            base.Update();
            this.menu.Update();
        }
        public static void save(int count, string weapon)
        {
            weapons[count] = weapon;
            count += 1;
            if (count == 4)
            {
                // gtfo.
            }
        }
        public override void Draw()
        {
            base.Draw();
            Drawer.DrawRectangle(
                new Rectangle(0, 0, 1920, 1080),
                1920,
                0f,
                Color.Black);
            Drawer.DrawString(
                FontStatic.Get("defaultFont"),
                "Please Select Your Weapon For Slot #" + count,
                new Vector2(300, 100),
                Color.White,
                0f,
                this.textDrawOrigin,
                0.35f,
                SpriteEffects.None,
                1f);
            this.menu.Draw();
            
        }
    }
}
