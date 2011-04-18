using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Physics.Dynamics;

namespace Project290.Games.SuperPowerRobots
{
    class SPRWorld
    {
        private World m_World;

        public World World { get { return m_World; } }

        public void Update(float dTime)
        {
            //call m_World.Step() first, to update the physics

            //then call the entity updates, take damage, listen to controls, spawn any projectiles, etc.

            //check for dead bots
        }
    }
}
