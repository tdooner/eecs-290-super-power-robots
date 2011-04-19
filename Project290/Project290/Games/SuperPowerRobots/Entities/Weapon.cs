using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project290.Physics.Dynamics;
using Project290.Physics.Factories;
using Project290.Rendering;

namespace Project290.Games.SuperPowerRobots.Entities
{
    public class Weapon : Entity
    {
        private Bot m_owner;
        private bool m_firing;
        Texture2D texture;

        public Weapon(SPRWorld sprWorld, Body body, Bot bot, float rotation, String textureName)
            : base(sprWorld, body)
        {
            texture = TextureStatic.Get(textureName);
            this.SetRotation(rotation);
            this.m_owner = bot;
            this.m_firing = false;
        }
        //Weapons can spawn Projectiles, attached or unattached.

        public void Fire()
        {
            this.m_firing = true;
        }

        public override void Update(float dTime)
        {
            Console.WriteLine(this.GetRotation() % Math.PI);

            if (this.m_firing && texture == TextureStatic.Get("Gun"))
            {
                Body tempBody = BodyFactory.CreateBody(this.SPRWorld.World);
                tempBody.BodyType = BodyType.Dynamic;
                tempBody.Mass = 0.000001f;
                tempBody.Inertia = 0.000001f;
                tempBody.Position = this.GetPosition();
                this.SPRWorld.AddEntitiy(new Projectile(this.SPRWorld, tempBody, m_owner.GetVelocity() * this.GetRotation() * 1000f, this.GetRotation()));
                this.m_firing = false;
            }
        }

        public override void Draw()
        {
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
