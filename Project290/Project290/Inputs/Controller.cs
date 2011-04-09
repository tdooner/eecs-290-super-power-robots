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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Project290.GameElements;

namespace Project290.Inputs
{
    /// <summary>
    /// Used to interact with the gamepad controller. This method reports a list 
    /// of Actions that the player performed based on what buttons were pressed.
    /// These actions may or may not have anything to do with the game world; 
    /// they only have to do with the controller and the timing and sequence of the presses.
    /// </summary>
    public class Controller
    {
        #region Timers
        /// <summary>
        /// The current update cycle.
        /// </summary>
        private int updateCycle;

        /// <summary>
        /// The last time the movement from the controller.
        /// </summary>
        private int lastSelectionMovement;

        /// <summary>
        /// The last time select was pressed.
        /// </summary>
        private int lastSelect;

        /// <summary>
        /// Records the previous update.
        /// </summary>
        private int previousUpdate;
        #endregion
        
        #region PressRecorders
        /// <summary>
        /// This is used to recored if the previous controller configuration contained this action.
        /// </summary>
        private bool previousSelectDown;

        /// <summary>
        /// This is used to recored if the previous controller configuration contained this action.
        /// </summary>
        private bool previousSelectUp;

        /// <summary>
        /// This is used to recored if the previous controller configuration contained this action.
        /// </summary>
        private bool previousSelectRight;

        /// <summary>
        /// This is used to recored if the previous controller configuration contained this action.
        /// </summary>
        private bool previousSelectLeft;

        /// <summary>
        /// This is used to recored if the previous controller configuration contained this action.
        /// </summary>
        private bool previousSelect;

        /// <summary>
        /// This is used to recored if the previous controller configuration contained this action.
        /// </summary>
        private bool previousPause;

        /// <summary>
        /// This is used to recored if the previous controller configuration contained this action.
        /// </summary>
        private bool previousGoBack;

        /// <summary>
        /// Was A previously pressed?
        /// </summary>
        private bool previousA;

        /// <summary>
        /// Was B previously pressed?
        /// </summary>
        private bool previousB;

        /// <summary>
        /// Was X previously pressed?
        /// </summary>
        private bool previousX;

        /// <summary>
        /// Was Y previously pressed?
        /// </summary>
        private bool previousY;

        /// <summary>
        /// Was Right Bumper previously pressed?
        /// </summary>
        private bool previousRB;

        /// <summary>
        /// Was Left Bumper previously pressed?
        /// </summary>
        private bool previousLB;
        #endregion

        /// <summary>
        /// The ulong containing the pressed button states.
        /// </summary>
        private ControllerPacket packet;

        /// <summary>
        /// Gets the index of the player for this controller.
        /// </summary>
        /// <value>The index of the player.</value>
        public PlayerIndex playerIndex { get; private set; }

        /// <summary>
        /// Gets the gamer.
        /// </summary>
        public Gamer gamer { get; private set; }

        /// <summary>
        /// The rumble pack for the controller.
        /// </summary>
        private RumblePack rumblePack;

        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        public Controller(PlayerIndex playerIndex)
        {
            this.packet = new ControllerPacket();
            this.playerIndex = playerIndex;
            this.rumblePack = new RumblePack(this.playerIndex);

            #region TimerInitiallization
            this.updateCycle = 0;
            this.previousUpdate = 0;
            this.lastSelectionMovement = 0;
            this.lastSelect = 0;
            #endregion

            #region PreviousRecorderInitiallization
            this.previousPause = false;
            this.previousSelect = false;
            this.previousSelectDown = false;
            this.previousSelectLeft = false;
            this.previousSelectRight = false;
            this.previousSelectUp = false;
            this.previousGoBack = false;
            this.previousA = false;
            this.previousB = false;
            this.previousX = false;
            this.previousY = false;
            this.previousRB = false;
            this.previousLB = false;
            #endregion
        }

