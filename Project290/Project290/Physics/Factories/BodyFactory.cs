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
using Project290.Physics.Dynamics;
using Microsoft.Xna.Framework;

namespace Project290.Physics.Factories
{
    /// <summary>
    /// An easy to use factory for creating bodies
    /// </summary>
    public static class BodyFactory
    {
        public static Body CreateBody(World world)
        {
            return CreateBody(world, null);
        }

        public static Body CreateBody(World world, Object userData)
        {
            Body body = new Body(world, userData);
            return body;
        }

        public static Body CreateBody(World world, Vector2 position)
        {
            return CreateBody(world, position, null);
        }

        public static Body CreateBody(World world, Vector2 position, Object userData)
        {
            Body body = CreateBody(world, userData);
            body.Position = position;
            return body;
        }
    }
}