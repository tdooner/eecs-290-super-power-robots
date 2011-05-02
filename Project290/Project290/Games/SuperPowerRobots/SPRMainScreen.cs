using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Screens;
using Microsoft.Xna.Framework;
using Project290.Rendering;
using Project290.GameElements;
using Project290.Inputs;
using Microsoft.Xna.Framework.Graphics;
using Project290.Clock;
using Project290.Physics.Dynamics;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Factories;
using Project290.Games.SuperPowerRobots.Entities;
using Project290.Games.SuperPowerRobots.Menus;
using Project290.Games.SuperPowerRobots.Menus.MenuDelegates;
using Project290.Menus;
using Project290.Menus.MenuDelegates;
using Project290.Mathematics;
using Project290.Screens.Shared;

namespace Project290.Games.SuperPowerRobots
{
    class SPRMainScreen : Screen
    {
        private MainMenu menu;

        private tVector2 position;

        private Vector2 textDrawPosition;

        private Vector2 textDrawOrigin;

        public SPRMainScreen(int scoreBoardIndex)
            : base()
        {
            this.menu = new MainMenu(
                new Vector2(0, -60),
                new MenuAction[]
                {
                    new MenuAction(ActionType.Select, new PlayGameDelegate(scoreBoardIndex))
                },
                50,
                scoreBoardIndex);

            this.textDrawPosition = new Vector2(265, -225);
            this.textDrawOrigin = FontStatic.Get("defaultFont").MeasureString("Super Power");
            this.position = new tVector2(1920f / 2f, 1080f / 2f);
            this.position.GoTo(1920f / 2f, 1080f / 2f, 0.3f, true);
        }

        public override void Update()
        {
            base.Update();

            this.menu.position = this.position.Value;

            this.menu.Update();
        }

        public override void Draw()
        {
            base.Draw();

            // Write "Super Power Robots" at the top-center of the background.
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                "",
                this.position.Value + this.textDrawPosition,
                Color.White,
                0f,
                this.textDrawOrigin,
                1f,
                SpriteEffects.None,
                0.99f);

            // Draw the frame
            Drawer.Draw(
                TextureStatic.Get("BoxArtHolder"),
                this.position.Value,
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin("BoxArtHolder"),
                1f,
                SpriteEffects.None,
                0.3f);

            this.menu.Draw();
        }
    }
}