        /// <summary>
        /// Checks all controller that are plugged in and and if that controller
        /// is pressing a button, it sets it as the active controller.
        /// </summary>
        private void CheckAndSetActive()
        {
            if (this.gamer != null && this.gamer.IsDisposed || this.gamer is SignedInGamer)
            {
                this.gamer = null;
            }

            if (this.gamer is SignedInGamer && !this.gamer.IsDisposed && (this.gamer as SignedInGamer).PlayerIndex != this.playerIndex)
            {
                this.gamer = null;
            }

            foreach (SignedInGamer sig in SignedInGamer.SignedInGamers)
            {
                if (sig.PlayerIndex == this.playerIndex)
                {
                    this.gamer = sig;
                    return;
                }
            }            
        }
        
        /// <summary>
        /// Sets the index of the controller for the player on the xbox.
        /// </summary>
        /// <param name="index">The index of the controller.</param>
        public void SetPlayerIndex(PlayerIndex index)
        {
            this.playerIndex = index;
            this.rumblePack.playerIndex = index;
        }

        /// <summary>
        /// Updates the Contoller method, saving the conroller state and incrementing time.
        /// It is important that this is only called once per game Update, and this should be called first.
        /// </summary>
        public void Update()
        {
            this.CheckAndSetActive();
            this.rumblePack.Update();

            #region TimerUpdate
            this.updateCycle++;
            this.lastSelectionMovement++;
            this.lastSelect++;
            #endregion
        }

        /// <summary>
        /// Returns a list of actions that the player has performed based on
        /// the controller input at the instance that this method is called.
        /// </summary>
        private void GetActions()
        {
            // Check to make sure this isn't called twice per update
            if (this.previousUpdate == this.updateCycle)
            {
                return;
            }

            this.packet.Clear();

            if (Guide.IsVisible)
            {
                return;
            }

            GamePadState gamePadState = GamePad.GetState(this.playerIndex);

#if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();
#endif

            #region SelectionDOWN

#if !XBOX
            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.PageDown))
                && (this.lastSelectionMovement > 15 || !this.previousSelectDown))
            {
                this.packet.SelectionDown = true;
                this.lastSelectionMovement = 0;
                this.previousSelectDown = true;
            }

            if (!gamePadState.IsConnected &&
                keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.PageDown))
            {
                this.previousSelectDown = false;
            }
#endif

            if ((gamePadState.IsButtonDown(Buttons.DPadDown) ||
                gamePadState.ThumbSticks.Left.Y < -0.7f ||
                gamePadState.ThumbSticks.Right.Y < -0.7f) &&
                (this.lastSelectionMovement > 15 || !this.previousSelectDown))
            {
                this.packet.SelectionDown = true;
                this.lastSelectionMovement = 0;
                this.previousSelectDown = true;
            }

            if (
#if !XBOX
                gamePadState.IsConnected && 
#endif
                gamePadState.IsButtonUp(Buttons.DPadDown) &&
                gamePadState.ThumbSticks.Left.Y >= -0.7f &&
                gamePadState.ThumbSticks.Right.Y >= -0.7f)
            {
                this.previousSelectDown = false;
            }

            #endregion

            #region SelectionUP

#if !XBOX
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.PageUp))
                && (this.lastSelectionMovement > 15 || !this.previousSelectUp))
            {
                this.packet.SelectionUp = true;
                this.lastSelectionMovement = 0;
                this.previousSelectUp = true;
            }

            if (!gamePadState.IsConnected &&
                keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.PageUp))
            {
                this.previousSelectUp = false;
            }
