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
using Project290.Mathematics;
using Microsoft.Xna.Framework;
using Project290.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Project290.Particles;
using Project290.Screens.Shared;

namespace Project290.Screens.Title
{
    /// <summary>
    /// This class is used for displaying a single game info from the title screen.
    /// </summary>
    public class GameInfoDisplayElement
    {
        /// <summary>
        /// Gets the position of the box art.
        /// </summary>
        public tVector2 Position { get; private set; }

        /// <summary>
        /// Gets the game info.
        /// </summary>
        public GameInfo Info { get; private set; }

        /// <summary>
        /// The layer depth.
        /// </summary>
        /// <value>
        /// The layer depth.
        /// </value>
        public float LayerDepth { get; set; }

        /// <summary>
        /// Used for displaying the background.
        /// </summary>
        private HypercubeDisplay background;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInfoDisplayElement"/> class.
        /// </summary>
        /// <param name="gameInfo">The game info.</param>
        /// <param name="screen">The screen.</param>
        public GameInfoDisplayElement(GameInfo gameInfo, Screen screen)
        {
            this.Position = new tVector2(-500, -500);
            this.Info = gameInfo;

            if (string.IsNullOrEmpty(gameInfo.BoxArt))
            {
                this.background = new HypercubeDisplay(
                    new Rectangle((int)(this.Position.Value.X - 584f / 2f), (int)(this.Position.Value.Y - 700f / 2f), 584, 700),
                    3,
                    screen.random,
                    this.LayerDepth);
            }
        }

        /// <summary>
        /// Gets the shift amount.
        /// </summary>
        /// <param name="amount">The amount of shift requested.</param>
        /// <returns>The amount to shift the menu system by.</returns>
        public virtual int GetShiftAmount(int amount)
        {
            return amount;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public virtual void Update()
        {
            this.Position.Update();

            if (this.background != null)
            {
                this.background.SetLayerDepth(this.LayerDepth);
                this.background.Set(this.Position.X.Value, this.Position.Y.Value);
                this.background.Update();
            }
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public virtual void Draw()
        {
            if (!string.IsNullOrEmpty(this.Info.BoxArt))
            {
                Drawer.Draw(
                    TextureStatic.Get("BoxArtHolder"),
                    this.Position.Value,
                    null,
                    Color.White,
                    0f,
                    TextureStatic.GetOrigin("BoxArtHolder"),
                    1f,
                    SpriteEffects.None,
                    this.LayerDepth - 0.04f);

                Drawer.Draw(
                    TextureStatic.Get(this.Info.BoxArt),
                    this.Position.Value,
                    null,
                    Color.White,
                    0f,
                    TextureStatic.GetOrigin(this.Info.BoxArt),
                    1f,
                    SpriteEffects.None,
                    this.LayerDepth);
            }
            else
            {
                Drawer.Draw(
                    TextureStatic.Get("BoxArtHolder"),
                    this.Position.Value,
                    null,
                    Color.White,
                    0f,
                    TextureStatic.GetOrigin("BoxArtHolder"),
                    1f,
                    SpriteEffects.None,
                    this.LayerDepth + 0.04f);

                this.background.Draw();
            }
        }
    }
}
