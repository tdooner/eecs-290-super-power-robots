using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Physics.Dynamics;
using Microsoft.Xna.Framework;

namespace Project290.Games.SuperPowerRobots.Entities
{
    public abstract class Entity
    {
        protected static ulong s_ID = 0;

        private Body m_Body;
        private SPRWorld m_SPRWorld;
        private ulong m_ID;

        public Entity(SPRWorld sprWorld, Body body)
        {
            this.m_SPRWorld = sprWorld;
            this.m_Body = body;
            m_ID = s_ID++;
        }

        public Body Body
        {
            get { return m_Body; }
            private set { m_Body = value; }
        }

        public SPRWorld SPRWorld
        {
            get { return m_SPRWorld; }
            private set { m_SPRWorld = value; }
        }

        public Vector2 GetPosition()
        {
            return m_Body.Position;
        }

        public float GetRotation()
        {
            return m_Body.Rotation;
        }

        public ulong GetID()
        {
            return m_ID;
        }

        public void ApplyLinearImpulse(Vector2 impulse)
        {
            this.Body.ApplyLinearImpulse(impulse);
        }

        public void ApplyAngularImpulse(float impulse)
        {
            this.Body.ApplyAngularImpulse(impulse);
        }

        public void SetRotation(float rotation)
        {
            this.Body.Rotation = rotation;
        }

        public abstract void Update(float dTime);

        public abstract void Draw();
    }
}
