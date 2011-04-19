using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.GameElements;
using Project290.Physics.Dynamics;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Factories;

namespace Project290.Games.SuperPowerRobots.Entities
{
    class Bot:Entity
    {
        //Bots have four Weapons, the Bodies are attached via WeldJoints
        private Weapon[] m_weapons;
        private CircleShape m_shape;

        public Bot(SPRWorld sprWord)
            : base(sprWord)
        {
            //m_Body = BodyFactory.CreateBody(sprWord);

        }
    }
}
