using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Dynamics;
using Project290.Rendering;
using Project290.Physics.Dynamics.Contacts;

namespace Project290.Games.SuperPowerRobots.Entities
{
    class Projectile : Entity
    {
        private Vector2 m_velocity;
        private float m_rotation;
        private float m_Life;
        private float m_power;
        private static List<Body> m_toRemove;

        public Projectile(SPRWorld sprWorld, Body body, Texture2D texture, Vector2 velocity, float rotation, float life, float width, float height, float power, float health)
            : base(sprWorld, body, texture, width, height, health)
        {
            this.m_rotation = rotation;
            this.m_velocity = velocity;
            this.m_Life = life;
            this.m_power = power;
            m_toRemove = new List<Body>();
        }

        public float GetPower()
        {
            return m_power;
        }

        public override void Update(float dTime)
        {
            base.Update(dTime);

            //if (!IsDead())
            //{
            this.Body.ResetDynamics();
            this.Body.ApplyLinearImpulse(m_velocity);

            foreach (Body b in m_toRemove)
            {
                b.Dispose();
            }

            m_toRemove = new List<Body>();
            //    m_Life -= dTime;
            //    if (m_Life <= 0) this.SetDead(true);
            //}
        }

        public static bool OnBulletHit(Fixture a, Fixture b, Contact c)
        {
            // Fixture a is always the bullet, and Fixture b is what it hit.

            if (b.UserData is String && ((string)(b.UserData.ToString())).Equals("Wall"))
            {
                if (!m_toRemove.Contains(a.Body))
                    m_toRemove.Add(a.Body);
                return true;
            }
            if (b.Body.IsBullet)
                return true;
            // If we've gotten this far, b.UserData is an Object
            Projectile p = (Projectile)a.UserData;
            
            // If we hit a weapon.
            if (b.UserData is Weapon)
            {
                Weapon w = (Weapon)b.UserData;
                w.TakeDamage(p.GetPower());
            }
            // If we hit a bot
            if (b.UserData is Bot)
            {
                Bot bot = (Bot)b.UserData;
                bot.TakeDamage(p.GetPower());
            }
            // If we hit a projectile (e.g. an axe)
            if (b.UserData is Projectile)
            {
                Projectile o = (Projectile)b.UserData;
                o.TakeDamage(p.GetPower());
            }
            if (!m_toRemove.Contains(a.Body))
                m_toRemove.Add(a.Body);
            return true;
        }
        public static bool OnMeleeHit(Fixture a, Fixture b, Contact c)
        {
            if (b.Body.IsBullet)
                return true;
            Projectile p = (Projectile)a.UserData;
            // If we hit a weapon.
            if (b.UserData is Weapon)
            {
                Weapon w = (Weapon)b.UserData;
                w.TakeDamage(p.GetPower());
            }
            // If we hit a bot
            if (b.UserData is Bot)
            {
                Bot bot = (Bot)b.UserData;
                bot.TakeDamage(p.GetPower());
            }
            // If we hit a projectile (e.g. an axe)
            if (b.UserData is Projectile)
            {
                Projectile o = (Projectile)b.UserData;
                o.TakeDamage(p.GetPower());
            }
            return true;
        }
    }
}
