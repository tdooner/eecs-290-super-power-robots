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

namespace Project290.Inputs
{
    /// <summary>
    /// Defines a type of action returned from the Xbox Controller.
    /// This does not have to correspond to a button. For example,
    /// the sequence X, Y, A could be public classified as its own action.
    /// </summary>
    public enum ActionType
    {
        SelectionUp,
        SelectionDown,
        SelectionLeft,
        SelectionRight,
        Select,
        GoBack,
        XButton,
        YButton,
        AButton,
        BButton,
        XButtonFirst,
        YButtonFirst,
        AButtonFirst,
        BButtonFirst,
        RightBumper,
        LeftBumper,
        RightBumperFirst,
        LeftBumperFirst,
        Pause,
        DPadUp,
        DPadDown,
        DPadLeft,
        DPadRight,
        MoveHorizontal,
        MoveVertical,
        LookHorizontal,
        LookVertical,
        RightTrigger,
        LeftTrigger
    }
}