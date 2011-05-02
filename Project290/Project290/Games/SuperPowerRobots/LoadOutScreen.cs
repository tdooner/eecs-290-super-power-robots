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
        private SPRGameScreen parentobj;

        private static string[] weapons;
        private string yWeapons;
        private string xWeapons;
        private string bWeapons;
        private string aWeapons;

        public LoadOutScreen(int scoreboardIndex, SPRGameScreen p)
            : base(scoreboardIndex)
        {
            weapons = new string[4];
            count = 0;
            parentobj = p;

            this.menu = new LoadOutMenu(
                new Vector2(0, -60),
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new LoadOutDelegate(count, "Gun", this)),
                },
            count, this);
            this.menu.position = new Vector2(1920f / 2f, 1080f / 2f);

        }


        public override void Update()
        {
            base.Update();
            this.menu.Update();
        }
        public void save(string weapon)
        {
            weapons[count] = weapon;
            count += 1;
            if (count == 4)
            {
                // gtfo.
                Dictionary<string, string> a = new Dictionary<string, string>();
                a.Add("Gun", "gun");
                a.Add("Melee", "melee");
                a.Add("Shield", "shield");
                Dictionary<string, string> b = new Dictionary<string, string>();
                b.Add("Gun", "Gun");
                b.Add("Melee", "Shield");
                b.Add("Shield", "Shield");
                // let's assume certain things are selected...
                xWeapons = weapons[0];
                yWeapons = weapons[1];
                aWeapons = weapons[2];
                bWeapons = weapons[3];

                Battle.nextAllies = "<Allies>\n  <Bot>\n    <AI>HumanAI</AI>\n    <Health>500</Health>\n    <Texture>4SideFriendlyRobot</Texture>\n    <PositionX>400</PositionX>\n    <PositionY>400</PositionY>\n    <Weapon>\n      <Type>" + a[xWeapons] + "</Type>\n<Texture>" + b[xWeapons] + "</Texture>\n      <Health>300</Health>\n      <Power>10</Power>\n    </Weapon>\n    <Weapon>\n      <Type>" + a[yWeapons] + "</Type>\n      <Texture>" + b[yWeapons] + "</Texture>\n      <Health>300</Health>\n      <Power>10</Power>\n    </Weapon>\n    <Weapon>\n       <Type>" + a[aWeapons] + "</Type>\n<Texture>" + b[aWeapons] + "</Texture>\n      <Health>300</Health>\n      <Power>10</Power>\n    </Weapon>\n    <Weapon>\n       <Type>" + a[bWeapons] + "</Type>\n<Texture>" + b[bWeapons] + "</Texture>\n      <Health>300</Health>\n      <Power>10</Power>\n    </Weapon>\n  </Bot>\n  <Level>0</Level>\n  <HasWatchedIntro>0</HasWatchedIntro>\n</Allies>\n";

                this.Disposed = true;
                parentobj.NextLevel();
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
                new Vector2(1920/2, 100),
                0.35f,
                SpriteEffects.None,
                1f);
            this.menu.Draw();
            
        }
    }
}
