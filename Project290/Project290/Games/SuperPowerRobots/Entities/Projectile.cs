using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Dynamics;
using Project290.Rendering;

namespace Project290.Games.SuperPowerRobots.Entities
{
    class Projectile : Entity
    {
        private Vector2 m_velocity;
        private float m_rotation;
        private float m_Life;

        public Projectile(SPRWorld sprWorld, Body body, Vector2 velocity, float rotation, float life)
            : base(sprWorld, body)
        {
            this.m_rotation = rotation;
            this.m_velocity = velocity;
            this.m_Life = life;
        }

        public override void Update(float dTime)
        {
            if (!IsDead())
            {
                this.Body.ApplyLinearImpulse(this.m_velocity);
                m_Life -= dTime;
                if (m_Life <= 0) this.SetDead(true);
            }
        }

        public override void Draw()
        {
            Texture2D texture = TextureStatic.Get("Projectile");
            Drawer.Draw(
                texture,
                this.GetPosition(),
                null,
                Color.White,
                this.GetRotation(),
                new Vector2(texture.Width / 2, texture.Height / 2),
                1f,
                SpriteEffects.None,
                0f);
        }
    }
}
