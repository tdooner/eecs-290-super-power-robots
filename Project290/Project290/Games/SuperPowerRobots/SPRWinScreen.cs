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
using Project290.Screens;
using Project290.Inputs;
using Microsoft.Xna.Framework;
using Project290.Screens;
using Project290.Inputs;

namespace Project290.Games.SuperPowerRobots
{
    /// <summary>
    /// This is a Screen specific for a game (as opposed to title or pause screen).
    /// </summary>
    public class SPRWinScreen : Screen
    {
        public SPRWinScreen()
        {
        }

        public override void Update()
        {
            
        }

        public override void Draw()
        {
            base.Draw();
            
            Drawer.Draw(
                TextureStatic.Get("winnerwinnerchickendinner"),
                new Rectangle(1920 / 2, 1080 / 2, 1520, 680),
                new Rectangle(0, 0, 1920, 1080),
                Color.White,
                0f,
                new Vector2(1920/2, 1080/2),
                SpriteEffects.None,
                0.5f);
        }
    }
}
