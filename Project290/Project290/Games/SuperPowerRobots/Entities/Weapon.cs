using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Physics.Dynamics;

namespace Project290.Games.SuperPowerRobots.Entities
{
    public class Weapon:Entity
    {
        public Weapon(SPRWorld sprWorld, Body body)
            : base(sprWorld, body)
        {
        }
        //Weapons can spawn Projectiles, attached or unattached.

        public void Updated(float dTime)
        {
        }

        public void Draw()
        {
        }
    }
}
