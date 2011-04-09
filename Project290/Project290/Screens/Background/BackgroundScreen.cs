//////__     __               _                 _     _               _   
//////\ \   / /              | |               | |   | |             | |  
////// \ \_/ /__  _   _   ___| |__   ___  _   _| | __| |  _ __   ___ | |_ 
//////  \   / _ \| | | | / __| '_ \ / _ \| | | | |/ _` | | '_ \ / _ \| __|
//////   | | (_) | |_| | \__ \ | | | (_) | |_| | | (_| | | | | | (_) | |_ 
//////   |_|\___/ \__,_| |___/_| |_|\___/ \__,_|_|\__,_| |_| |_|\___/ \__|
//////      _                              _   _     _     _ 
//////     | |                            | | | |   (_)   | |
//////  ___| |__   __ _ _ __   __ _  ___  | |_| |__  _ ___| |
////// / __| '_ \ / _` | '_ \ / _` |/ _ \ | __| '_ \| / __| |
//////| (__| | | | (_| | | | | (_| |  __/ | |_| | | | \__ \_|
////// \___|_| |_|\__,_|_| |_|\__, |\___|  \__|_| |_|_|___(_)
//////                         __/ |                         
//////                        |___/                          

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Particles;
using Microsoft.Xna.Framework;
using Project290.Clock;
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Project290.Screens.Background
{
    /// <summary>
    /// This is the screen below the start and title screen.
    /// </summary>
    public class BackgroundScreen : Screen
    {
        /// <summary>
        /// The background effects behind the particles.
        /// </summary>
        private List<RandomColorEffect> effects;
        
        /// <summary>
        /// Where to draw the background.
        /// </summary>
        private Vector2 backgroundDrawLocation;

        /// <summary>
        /// The rotation of the background.
        /// </summary>
        private float backgroundDrawRotation;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundScreen"/> class.
        /// </summary>
        public BackgroundScreen()
        {
            this.effects = new List<RandomColorEffect>();
            for (int i = 0; i < 8; i++)
            {
                this.effects.Add(new RandomColorEffect(150, i * 0.01f + 0.01f, this.random));
            }

            this.backgroundDrawLocation = new Vector2(1920 / 2, 1080 / 2);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            float x = (float)(GameClock.Now / 123456789f);
            this.backgroundDrawRotation = (float)Math.Cos(x * Math.Sin(0.7236743534 * x)) * MathHelper.Pi + MathHelper.Pi;
            
            foreach (RandomColorEffect effect in this.effects)
            {
                effect.Update();
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            Drawer.Draw(
                TextureStatic.Get("colorSwirl"),
                this.backgroundDrawLocation,
                null,
                Color.White,
                this.backgroundDrawRotation,
                TextureStatic.GetOrigin("colorSwirl"),
                4f,
                SpriteEffects.None,
                0f);

            foreach (RandomColorEffect effect in this.effects)
            {
                effect.Draw();
            }

            base.Draw();
        }
    }
}
