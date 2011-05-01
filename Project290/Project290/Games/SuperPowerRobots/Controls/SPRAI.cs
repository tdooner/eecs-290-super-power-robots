using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project290.Games.SuperPowerRobots.Entities;

namespace Project290.Games.SuperPowerRobots.Controls
{
    //The base controls class for Bots.
    public abstract class SPRAI
    {
        private Vector2 m_Move;
        private Vector2 m_Fire;
        private float m_Spin;
        private bool[] m_Weapons;
        protected SPRWorld m_World;

        public SPRAI(SPRWorld world)
        {
            m_World = world;
            m_Weapons = new bool[4];
        }

        /// <summary>
        /// Movement vector, length between 0 and 1. "Left Joystick"
        /// </summary>
        public Vector2 Move {
            get { return m_Move; }
            protected set {
                m_Move = value;
                if (m_Move.LengthSquared() > 1)
                    m_Move.Normalize();
            }
        }

        /// <summary>
        /// Firing vector, length between 0 and 1. "Right Joystick"
        /// </summary>
        public Vector2 Fire
        {
            get { return m_Fire; }
            protected set
            {
                m_Fire = value;
                if (m_Fire.LengthSquared() > 1)
                    m_Fire.Normalize();
            }
        }

        /// <summary>
        /// Counterclockwise angular impulse (relative to max, between -1 and 1.) "Left Trigger minus Right Trigger"
        /// </summary>
        public float Spin
        {
            get { return m_Spin; }
            protected set
            {
                m_Spin = Math.Max(Math.Min(value, 1), -1); //clamp between -1 and 1
            }
        }

        /// <summary>
        /// For each weapon, true if weapon is firing, false if not.
        /// 0 = "B", 1 = "A", 2 = "X", 3 = "Y"
        /// </summary>
        public bool[] Weapons
        {
            get { return m_Weapons; }
        }

        /// <summary>
        /// Updates the AI.
        /// </summary>
        /// <param name="dTime">Time elapsed.</param>
        public abstract void Update(float dTime, Bot self);
    }
}
