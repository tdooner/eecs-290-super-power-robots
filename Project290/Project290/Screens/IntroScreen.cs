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
        private string[] pictures;
        private bool isOver;
        private float time;
        private float[] thresh;
        private int count;

        public IntroScreen()
            : base()
        {
            pictures = new String[5];
            thresh = new float[5];
            thresh[0] = 6000;
            thresh[1] = 10000;
            thresh[2] = 3000;
            thresh[3] = 12000;
            thresh[4] = 10000;
            this.time = 0f;
            this.currentTime = (long)System.DateTime.Now.Millisecond;
            pictures[0] = "pic1";
            pictures[1] = "pic2";
            pictures[2] = "pic3";
            pictures[3] = "pic4";
            pictures[4] = "pic5";

            this.FadingOut = false;
            this.isOver = false;
        }

        public void Update()
        {
            base.Update();
            
            float dTime = currentTime - (long)System.DateTime.Now.Millisecond;
            time += dTime;

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
                GameWorld.screens.Add(new GameScreen(1));
            }
        }

        public override void Draw()
        {
            base.Draw();

            Drawer.Draw(
                TextureStatic.Get(this.pictures[count]),
                new Vector2(1920f / 2f, 1080f / 2f),
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin(this.pictures[count]),
                1f,
                SpriteEffects.None,
                0.5f);
        }
    }
}
