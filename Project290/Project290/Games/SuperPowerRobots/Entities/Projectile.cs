using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Dynamics;

namespace Project290.Games.SuperPowerRobots.Entities
{
    class Projectile : Entity
    {
        private CircleShape shape;
        private Vector2 m_velocity;

        public Projectile(SPRWorld sprWorld, Body body, Vector2 velocity)
            : base(sprWorld, body)
        {
        }

        public override void Update(float dTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }
    }
}
