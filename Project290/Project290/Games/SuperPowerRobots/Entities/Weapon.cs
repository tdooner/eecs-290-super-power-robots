using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Physics.Dynamics;

namespace Project290.Games.SuperPowerRobots.Entities
{
    class Weapon:Entity
    {
        private Body m_Body;

        public Weapon(SPRWorld sprWorld, Body body)
            : base(sprWord, body)
        {
        }
        //Weapons can spawn Projectiles, attached or unattached.
    }
}