#endif

            if ((gamePadState.IsButtonDown(Buttons.DPadUp) ||
                gamePadState.ThumbSticks.Left.Y > 0.7f ||
                gamePadState.ThumbSticks.Right.Y > 0.7f) &&
                (this.lastSelectionMovement > 15 || !this.previousSelectUp))
            {
                this.packet.SelectionUp = true;
                this.lastSelectionMovement = 0;
                this.previousSelectUp = true;
            }

            if (
#if !XBOX
                gamePadState.IsConnected && 
#endif
                gamePadState.IsButtonUp(Buttons.DPadUp) &&
                gamePadState.ThumbSticks.Left.Y <= 0.7f &&
                gamePadState.ThumbSticks.Right.Y <= 0.7f)
            {
                this.previousSelectUp = false;
            }

            #endregion

            #region SelectionRIGHT

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Right)
                && (this.lastSelectionMovement > 15 || !this.previousSelectRight))
            {
                this.packet.SelectionRight = true;
                this.lastSelectionMovement = 0;
                this.previousSelectRight = true;
            }

            if (!gamePadState.IsConnected &&
                keyboardState.IsKeyUp(Keys.Right))
            {
                this.previousSelectRight = false;
            }
#endif

            if ((gamePadState.IsButtonDown(Buttons.DPadRight) ||
                gamePadState.ThumbSticks.Left.X > 0.7f ||
                gamePadState.ThumbSticks.Right.X > 0.7f) &&
                (this.lastSelectionMovement > 15 || !this.previousSelectRight))
            {
                this.packet.SelectionRight = true;
                this.lastSelectionMovement = 0;
                this.previousSelectRight = true;
            }

            if (
#if !XBOX
                gamePadState.IsConnected && 
#endif
                gamePadState.IsButtonUp(Buttons.DPadRight) &&
                gamePadState.ThumbSticks.Left.X <= 0.7f &&
                gamePadState.ThumbSticks.Right.X <= 0.7f)
            {
                this.previousSelectRight = false;
            }
            #endregion

            #region SelectionLEFT
            
#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Left)
                && (this.lastSelectionMovement > 15 || !this.previousSelectLeft))
            {
                this.packet.SelectionLeft = true;
                this.lastSelectionMovement = 0;
                this.previousSelectLeft = true;
            }

            if (!gamePadState.IsConnected &&
                keyboardState.IsKeyUp(Keys.Left))
            {
                this.previousSelectLeft = false;
            }
#endif

            if ((gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                gamePadState.ThumbSticks.Left.X < -0.7f ||
                gamePadState.ThumbSticks.Right.X < -0.7f) &&
                (this.lastSelectionMovement > 15 || !this.previousSelectLeft))
            {
                this.packet.SelectionLeft = true;
                this.lastSelectionMovement = 0;
                this.previousSelectLeft = true;
            }

            if (
#if !XBOX
                gamePadState.IsConnected && 
#endif
                gamePadState.IsButtonUp(Buttons.DPadLeft) &&
                gamePadState.ThumbSticks.Left.X >= -0.7f &&
                gamePadState.ThumbSticks.Right.X >= -0.7f)
            {
                this.previousSelectLeft = false;
            }
            #endregion

            #region Select
            
            if (gamePadState.IsButtonDown(Buttons.A)
                && (this.lastSelect > 15 || !this.previousSelect))
            {
                this.packet.Select = true;
                this.lastSelect = 0;
                this.previousSelect = true;
            }

            if (
#if !XBOX
                gamePadState.IsConnected && 
#endif
                gamePadState.IsButtonUp(Buttons.A))
            {
                this.previousSelect = false;
            }

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Enter)
                && (this.lastSelect > 15 || !this.previousSelect))
            {
                this.packet.Select = true;
                this.lastSelect = 0;
                this.previousSelect = true;
            }

            if (!gamePadState.IsConnected &&
                keyboardState.IsKeyUp(Keys.Enter))
            {
                this.previousSelect = false;
            }
