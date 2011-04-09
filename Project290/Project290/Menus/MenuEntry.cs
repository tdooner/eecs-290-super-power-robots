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
using Project290.Mathematics;

namespace Project290.Menus
{
    /// <summary>
    /// This is an instance of an individual entry in a menu.
    /// </summary>
    public class MenuEntry
    {
        /// <summary>
        /// Gets the text to display for this entry.
        /// </summary>
        /// <value>The text to display.</value>
        public string text { get; private set; }

        /// <summary>
        /// Gets or sets the upper menu, which is the menu entry that will
        /// be highlighted if this menu entry is highlighted and the player
        /// presses Up. If this is null, then no action will be taken.
        /// </summary>
        /// <value>The upper menu.</value>
        public MenuEntry UpperMenu { get; set; }

        /// <summary>
        /// Gets or sets the lower menu, which is the menu entry that will
        /// be highlighted if this menu entry is highlighted and the player
        /// presses Down. If this is null, then no action will be taken.
        /// </summary>
        /// <value>The lower menu.</value>
        public MenuEntry LowerMenu { get; set; }

        /// <summary>
        /// Gets or sets the right menu, which is the menu entry that will
        /// be highlighted if this menu entry is highlighted and the player
        /// presses Right. If this is null, then no action will be taken.
        /// </summary>
        /// <value>The right menu.</value>
        public MenuEntry RightMenu { get; set; }

        /// <summary>
        /// Gets or sets the left menu, which is the menu entry that will
        /// be highlighted if this menu entry is highlighted and the player
        /// presses Left. If this is null, then no action will be taken.
        /// </summary>
        /// <value>The left menu.</value>
        public MenuEntry LeftMenu { get; set; }

        /// <summary>
        /// Gets the position of the menu entry.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 position;

        /// <summary>
        /// The layer depth of the menu entry.
        /// </summary>
        public float layerDepth;

        /// <summary>
        /// Gets the actions associated with this menu entry. For example,
        /// one action might be pressing "A" to select the entry. Adding
        /// an action of Left, Right, Up, or Down will overwrite the 
        /// menu entry navigation.
        /// </summary>
        /// <value>The actions.</value>
        public MenuAction[] actions { get; internal set; }

        /// <summary>
        /// The origin of the image.
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuEntry"/> class.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="actions">The this.actions associated with this menu entry. For example,
        /// one action might be pressing "A" to select the entry. Adding
        /// an action of Left, Right, Up, or Down will overwrite the
        /// menu entry navigation.</param>
        /// <param name="position">The this.position of the image.</param>
        public MenuEntry(string text, MenuAction[] actions, Vector2 position)
        {
            this.text = text;
            this.actions = actions;
            this.layerDepth = 1;
            this.position = position;
            this.origin = FontStatic.Get("defaultFont").MeasureString(text) / 2f;
        }

        /// <summary>
        /// Updates the specified highlighted.
        /// </summary>
        /// <param name="highlighted">Specifies whether or not this menu entry
        /// is the highlighted menu entry in the list of menu entries.</param>
        public virtual void Update(bool highlighted)
        {
            if (highlighted)
            {
                if (this.actions != null)
                {
                    foreach (MenuAction action in this.actions)
                    {
                        if (action != null)
                        {
                            action.TryRunDelegate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws the specified highlighted.
        /// </summary>
        /// <param name="offsetPosition">The offset position.</param>
        /// <param name="highlighted">Specifies whether or not this menu entry
        /// is the highlighted menu entry in the list of menu entries.</param>
        public virtual void Draw(Vector2 offsetPosition, bool highlighted)
        {
            Drawer.DrawOutlinedString(
                FontStatic.Get("defaultFont"),
                this.text,
                this.position + offsetPosition,
                Color.White,
                0f,
                this.origin, 
                (highlighted ? 0.9f : 0.7f),
                SpriteEffects.None, 
                this.layerDepth);
        }
    }
}