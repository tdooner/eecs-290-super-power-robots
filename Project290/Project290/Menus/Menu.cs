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
using Project290.Mathematics;
using Project290.Inputs;

namespace Project290.Menus
{
    /// <summary>
    /// This is a set of Menu Entries, with the ability to scroll
    /// through the menu entries, each having a set of delegated this.actions.
    /// </summary>
    public class Menu : List<MenuEntry>
    {
        /// <summary>
        /// Gets the currently selected menu entry. This is the index
        /// of the menu entry that is highlighted.
        /// </summary>
        /// <value>The currently highlighted menu entry.</value>
        public int CurrentSelected { get; private set; }

        /// <summary>
        /// The base this.position of the entire set of menu entries.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 position;

        /// <summary>
        /// The set of this.actions corresponding the the menu system
        /// as a whole. For example, pressing B to exit the menu
        /// instead of clicking a "back" menu entry would be an
        /// appropriate action to add.
        /// </summary>
        private MenuAction[] actions;

        /// <summary>
        /// A variable used for optimization, so that the list of
        /// this.actions does not need to be looped through every cycle.
        /// </summary>
        private bool containsUpAction;

        /// <summary>
        /// A variable used for optimization, so that the list of
        /// this.actions does not need to be looped through every cycle.
        /// </summary>
        private bool containsDownAction;

        /// <summary>
        /// A variable used for optimization, so that the list of
        /// this.actions does not need to be looped through every cycle.
        /// </summary>
        private bool containsRightAction;

        /// <summary>
        /// A variable used for optimization, so that the list of
        /// this.actions does not need to be looped through every cycle.
        /// </summary>
        private bool containsLeftAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="position">The this.position of the menu system.</param>
        /// <param name="actions">The set of this.actions corresponding the the menu system
        /// as a whole. For example, pressing B to exit the menu
        /// instead of clicking a "back" menu entry would be an
        /// appropriate action to add.</param>
        public Menu(Vector2 position, MenuAction[] actions)
        {
            this.position = new Vector2(position.X, position.Y);
            this.CurrentSelected = 0;
            this.actions = actions;

            this.containsDownAction = false;
            this.containsLeftAction = false;
            this.containsRightAction = false;
            this.containsUpAction = false;

            // Fill the value of the four variables above if 
            // a corresponding action is found in the array.
            if (this.actions != null)
            {
                foreach (MenuAction action in this.actions)
                {
                    if (action != null)
                    {
                        if (action.actionType == ActionType.SelectionDown)
                        {
                            this.containsDownAction = true;
                        }

                        if (action.actionType == ActionType.SelectionUp)
                        {
                            this.containsUpAction = true;
                        }

                        if (action.actionType == ActionType.SelectionRight)
                        {
                            this.containsRightAction = true;
                        }

                        if (action.actionType == ActionType.SelectionLeft)
                        {
                            this.containsLeftAction = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If the associated menu entry is not null, and
        /// exists in the list, the index of the currently 
        /// selected menu entry is changed to the index
        /// of the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void TrySet(MenuEntry entry)
        {
            if (entry != null)
            {
                int temp = this.CurrentSelected;
                this.CurrentSelected = this.IndexOf(entry);

                if (this.CurrentSelected == -1)
                {
                    this.CurrentSelected = temp;
                }
            }
        }

        /// <summary>
        /// Call this method when the user makes a left selection.
        /// If the current menu entry has a left menu entry link associated
        /// with it, that linked menu entry will be the new highlighted
        /// menu entry.
        /// </summary>
        public void ToggleLeft()
        {
            this.TrySet(this[this.CurrentSelected].LeftMenu);
        }

        /// <summary>
        /// Call this method when the user makes a right selection.
        /// If the current menu entry has a right menu entry link associated
        /// with it, that linked menu entry will be the new highlighted
        /// menu entry.
        /// </summary>
        public void ToggleRight()
        {
            this.TrySet(this[this.CurrentSelected].RightMenu);
        }

        /// <summary>
        /// Call this method when the user makes a up selection.
        /// If the current menu entry has a up menu entry link associated
        /// with it, that linked menu entry will be the new highlighted
        /// menu entry.
        /// </summary>
        public void ToggleUp()
        {
            this.TrySet(this[this.CurrentSelected].UpperMenu);
            GameWorld.audio.PlaySound("menuScrollUp");
        }

        /// <summary>
        /// Call this method when the user makes a down selection.
        /// If the current menu entry has a down menu entry link associated
        /// with it, that linked menu entry will be the new highlighted
        /// menu entry.
        /// </summary>
        public void ToggleDown()
        {
            this.TrySet(this[this.CurrentSelected].LowerMenu);
            GameWorld.audio.PlaySound("menuScrollDown");
        }

        /// <summary>
        /// Updates this instance. This checks to see if the user has made an
        /// up, down, left, or right selection, and if so, and if the set of
        /// action delegates does not overwrite this, it tries to change the
        /// highlighted menu entry accordingly. This also runs menu entry
        /// delegate this.actions when they are pressed and updates each menu
        /// entry individually.
        /// </summary>
        public virtual void Update()
        {
            if (!this.containsUpAction)
            {
                if (GameWorld.controller.ContainsBool(ActionType.SelectionUp))
                {
                    this.ToggleUp();
                }
            }

            if (!this.containsDownAction)
            {
                if (GameWorld.controller.ContainsBool(ActionType.SelectionDown))
                {
                    this.ToggleDown();
                }
            }

            if (!this.containsLeftAction)
            {
                if (GameWorld.controller.ContainsBool(ActionType.SelectionLeft))
                {
                    this.ToggleLeft();
                }
            }

            if (!this.containsRightAction)
            {
                if (GameWorld.controller.ContainsBool(ActionType.SelectionRight))
                {
                    this.ToggleRight();
                }
            }

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

            foreach (MenuEntry entry in this)
            {
                entry.Update(this.IndexOf(entry) == this.CurrentSelected);
            }
        }

        /// <summary>
        /// Draws each menu entry in the set.
        /// </summary>
        public virtual void Draw()
        {
            foreach (MenuEntry entry in this)
            {
                entry.Draw(this.position, this.IndexOf(entry) == this.CurrentSelected);
            }
        }
    }
}