#endif
            #endregion

            #region GoBack
            
            if (gamePadState.IsButtonDown(Buttons.B)
                && !this.previousGoBack)
            {
                this.packet.GoBack = true;
                this.previousGoBack = true;
            }

            if (
#if !XBOX
                gamePadState.IsConnected && 
#endif
                gamePadState.IsButtonUp(Buttons.B))
            {
                this.previousGoBack = false;
            }

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Back)
                && !this.previousGoBack)
            {
                this.packet.GoBack = true;
                this.lastSelect = 0;
                this.previousGoBack = true;
            }

            if (!gamePadState.IsConnected &&
                (keyboardState.IsKeyUp(Keys.Back) &&
                    keyboardState.IsKeyUp(Keys.Left)))
            {
                this.previousGoBack = false;
            }
#endif
            #endregion

            #region Pause
                        
#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Escape) && !this.previousPause)
            {
                this.packet.Pause = true;
                this.previousPause = true;
            }

            if (!gamePadState.IsConnected &&
                keyboardState.IsKeyUp(Keys.Escape))
            {
                this.previousPause = false;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.Start) && !this.previousPause)
            {
                this.packet.Pause = true;
                this.previousPause = true;
            }

            if (
#if !XBOX
                gamePadState.IsConnected && 
#endif
                gamePadState.IsButtonUp(Buttons.Start))
            {
                this.previousPause = false;
            }

            #endregion

            #region AButton

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                this.packet.AButton = true;

                if (!this.previousA)
                {
                    this.packet.AButtonFirst = true;
                }

                this.previousA = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.A))
            {
                this.packet.AButton = true;

                if (!this.previousA)
                {
                    this.packet.AButtonFirst = true;
                }

                this.previousA = true;
            }

            if (!this.packet.AButton)
            {
                this.previousA = false;
            }

            #endregion

            #region YButton

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.RightControl))
            {
                this.packet.YButton = true;

                if (!this.previousY)
                {
                    this.packet.YButtonFirst = true;
                }

                this.previousY = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.Y))
            {
                this.packet.YButton = true;

                if (!this.previousY)
                {
                    this.packet.YButtonFirst = true;
                }

                this.previousY = true;
            }

            if (!this.packet.YButton)
            {
                this.previousY = false;
            }

            #endregion

            #region XButton

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.RightAlt))
            {
                this.packet.XButton = true;

                if (!this.previousX)
                {
                    this.packet.XButtonFirst = true;
                }

                this.previousX = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.X))
            {
                this.packet.XButton = true;

                if (!this.previousX)
                {
                    this.packet.XButtonFirst = true;
                }

                this.previousX = true;
            }

            if (!this.packet.XButton)
            {
                this.previousX = false;
            }

            #endregion

            #region BButton

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.RightShift))
            {
                this.packet.BButton = true;

                if (!this.previousB)
                {
                    this.packet.BButtonFirst = true;
                }

                this.previousB = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.B))
            {
                this.packet.BButton = true;

                if (!this.previousB)
                {
                    this.packet.BButtonFirst = true;
                }

                this.previousB = true;
            }

            if (!this.packet.BButton)
            {
                this.previousB = false;
            }

            #endregion

            #region DPadUp

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.packet.DPadUp = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.DPadUp))
            {
                this.packet.DPadUp = true;
            }

            #endregion

            #region DPadDown

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.packet.DPadDown = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.DPadDown))
            {
                this.packet.DPadDown = true;
            }

            #endregion

            #region DPadLeft

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                this.packet.DPadLeft = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                this.packet.DPadLeft = true;
            }

            #endregion

            #region DPadRight

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.packet.DPadRight = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.DPadRight))
            {
                this.packet.DPadRight = true;
            }

            #endregion

            #region RightBumper

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.E))
            {
                this.packet.RightBumper = true;

                if (!this.previousRB)
                {
                    this.packet.RightBumperFirst = true;
                }

                this.previousRB = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.RightShoulder))
            {
                this.packet.RightBumper = true;

                if (!this.previousRB)
                {
                    this.packet.RightBumperFirst = true;
                }

                this.previousRB = true;
            }

            if (!this.packet.RightBumper)
            {
                this.previousRB = false;
            }

            #endregion

            #region LeftBumper

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                this.packet.LeftBumper = true;

                if (!this.previousLB)
                {
                    this.packet.LeftBumperFirst = true;
                }

                this.previousLB = true;
            }
