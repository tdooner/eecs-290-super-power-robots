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
using Microsoft.Xna.Framework;
using Project290.Rendering;
using Project290.Mathematics;
using Project290.GameElements;
using Project290.Inputs;
using Microsoft.Xna.Framework.Graphics;
using Project290.Screens.Shared;

namespace Project290.Screens
{
    /// <summary>
    /// Used to display game credits.
    /// </summary>
    public class CreditsScreen : Screen
    {
        /// <summary>
        /// Used for displaying the background.
        /// </summary>
        private HypercubeDisplay background;

        /// <summary>
        /// The position of the image.
        /// </summary>
        private tVector2 position;

        /// <summary>
        /// Should this be disposed whe the transition stops?
        /// </summary>
        private bool shouldDispose;

        /// <summary>
        /// The credits to display.
        /// </summary>
        private List<string> display;

        /// <summary>
        /// The verticle spacings for all lines in the credits.
        /// </summary>
        private List<Vector2> verticleSpacing;

        /// <summary>
        /// The scrolling credits position.
        /// </summary>
        private tVector2 scrollPosition;

        /// <summary>
        /// The origin of "Credits"
        /// </summary>
        private Vector2 titleOrigin;

        /// <summary>
        /// The viewport for constraining the text within the rectangle.
        /// </summary>
        private Viewport viewport;

        /// <summary>
        /// The previous position X value of the rectangle.
        /// </summary>
        private float previousPositionX;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditsScreen"/> class.
        /// </summary>
        public CreditsScreen()
            : base()
        {
            this.shouldDispose = false;
            this.FadingOut = true;
            this.position = new tVector2(1920f / 2f - 2000f, 1080f / 2f);
            this.position.GoTo(1920f / 2f, 1080f / 2f, 0.3f, true);
            this.background = new HypercubeDisplay(
                new Rectangle((int)(this.position.Value.X - 1450f / 2f) - 3, (int)(this.position.Value.Y - 800f / 2f) - 3, 1450 + 6, 800 + 6),
                3,
                this.random,
                0.2f);
            this.scrollPosition = new tVector2(0, 1000);
            this.scrollPosition.GoTo(0, -21000, 82f, true);
            this.titleOrigin = FontStatic.Get("defaultFont").MeasureString("Credits") / 2f;
            this.viewport = new Viewport(
                (int)((this.position.Value.X - 1450f / 2f) * Drawer.GetRatio()), 
                (int)((this.position.Value.Y - 800f / 2f) * Drawer.GetRatio()),
                (int)(1450 * Drawer.GetRatio()),
                (int)(800 * Drawer.GetRatio()));
            this.previousPositionX = 0;
            string fullText =
@"Ty Taylor
    Supreme Overlord of the Universe
    Framework and Game Engine

Marc Buchner
    Assistant to Mr. Taylor

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Some guy
    Stuff and things

Copyright 2011
Case Western Reserve University
http://eecs.case.edu";

            this.display = fullText.Split('\n').ToList<string>();
            float unitSpacing = FontStatic.Get("defaultFont").MeasureString(" ").Y;
            this.verticleSpacing = new List<Vector2>(this.display.Count);
            for (int i = 0; i < this.display.Count; i++)
            {
                this.verticleSpacing.Add(new Vector2(-800, 300 + i * unitSpacing));
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (this.shouldDispose && !this.position.IsTransitioning)
            {
                this.Disposed = true;
            }

            if (GameWorld.controller.ContainsBool(ActionType.GoBack))
            {
                if (!this.shouldDispose)
                {
                    this.position.GoTo(1920f / 2f + 2000f, 1080f / 2f, 0.3f);
                    this.shouldDispose = true;
                    GameWorld.audio.PlaySound("menuGoBack");
                }
            }

            this.position.Update();
            this.scrollPosition.Update();

            if (!this.scrollPosition.IsTransitioning)
            {
                this.scrollPosition.Set(0, 1000);
                this.scrollPosition.GoTo(0, -21000, 82f, true);
            }

            this.background.Set(this.position.X.Value, this.position.Y.Value);
            this.background.Update();

            // Only change the viewport if the position has changed so the graphics card doesn't get angry.
            if (this.previousPositionX != this.position.X.Value)
            {
                this.viewport.X = (int)(MathHelper.Clamp(this.position.Value.X - 1450f / 2f, 0, 1920) * Drawer.GetRatio());
                this.viewport.Width = (int)(MathHelper.Clamp(1920f - (this.position.Value.X - 1450f / 2f), 0f, 1450f) * Drawer.GetRatio());
            }

            this.previousPositionX = this.position.X.Value;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            this.background.Draw();
            Drawer.Draw(
                TextureStatic.Get("instructionBorder"),
                this.position.Value,
                null,
                Color.White,
                0f,
                TextureStatic.GetOrigin("instructionBorder"),
                1f,
                SpriteEffects.None,
                0.3f);

            if (this.viewport.Width > 0)
            {
                GameWorld.spriteBatch.End();
                Viewport previousViewport = GameWorld.spriteBatch.GraphicsDevice.Viewport;
                GameWorld.spriteBatch.GraphicsDevice.Viewport = this.viewport;
                GameWorld.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                Drawer.DrawOutlinedString(
                    FontStatic.Get("defaultFont"),
                    "Credits",
                    this.position.Value + this.scrollPosition.Value,
                    Color.White,
                    0f,
                    this.titleOrigin,
                    1.3f,
                    SpriteEffects.None,
                    0.8f);
                for (int i = 0; i < this.display.Count; i++)
                {
                    // Only draw if it is going to be on the screen...
                    Vector2 temp = this.position.Value + this.scrollPosition.Value + this.verticleSpacing[i];
                    if (temp.Y > -300 && temp.Y < 2220)
                    {
                        Drawer.DrawOutlinedString(
                            FontStatic.Get("defaultFont"),
                            this.display[i],
                            temp,
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1f,
                            SpriteEffects.None,
                            0.8f);
                    }
                }

                GameWorld.spriteBatch.End();
                GameWorld.spriteBatch.GraphicsDevice.Viewport = previousViewport;
                GameWorld.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            }
        }
    }
}
