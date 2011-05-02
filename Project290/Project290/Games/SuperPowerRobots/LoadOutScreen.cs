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

namespace Project290.Screens
{
    /// <summary>
    /// This is a Screen specific for a game (as opposed to title or pause screen).
    /// </summary>
    public class LoadOutScreen : Screen
    {
        private int scoreBoardIndex;
        private string[] pictures;
        private bool isOver;
        private float time;
        private float[] thresh;
        private int count;

        private int yWeapons;
        private int xWeapons;
        private int bWeapons;
        private int aWeapons;
        private Bot bot;

        public LoadOutScreen(int scoreBoardIndex)
            : base()
        {
            this.scoreBoardIndex = scoreBoardIndex;
            World world = new World(Vector2.Zero);
            SPRWorld loadOutWorld = new SPRWorld(world, 0);

            yWeapons = 0;
            xWeapons = 0;
            bWeapons = 0;
            aWeapons = 0;

            this.bot = new Bot(loadOutWorld, new Body(world), Bot.Player.Computer, Bot.Type.FourSided, new BrickAI(loadOutWorld), TextureStatic.Get("4SideFriendlyRobot"), 62, 62, 100);
            
            bot.AddWeapon(0, "gun", WeaponType.gun, 100f, 10f);
            bot.AddWeapon(1, "gun", WeaponType.gun, 100f, 10f);
            bot.AddWeapon(2, "gun", WeaponType.gun, 100f, 10f);
            bot.AddWeapon(3, "gun", WeaponType.gun, 100f, 10f);

            
        }

        public override void Update()
        {
            base.Update();

            if (GameWorld.controller.ContainsBool(ActionType.BButton) && bWeapons == 0)
            {
                bWeapons = 1;
                bot.RemoveWeapon(bot.GetWeapons()[0]);
                bot.AddWeapon(0, "shield", WeaponType.melee, 100f, 40f);
            }
            else if (GameWorld.controller.ContainsBool(ActionType.BButton) && bWeapons == 1)
            {
                bWeapons = 2;
                bot.RemoveWeapon(bot.GetWeapons()[0]);
                bot.AddWeapon(0, "shield", WeaponType.shield, 100f, 0f);
            }
            else if (GameWorld.controller.ContainsBool(ActionType.BButton))
            {
                bWeapons = 0;
                bot.RemoveWeapon(bot.GetWeapons()[0]);
                bot.AddWeapon(0, "gun", WeaponType.gun, 100f, 10f);
            }


            if (GameWorld.controller.ContainsBool(ActionType.AButton) && aWeapons == 0)
            {
                aWeapons = 1;
                bot.RemoveWeapon(bot.GetWeapons()[1]);
                bot.AddWeapon(0, "shield", WeaponType.melee, 100f, 40f);
            }
            else if (GameWorld.controller.ContainsBool(ActionType.AButton) && aWeapons == 1)
            {
                aWeapons = 2;
                bot.RemoveWeapon(bot.GetWeapons()[1]);
                bot.AddWeapon(0, "shield", WeaponType.shield, 100f, 0f);
            }
            else if (GameWorld.controller.ContainsBool(ActionType.AButton))
            {
                aWeapons = 0;
                bot.RemoveWeapon(bot.GetWeapons()[1]);
                bot.AddWeapon(0, "gun", WeaponType.gun, 100f, 10f);
            }


            if (GameWorld.controller.ContainsBool(ActionType.XButton) && xWeapons == 0)
            {
                xWeapons = 1;
                bot.RemoveWeapon(bot.GetWeapons()[2]);
                bot.AddWeapon(0, "shield", WeaponType.melee, 100f, 40f);
            }
            else if (GameWorld.controller.ContainsBool(ActionType.XButton) && xWeapons == 1)
            {
                xWeapons = 2;
                bot.RemoveWeapon(bot.GetWeapons()[2]);
                bot.AddWeapon(0, "shield", WeaponType.shield, 100f, 0f);
            }
            else if (GameWorld.controller.ContainsBool(ActionType.XButton))
            {
                xWeapons = 0;
                bot.RemoveWeapon(bot.GetWeapons()[2]);
                bot.AddWeapon(0, "gun", WeaponType.gun, 100f, 10f);
            }


            if (GameWorld.controller.ContainsBool(ActionType.YButton) && yWeapons == 0)
            {
                yWeapons = 1;
                bot.RemoveWeapon(bot.GetWeapons()[3]);
                bot.AddWeapon(0, "shield", WeaponType.melee, 100f, 40f);
            }
            else if (GameWorld.controller.ContainsBool(ActionType.YButton) && yWeapons == 1)
            {
                yWeapons = 2;
                bot.RemoveWeapon(bot.GetWeapons()[3]);
                bot.AddWeapon(0, "shield", WeaponType.shield, 100f, 0f);
            }
            else if (GameWorld.controller.ContainsBool(ActionType.YButton))
            {
                yWeapons = 0;
                bot.RemoveWeapon(bot.GetWeapons()[3]);
                bot.AddWeapon(0, "gun", WeaponType.gun, 100f, 10f);
            }
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