#endif

            if (gamePadState.IsButtonDown(Buttons.LeftShoulder))
            {
                this.packet.LeftBumper = true;

                if (!this.previousLB)
                {
                    this.packet.LeftBumperFirst = true;
                }

                this.previousLB = true;
            }

            if (!this.packet.LeftBumper)
            {
                this.previousLB = false;
            }

            #endregion

            #region MoveHorizontal

#if !XBOX
            if (gamePadState.IsConnected)
            {
#endif
                if (Math.Abs(gamePadState.ThumbSticks.Left.X) < 0.1)
                {
                    this.packet.MoveHorizontal = 0;
                }
                else
                {
                    this.packet.MoveHorizontal = gamePadState.ThumbSticks.Left.X;
                }
#if !XBOX
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.A) &&
                    !keyboardState.IsKeyDown(Keys.D))
                {
                    this.packet.MoveHorizontal = -1;
                }
                else if (!keyboardState.IsKeyDown(Keys.A) &&
                    keyboardState.IsKeyDown(Keys.D))
                {
                    this.packet.MoveHorizontal = 1;
                }
                else
                {
                    this.packet.MoveHorizontal = 0;
                }
            }
#endif
            #endregion

            #region MoveVertical

#if !XBOX
            if (gamePadState.IsConnected)
            {
#endif
                if (Math.Abs(gamePadState.ThumbSticks.Left.Y) < 0.1)
                {
                    this.packet.MoveVertical = 0;
                }
                else
                {
                    this.packet.MoveVertical = gamePadState.ThumbSticks.Left.Y;
                }
#if !XBOX
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.W) &&
                    !keyboardState.IsKeyDown(Keys.S))
                {
                    this.packet.MoveVertical = 1;
                }
                else if (!keyboardState.IsKeyDown(Keys.W) &&
                    keyboardState.IsKeyDown(Keys.S))
                {
                    this.packet.MoveVertical = -1;
                }
                else
                {
                    this.packet.MoveVertical = 0;
                }
            }
#endif

            #endregion
            
            #region LookHorizontal

#if !XBOX
            if (gamePadState.IsConnected)
            {
#endif
                if (Math.Abs(gamePadState.ThumbSticks.Right.X) < 0.1)
                {
                    this.packet.LookHorizontal = 0;
                }
                else
                {
                    this.packet.LookHorizontal = gamePadState.ThumbSticks.Right.X;
                }
#if !XBOX
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.J) &&
                    !keyboardState.IsKeyDown(Keys.L))
                {
                    this.packet.LookHorizontal = -1;
                }
                else if (!keyboardState.IsKeyDown(Keys.J) &&
                    keyboardState.IsKeyDown(Keys.L))
                {
                    this.packet.LookHorizontal = 1;
                }
                else
                {
                    this.packet.LookHorizontal = 0;
                }
            }
#endif
            #endregion

            #region LookVertical

#if !XBOX
            if (gamePadState.IsConnected)
            {
#endif
                if (Math.Abs(gamePadState.ThumbSticks.Right.Y) < 0.1)
                {
                    this.packet.LookVertical = 0;
                }
                else
                {
                    this.packet.LookVertical = gamePadState.ThumbSticks.Right.Y;
                }
#if !XBOX
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.I) &&
                    !keyboardState.IsKeyDown(Keys.K))
                {
                    this.packet.LookVertical = 1;
                }
                else if (!keyboardState.IsKeyDown(Keys.I) &&
                    keyboardState.IsKeyDown(Keys.K))
                {
                    this.packet.LookVertical = -1;
                }
                else
                {
                    this.packet.LookVertical = 0;
                }
            }
