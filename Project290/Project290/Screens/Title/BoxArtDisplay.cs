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
using Project290.GameElements;
using Project290.Inputs;
using Project290.Menus;

namespace Project290.Screens.Title
{
    /// <summary>
    /// Used for displaying all of the game info box arts on the title screen.
    /// </summary>
    public class BoxArtDisplay
    {
        /// <summary>
        /// Gets the box art menu.
        /// </summary>
        public BoxArtMenu boxArtMenu { get; private set; }

        /// <summary>
        /// The box arts to display.
        /// </summary>
        private List<GameInfoDisplayElement> images;

        /// <summary>
        /// The base position (positon of center of active box art).
        /// </summary>
        private Vector2 basePosition;

        /// <summary>
        /// The offset between each image.
        /// </summary>
        private Vector2 boxArtOffset;

        /// <summary>
        /// The offset for all box art displays put together.
        /// </summary>
        private Vector2 fullOffset;

        /// <summary>
        /// The last index of the game info container.
        /// </summary>
        private int lastOffset;

        /// <summary>
        /// The title screen.
        /// </summary>
        private Screen screen;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxArtDisplay"/> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        /// <param name="basePosition">The base position (positon of center of active box art).</param>
        /// <param name="boxArtOffset">The box art offset between each image.</param>
        public BoxArtDisplay(Screen screen, Vector2 basePosition, Vector2 boxArtOffset)
        {
            this.screen = screen;
            this.basePosition = basePosition;
            this.boxArtOffset = boxArtOffset;
            this.images = new List<GameInfoDisplayElement>();
            this.boxArtMenu = new BoxArtMenu(new Vector2(0, -150), new MenuAction[] { }, 80f);

            // Set each element in images to the correct starting location. 
            // Mind the "current Index" of GameInfoCollection.
            for (int i = 0; i < GameInfoCollection.GameInfos.Count * 5; i++)
            {                
                if (i % GameInfoCollection.GameInfos.Count == GameInfoCollection.GameInfos.Count - 1)
                {
                    this.images.Add(new GameInfoDisplayMenu(screen, boxArtMenu));
                }
                else
                {
                    this.images.Add(new GameInfoDisplayElement(GameInfoCollection.GameInfos[i % GameInfoCollection.GameInfos.Count], screen));
                }
            }

            this.lastOffset = GameInfoCollection.GameInfos.Count * 3;
            this.Shift(0);
            this.fullOffset = -GameInfoCollection.GameInfos.Count * boxArtOffset;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            // Update each element
            foreach (GameInfoDisplayElement element in this.images)
            {
                element.Update();
            }
        }

        /// <summary>
        /// Shifts the entries by the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void Shift(int amount)
        {
            int newAmount = this.images[this.lastOffset].GetShiftAmount(amount);
            GameInfoCollection.ShiftIndex(newAmount);

            // Shift each element in images to the correct starting location. 
            int currentOffset = this.lastOffset + newAmount;
            if (currentOffset < 2 * GameInfoCollection.GameInfos.Count)
            {
                currentOffset += GameInfoCollection.GameInfos.Count;
                for (int i = 0; i < this.images.Count; i++)
                {
                    this.images[i].Position.Set(this.images[i].Position.Value.X - this.fullOffset.X, this.images[i].Position.Value.Y - this.fullOffset.Y);
                }
            }
            else if (currentOffset > 3 * GameInfoCollection.GameInfos.Count)
            {
                currentOffset -= GameInfoCollection.GameInfos.Count;
                for (int i = 0; i < this.images.Count; i++)
                {
                    this.images[i].Position.Set(this.images[i].Position.Value.X + this.fullOffset.X, this.images[i].Position.Value.Y + this.fullOffset.Y);
                }
            }

            float layerDepth = 0.95f;
            int listIndex = currentOffset;
            int change = 1;
            bool lastMultiply = false;
            for (int i = 0; i < this.images.Count; i++)
            {
                int nowIndex = (listIndex + 1000000 * this.images.Count) % this.images.Count;
                Vector2 temp = this.basePosition + (currentOffset - nowIndex) * this.boxArtOffset;
                this.images[nowIndex].Position.GoTo(temp.X, temp.Y, 0.25f, true);
                this.images[nowIndex].LayerDepth = layerDepth;
                listIndex = currentOffset + change;
                if (!lastMultiply)
                {
                    change *= -1;
                    lastMultiply = true;
                    layerDepth -= 0.1f;
                }
                else
                {
                    change += (change < 0 ? -1 : 1);
                    lastMultiply = false;
                }
            }

            this.lastOffset = currentOffset;
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public void Draw()
        {
            foreach (GameInfoDisplayElement display in this.images)
            {
                display.Draw();
            }
        }
    }
}
