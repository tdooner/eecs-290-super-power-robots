using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Physics.Collision.Shapes;

namespace Project290.Games.SuperPowerRobots.Entities
{
    class Projectile : Entity
    {
        private CircleShape shape;
        private Body body;

        public Projectile(SPRWorld sprWorld, Body body)
            : base(sprWord, body)
        {
        }
    }
}
