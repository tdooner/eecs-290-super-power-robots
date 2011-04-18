using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Physics.Dynamics;

namespace Project290.Games.SuperPowerRobots.Entities
{
    class Entity
    {
        private Body m_Body;
        protected SPRWorld m_SPRWorld;

        public Entity(SPRWorld sprWorld)
        {
            m_SPRWorld = sprWorld;
        }

        public Body Body
        {
            get { return m_Body; }
            private set { m_Body = value; }
        }

        //public abstract void Update(float dTime);

        //public abstract void Draw();
    }
}
