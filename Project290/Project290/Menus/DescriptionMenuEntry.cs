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
using Microsoft.Xna.Framework.Graphics;

namespace Project290.Menus
{
    /// <summary>
    /// Used for representing a menu entry with a string description.
    /// </summary>
    public class DescriptionMenuEntry : MenuEntry
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionMenuEntry"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="descripton">The descripton.</param>
        /// <param name="actions">The actions.</param>
        /// <param name="position">The position.</param>
        public DescriptionMenuEntry(string text, string descripton, MenuAction[] actions, Vector2 position)
            : base(text, actions, position)
        {
            this.Description = descripton;
            this.layerDepth = 0.96f;
        }

        /// <summary>
        /// Draws the menu entry at the specified position.
        /// </summary>
        /// <param name="offsetPosition"></param>
        /// <param name="highlighted">Specifies whether or not this menu entry
        /// is the highlighted menu entry in the list of menu entries.</param>
        public override void Draw(Vector2 offsetPosition, bool highlighted)
        {
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                this.text,
                this.position + offsetPosition,
                Color.White,
                0f,
                FontStatic.Get("defaultFont").MeasureString(this.text) / 2f,
                highlighted ? 0.9f : 0.7f,
                SpriteEffects.None,
                this.layerDepth);
        }
    }
}
