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
        private CircleShape shape;
        private Vector2 m_velocity;
        private float m_rotation;

        public Projectile(SPRWorld sprWorld, Body body, Vector2 velocity, float rotation)
            : base(sprWorld, body)
        {
            this.m_rotation = rotation;
            this.m_velocity = velocity;
        }

        public override void Update(float dTime)
        {
            this.Body.ApplyLinearImpulse(this.m_velocity);
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
