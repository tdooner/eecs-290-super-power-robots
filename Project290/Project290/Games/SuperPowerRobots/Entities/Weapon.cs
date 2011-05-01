using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project290.Physics.Dynamics;
using Project290.Physics.Factories;
using Project290.Rendering;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Dynamics.Joints;
using Project290.Physics.Common;
using Project290.Physics.Common.PolygonManipulation;
using Project290.Physics.Dynamics.Contacts;

namespace Project290.Games.SuperPowerRobots.Entities
{
    public enum WeaponType
    {
        gun,
        melee,
        shield
    }
    public class Weapon
    {
        private SPRWorld m_SPRWorld;
        private Bot m_owner;
        private Vector2 m_Position; //relative to owner
        private float m_Rotation; //relative to owner
        private bool m_firing;
        private float m_reloadTime;
        private float m_reloading;
        private float m_power;
        private float m_health;
        private WeaponType weaponType;
        private Fixture m_Fixture;
        private Texture2D m_Texture;
        private Vector2 m_Scale;

        public Weapon(SPRWorld sprWorld, Bot bot, Vector2 relativePosition, float relativeRotation, String textureName, Vector2 scale, WeaponType weaponType, float health, float power)
        {
            m_SPRWorld = sprWorld;
            this.m_owner = bot;
            this.m_firing = false;
            this.m_reloadTime = .2f;
            this.m_reloading = 0;
            this.weaponType = weaponType;
            this.m_power = power;
            m_Position = relativePosition;
            m_Rotation = relativeRotation;
            m_Texture = TextureStatic.Get(textureName);
            m_Scale = scale;
            this.m_health = health;

            Vertices v = SPRWorld.computedSpritePolygons[textureName];
            // Simplify the object until it has few enough verticies.
            while (v.Count > Physics.Settings.MaxPolygonVertices) // Infinite loop potential?
            {
                v = SimplifyTools.DouglasPeuckerSimplify(v, 2); // Where 2 is a completely arbitrary number?
            }

            v.Scale(ref scale);
            //v.Translate(ref relativePosition);
            v.Rotate(relativeRotation);

            Fixture f = FixtureFactory.CreatePolygon(v, 1f, bot.Body, relativePosition);
            m_Fixture = f;
            f.Friction = 0.5f;
            f.Restitution = 0f;
            f.UserData = this;
            /*if (this.weaponType == WeaponType.melee)
            {
                Body tempBody = BodyFactory.CreateBody(m_SPRWorld.World);
                tempBody.BodyType = BodyType.Dynamic;
                float rotation = this.GetRotation();
                tempBody.Position = this.GetPosition();
                //tempBody.SetTransform(tempBody.Position, 0);
                Projectile justFired = new Projectile(m_SPRWorld, tempBody, TextureStatic.Get("Axe"), new Vector2(0, 0), this.GetRotation(), 5, Settings.MetersPerPixel * 5, 5 * Settings.MetersPerPixel);
                RevoluteJoint joint = JointFactory.CreateRevoluteJoint(m_SPRWorld.World, this.m_owner.Body, tempBody, Vector2.Zero);
                joint.LowerLimit = rotation - (float)Math.PI / 4f;
                joint.UpperLimit = rotation + (float)Math.PI / 4f;
            }*/
        }
        //Weapons can spawn Projectiles, attached or unattached.

        public void Fire()
        {
            if (m_reloading <= 0)
            {
                this.m_firing = true;
                m_reloading = m_reloadTime;
            }
        }

        public Vector2 GetRelPosition()
        {
            return m_Position;
        }

        public float GetRelRotation()
        {
            return m_Rotation;
        }

        public Vector2 GetAbsPosition()
        {
            float ownRot = this.m_owner.GetRotation();
            Vector2 p = this.GetRelPosition();
            Vector2 relPos = new Vector2(p.X * (float)Math.Cos(ownRot) + p.Y * (float)Math.Sin(ownRot), p.X * (float)Math.Sin(ownRot) - p.Y * (float)Math.Cos(ownRot));

            return m_owner.GetPosition() + relPos;
        }

        public float GetAbsRotation()
        {
            return m_owner.GetRotation() + m_Rotation;
        }
        
        public void Update(float dTime)
        {
            if (m_reloading > 0)
            {
                m_reloading -= dTime;
            }

            if (this.m_firing && weaponType == WeaponType.gun)
            {
                Body tempBody = BodyFactory.CreateBody(m_SPRWorld.World);
                tempBody.BodyType = BodyType.Dynamic;
                tempBody.IsBullet = true;
                float rotation = this.GetAbsRotation();
                tempBody.Position = this.GetAbsPosition() + 40 * Settings.MetersPerPixel * (new Vector2((float) Math.Cos(rotation), (float)Math.Sin(rotation)));
                tempBody.SetTransform(tempBody.Position, 0);
                Fixture f = FixtureFactory.CreateCircle(4 * Settings.MetersPerPixel, 0.000001f, tempBody);
                f.OnCollision += OnBulletHit;
                Vector2 initialVelocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                Projectile justFired = new Projectile(m_SPRWorld, tempBody, TextureStatic.Get("Projectile"), initialVelocity, this.GetAbsRotation(), 5, 5 * Settings.MetersPerPixel, 5 * Settings.MetersPerPixel, m_power);
                this.m_SPRWorld.AddEntity(justFired);
                f.UserData = justFired;
                m_SPRWorld.AddEntity(justFired);
                this.m_firing = false;
            }
            
			if (this.m_firing && weaponType == WeaponType.melee)
            {
            }
        }

        public void GetDamaged(float damage)
        {
            this.m_health -= damage;
        }

        public void Draw()
        {
            Drawer.Draw(
               m_Texture,
               (this.GetAbsPosition()) * Settings.PixelsPerMeter,
               new Rectangle(0, 0, m_Texture.Width, m_Texture.Height),
               Color.White,
               this.GetAbsRotation(),
               new Vector2(m_Texture.Width / 2, m_Texture.Height / 2),
               m_Scale,
               SpriteEffects.None,
               .3f);
        }

        public bool OnBulletHit(Fixture a, Fixture b, Contact c)
        {
            // Fixture a is always the bullet, and Fixture b is what it hit.
            if ((SPRWorld.ObjectTypes) b.UserData == SPRWorld.ObjectTypes.Wall)
            {
                Projectile p = (Projectile) a.UserData;
                if (b.UserData is Weapon)
                {
                    Weapon w = (Weapon) b.UserData;
                    w.GetDamaged(p.GetPower());
                }

                if (b.UserData is Bot)
                {
                    Bot bot = (Bot)b.UserData;
                    bot.GetDamaged(p.GetPower());
                }
                a.Body.Dispose(); // Simply delete the bullet.
            }
            if ((SPRWorld.ObjectTypes)b.UserData == SPRWorld.ObjectTypes.Weapon)
            {
                // Sean's TODO: Add damage calculations....
                
                a.Body.Dispose(); // Simply delete the bullet.
            }
            return true;
        }

        
    }
}