#endif

            #endregion

            #region RightTrigger

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.PageUp))
            {
                this.packet.RightTrigger = 1;
            }
#endif

            this.packet.RightTrigger = gamePadState.Triggers.Right;

            #endregion

            #region LeftTrigger

#if !XBOX
            if (keyboardState.IsKeyDown(Keys.PageDown))
            {
                this.packet.LeftTrigger = 1;
            }
#endif

            this.packet.LeftTrigger = gamePadState.Triggers.Left;

            #endregion

            this.previousUpdate = this.updateCycle;
        }

        /// <summary>
        /// Determines whether [contains] [the specified action type].
        /// If so, returns the ActionQuantity associated with it.
        /// If not, returns 0. 
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <returns>
        /// Returns the float value corresponding to the action type.
        /// </returns>
        public float ContainsFloat(ActionType actionType)
        {
            this.GetActions();

            switch (actionType)
            {
                case ActionType.LeftTrigger: return this.packet.LeftTrigger;
                case ActionType.RightTrigger: return this.packet.RightTrigger;
                case ActionType.LookHorizontal: return this.packet.LookHorizontal;
                case ActionType.LookVertical: return this.packet.LookVertical;
                case ActionType.MoveHorizontal: return this.packet.MoveHorizontal;
                case ActionType.MoveVertical: return this.packet.MoveVertical;
            }
            
            return this.ContainsBool(actionType) ? 1 : 0;
        }

        /// <summary>
        /// Determines whether [contains] [the specified action type].
        /// If so, returns true.
        /// If not, returns false.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <returns>
        /// Returns true if the action has been found.
        /// </returns>
        public bool ContainsBool(ActionType actionType)
        {
            this.GetActions();

            switch (actionType)
            {
                case ActionType.AButton: return this.packet.AButton;
                case ActionType.BButton: return this.packet.BButton;
                case ActionType.XButton: return this.packet.XButton;
                case ActionType.YButton: return this.packet.YButton;
                case ActionType.AButtonFirst: return this.packet.AButtonFirst;
                case ActionType.BButtonFirst: return this.packet.BButtonFirst;
                case ActionType.XButtonFirst: return this.packet.XButtonFirst;
                case ActionType.YButtonFirst: return this.packet.YButtonFirst;
                case ActionType.DPadDown: return this.packet.DPadDown;
                case ActionType.DPadUp: return this.packet.DPadUp;
                case ActionType.DPadLeft: return this.packet.DPadLeft;
                case ActionType.DPadRight: return this.packet.DPadRight;
                case ActionType.GoBack: return this.packet.GoBack;
                case ActionType.LeftBumper: return this.packet.LeftBumper;
                case ActionType.RightBumper: return this.packet.RightBumper;
                case ActionType.LeftBumperFirst: return this.packet.LeftBumperFirst;
                case ActionType.RightBumperFirst: return this.packet.RightBumperFirst;
                case ActionType.Pause: return this.packet.Pause;
                case ActionType.Select: return this.packet.Select;
                case ActionType.SelectionDown: return this.packet.SelectionDown;
                case ActionType.SelectionLeft: return this.packet.SelectionLeft;
                case ActionType.SelectionRight: return this.packet.SelectionRight;
                case ActionType.SelectionUp: return this.packet.SelectionUp;
            }

            return this.ContainsFloat(actionType) == 1;
        }

        /// <summary>
        /// Rumbles the controller.
        /// </summary>
        /// <param name="leftMotorAmount">The left motor amount, in [0, 1].</param>
        /// <param name="rightMotorAmount">The right motor amount, in [0, 1].</param>
        /// <param name="durationInTicks">The duration in ticks (10,000,000 ticks/second).</param>
        public void Rumble(float leftMotorAmount, float rightMotorAmount, long durationInTicks)
        {
            this.rumblePack.Rumble(leftMotorAmount, rightMotorAmount, durationInTicks);
        }
    }
}