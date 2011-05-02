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
    public class IntroScreen : Screen
    {
        private long currentTime;
        private int scoreBoardIndex;
        private string[] pictures;
        private bool isOver;
        private float time;
        private float[] thresh;
        private int count;

        public IntroScreen(int scoreBoardIndex)
            : base()
        {
            pictures = new String[5];
            thresh = new float[5];
            thresh[0] = 16000f;
            thresh[1] = 24000f;
            thresh[2] = 28000f;
            thresh[3] = 15000f;
            thresh[4] = 13000f;
            this.time = 0f;
            this.currentTime = (long)System.DateTime.Now.Millisecond;
            pictures[0] = "pic1";
            pictures[1] = "pic2";
            pictures[2] = "pic3";
            pictures[3] = "pic4";
            pictures[4] = "pic5";
            this.scoreBoardIndex = scoreBoardIndex;
            GameWorld.audio.SongPlay("intro");

            this.FadingOut = false;
            this.isOver = false;
        }

        public override void Update()
        {
            if (GameWorld.controller.ContainsBool(ActionType.AButton) && time > 500)
            {
                count++;
                time = 0;
            }

            if (count >= thresh.Length)
            {
                this.isOver = true;
            }
            else
            {
                base.Update();
                float dTime = Math.Abs(currentTime - (long)System.DateTime.Now.Millisecond);
                currentTime = (long)System.DateTime.Now.Millisecond;
                time += dTime;
                Console.WriteLine(time);


                if (time > thresh[count])
                {
                    count++;
                    time = 0;
                }
            }

            if (this.isOver)
            {
                this.Disposed = true;
                GameWorld.screens.Add(new Project290.Games.SuperPowerRobots.SPRGameScreen(this.scoreBoardIndex));
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (count < pictures.Length)
            {
                Drawer.DrawRectangle(
                    new Rectangle(0, 0, 1920, 1080),
                    1920, 
                    0.4f, 
                    Color.Black
                );
                Drawer.Draw(
                    TextureStatic.Get(this.pictures[count]),
                    new Rectangle(1920/2,1080/2,1520,680),
                    new Rectangle(0,0,1920,1080),
                    Color.White,
                    0f,
                    TextureStatic.GetOrigin(this.pictures[count]),
                    SpriteEffects.None,
                    0.5f);
            }
        }
    }
}
