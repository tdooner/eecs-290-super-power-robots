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
using Project290.Menus;
using Microsoft.Xna.Framework;
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Project290.Screens.Title
{
    /// <summary>
    /// A "box art" that is actually a menu.
    /// </summary>
    public class GameInfoDisplayMenu : GameInfoDisplayElement
    {
        /// <summary>
        /// The menu.
        /// </summary>
        private BoxArtMenu menu;

        /// <summary>
        /// What was the index of the previous game info before this one was selected?
        /// </summary>
        private int previousGameInfoSelected;

        /// <summary>
        /// The corresopnding screen.
        /// </summary>
        private Screen screen;

        /// <summary>
        /// Lag a tiny bit to prevent "double-scrolling".
        /// </summary>
        private long lagOverTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInfoDisplayMenu"/> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        public GameInfoDisplayMenu(Screen screen, BoxArtMenu menu)
            : base(GameInfoCollection.GameInfos[GameInfoCollection.GameInfos.Count - 1], screen)
        {
            this.screen = screen;
            this.menu = menu;
            this.previousGameInfoSelected = GameInfoCollection.CurrentIndex;
            this.lagOverTime = 0;
        }

        /// <summary>
        /// Gets the shift amount.
        /// </summary>
        /// <param name="amount">The amount of shift requested.</param>
        /// <returns>
        /// The amount to shift the box art menu system by.
        /// </returns>
        public override int GetShiftAmount(int amount)
        {
            int temp = 0;
            if (amount < 0)
            {
                temp = this.menu.CurrentSelected + amount;
                if (temp < 0)
                {
                    return temp;
                }
            }
            else if (amount > 0)
            {
                temp = amount + this.menu.CurrentSelected - this.menu.Count + 1;
                if (temp > 0)
                {
                    return temp;
                }
            }

            return 0;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            if (this.screen.IsOnTop())
            {
                if (GameInfoCollection.CurrentIndex == GameInfoCollection.GameInfos.Count - 1
                    && this.Position.Value.Y > 0 && this.Position.Value.Y < 1080) // This last part is a bit of a hack.
                {
                    this.menu.position = this.Position.Value;

                    if (DateTime.Now.Ticks > this.lagOverTime)
                    {
                        this.menu.Update();
                    }

                    if (this.previousGameInfoSelected != GameInfoCollection.GameInfos.Count - 1)
                    {
                        if (this.previousGameInfoSelected == 0)
                        {
                            this.menu.TrySet(this.menu[this.menu.Count - 1]);
                        }
                        else
                        {
                            this.menu.TrySet(this.menu[0]);
                        }
                    }
                }
                else
                {
                    this.lagOverTime = DateTime.Now.Ticks + 1000000;
                }

                this.previousGameInfoSelected = GameInfoCollection.CurrentIndex;
            }

            base.Update();
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            if (GameInfoCollection.CurrentIndex == GameInfoCollection.GameInfos.Count - 1)
            {
                this.menu.Draw();
            }

            base.Draw();
        }
    }
}
