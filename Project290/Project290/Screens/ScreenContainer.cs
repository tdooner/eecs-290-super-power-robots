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
using Project290.Screens;
using Project290.Inputs;
using Project290.GameElements;
using Project290.Screens.Title;
using Microsoft.Xna.Framework.GamerServices;

namespace Project290.Screens
{
    /// <summary>
    /// This is a stack of screens used in the game.
    /// Do not Add or Remove screens yourself. Use the "Play"
    /// method to pop a new screen on the stack, and if you want
    /// to remove a screen, so the GameScreen.Disposed value
    /// to true for a screen and it will get removed for you.
    /// </summary>
    public class ScreenContainer : List<Screen>
    {
        /// <summary>
        /// The "start" index for the layer depth rendering (the lowest value).
        /// </summary>
        public static float LayerDepthStart;

        /// <summary>
        /// The "end" index for the layer depth rendering (the highest value).
        /// </summary>
        public static float LayerDepthEnd;

        /// <summary>
        /// Gets a value indicating whether the game is paused.
        /// </summary>
        /// <value><c>true</c> if this instance is paused; otherwise, <c>false</c>.</value>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// This is the screen that will be added as soon as this instance updates.
        /// </summary>
        private List<Screen> toAdd;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenContainer"/> class.
        /// </summary>
        public ScreenContainer()
            : base()
        {
            this.IsPaused = false;
            this.toAdd = new List<Screen>();
        }

        /// <summary>
        /// Plays the specified Screen.
        /// This will not happen until Update is called.
        /// </summary>
        /// <param name="toAdd">To new GameScreen to be added.</param>
        public void Play(Screen toAdd)
        {
            this.toAdd.Add(toAdd);
        }

        /// <summary>
        /// Adds the screen in "toAdd" to the stack.
        /// </summary>
        private void Add()
        {
            foreach (Screen screen in this.toAdd)
            {
                this.Add(screen);
            }

            this.toAdd.Clear();
        }

        /// <summary>
        /// Finds all screens which have the value of
        /// "Disposed" set to true and removes them.
        /// </summary>
        private void Kill()
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (this[i].Disposed)
                {
                    if (this[i] is PauseScreen)
                    {
                        this.IsPaused = false;
                    }

                    this.Remove(this[i]);
                }
            }
        }

        /// <summary>
        /// Removes all screens from this stack. Update
        /// must be called after this for this to take effect.
        /// </summary>
        public void KillAll()
        {
            foreach (Screen screen in this)
            {
                screen.Disposed = true;
            }
        }

        /// <summary>
        /// Pauses the game and adds a Pause Screen to the stack.
        /// </summary>
        public void Pause()
        {
            // Do this only if not on the title screen.
            foreach (Screen screen in this)
            {
                if (screen is TitleScreen || screen is StartScreen || screen is GameOverScreen || screen is PauseScreen)
                {
                    return;
                }
            }

            this.Add(new PauseScreen());
            this.IsPaused = true;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.Kill();
            this.Add();

            // Update the screens from top to bottom, stopping when a
            // screen is found that is not "fading out".
            for (int i = Count - 1; i >= 0; i--)
            {
                this[i].Update();
                if (!this[i].FadingOut)
                {
                    break;
                }
            }

            // Check if the game is being paused, and there is no pause screen on the stack.
            if (!this.IsPaused && (GameWorld.controller.ContainsBool(ActionType.Pause) || Guide.IsVisible))
            {
                this.Pause();
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            if (this.Count > 0)
            {
                float layerDepthOffset = 0.8f / this.Count;
                LayerDepthStart = 0.199f;
                foreach (Screen screen in this)
                {
                    LayerDepthEnd = LayerDepthStart + layerDepthOffset - 0.0001f;
                    screen.Draw();
                    LayerDepthStart = LayerDepthEnd + 0.0001f;
                }
            }
        }
    }
}