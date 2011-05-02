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
        private RevoluteJoint m_AxeJoint;
        private float m_Time;

        public Weapon(SPRWorld sprWorld, Bot bot, Vector2 relativePosition, float relativeRotation, String textureName, Vector2 scale, WeaponType weaponType, float health, float power)
        {
            m_SPRWorld = sprWorld;
            m_Time = 0;
            this.m_owner = bot;
            this.m_firing = false;
            this.m_reloadTime = weaponType == WeaponType.melee ? 0 : .2f;
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
            if (this.weaponType == WeaponType.melee)
            {
                Body tempBody = BodyFactory.CreateBody(m_SPRWorld.World);
                tempBody.BodyType = BodyType.Dynamic;
                Vertices v2 = SPRWorld.computedSpritePolygons["Axe"];
                // Simplify the object until it has few enough verticies.
                while (v2.Count > Physics.Settings.MaxPolygonVertices) // Infinite loop potential?
                {
                    v2 = SimplifyTools.DouglasPeuckerSimplify(v2, 2); // Where 2 is a completely arbitrary number?
                }
                Fixture f2 = FixtureFactory.CreatePolygon(SPRWorld.computedSpritePolygons[textureName], 0.1f, tempBody);
                f2.Friction = 0.5f;
                f2.Restitution = 0f;
                tempBody.SetTransform(this.GetAbsPosition(), this.GetAbsRotation());
                Projectile justFired = new Projectile(m_SPRWorld, tempBody, TextureStatic.Get("Axe"), new Vector2(0, 0), this.GetRelRotation(), 5, Settings.MetersPerPixel * 80, 80 * Settings.MetersPerPixel, m_power);
                f2.UserData = justFired;
                RevoluteJoint joint = JointFactory.CreateRevoluteJoint(m_SPRWorld.World, this.m_owner.Body, tempBody, Vector2.Zero);
                joint.MaxMotorTorque = 160;
                joint.LimitEnabled = true;
                joint.MotorEnabled = true;
                joint.LowerLimit =  - (float)Math.PI / 4f;
                joint.UpperLimit = (float)Math.PI / 4f;
                m_AxeJoint = joint;
                m_SPRWorld.AddEntity(justFired);
            }
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

        public WeaponType GetWeaponType()
        {
            return weaponType;
        }

        public float GetHealth()
        {
            return m_health;
        }

        public float GetPower()
        {
            return m_power;
        }

        public float GetAbsRotation()
        {
            return m_owner.GetRotation() + m_Rotation;
        }
        
        public void Update(float dTime)
        {
            m_Time += dTime;
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
                tempBody.Position = this.GetAbsPosition() + 45 * Settings.MetersPerPixel * (new Vector2((float) Math.Cos(rotation), (float)Math.Sin(rotation)));
                tempBody.SetTransform(tempBody.Position, 0);
                Fixture f = FixtureFactory.CreateCircle(4 * Settings.MetersPerPixel, 1f, tempBody);
                f.OnCollision += Projectile.OnBulletHit;
                Vector2 initialVelocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation))/10;
                Projectile justFired = new Projectile(m_SPRWorld, tempBody, TextureStatic.Get("Projectile"), initialVelocity, this.GetAbsRotation(), 5, 5 * Settings.MetersPerPixel, 5 * Settings.MetersPerPixel, m_power);
                f.UserData = justFired;
                
                this.m_SPRWorld.AddEntity(justFired);
                this.m_firing = false;
            }

            if (!this.m_firing && weaponType == WeaponType.melee)
                m_AxeJoint.MotorSpeed = 0;

			if (this.m_firing && weaponType == WeaponType.melee)
            {
                m_AxeJoint.MotorSpeed = (float)Math.Cos(m_Time * 10) * 10;
                this.m_firing = false;
            }

            // Similar to Entity.Update()
            if (this.m_health <= 0f)
            {
                this.m_owner.RemoveWeapon(this);
            }

            this.m_SPRWorld.World.ProcessChanges();
            
        }

        public void TakeDamage(float damage)
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
    }
}
