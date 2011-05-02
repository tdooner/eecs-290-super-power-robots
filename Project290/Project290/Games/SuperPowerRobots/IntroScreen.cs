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

namespace Project290.Screens
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
            base.Disposed = true;
            GameWorld.screens.Add(new GameScreen(this.scoreBoardIndex));
            /*
            pictures = new String[5];
            thresh = new float[5];
            thresh[0] = 1000f;
            thresh[1] = 1000f;
            thresh[2] = 1000f;
            thresh[3] = 1000f;
            thresh[4] = 1000f;
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
            this.isOver = false;*/
        }

        public void Update()
        {
            base.Disposed = true;
            GameWorld.screens.Add(new GameScreen(this.scoreBoardIndex));
            /*
            base.Update();
            float dTime = currentTime - (long)System.DateTime.Now.Millisecond;
            currentTime = (long)System.DateTime.Now.Millisecond;
            time += dTime;
            Console.WriteLine(time);

            if (time > thresh[count])
            {
                count++;
                time = 0;
            }

            if (count > thresh.Length)
                this.isOver = true;

            if (this.isOver)
            {
                base.Disposed = true;
                GameWorld.screens.Add(new GameScreen(this.scoreBoardIndex));
            }*/
        }

        public override void Draw()
        {
            base.Draw();

            /*Drawer.Draw(
                TextureStatic.Get(this.pictures[count]),
                new Vector2(1920f / 2f, 1080f / 2f),
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin(this.pictures[count]),
                1f,
                SpriteEffects.None,
                0.5f);*/
        }
    }
}
