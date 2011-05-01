using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project290.GameElements;
using Project290.Physics.Dynamics;
using Project290.Physics.Dynamics.Joints;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Factories;
using Project290.Inputs;
using Project290.Rendering;
using Project290.Physics.Common;
using Project290.Physics.Common.PolygonManipulation;
using Project290.Games.SuperPowerRobots.Controls;

namespace Project290.Games.SuperPowerRobots.Entities
{
    public class Bot : Entity
    {
        //Bots have four Weapons, the Bodies are attached via WeldJoints
        private Weapon[] m_Weapons;
        private Texture2D texture;
        private List<Fixture> fixtures = new List<Fixture>();
        //temp variable, do not include in final project
        private Bot.Player m_player;
        private Bot.Type m_type;
        private SPRAI m_Control;

        public enum Player
        {
            Human = 0,
            Computer = 1
        }
        public enum Type
        {
            FourSided = 0
        }

        public Bot(SPRWorld sprWord, Body body, Bot.Player player, Bot.Type type, SPRAI control, Texture2D texture, float width, float height, float health)
            : base(sprWord, body, texture, width, height, health)
        {
            this.m_player = player;
            this.m_type = type;
            this.m_Control = control;

            this.m_Weapons = new Weapon[4];

            this.AddWeapon(0, "Gun", WeaponType.gun, (float) 100, (float) 10);
            this.AddWeapon(1, "Gun", WeaponType.gun, (float)100, (float)10);
            this.AddWeapon(2, "Shield", WeaponType.shield, (float) 500, (float) 50);
            this.AddWeapon(3, "Gun", WeaponType.melee, (float)100, (float)0);
            
        }   

        public void AddWeapon(int side, String textureName, WeaponType weaponType, float health, float power)
        {
            float relativeRotation = side * (float)Math.PI / 2f;
            Vector2 relativePosition = new Vector2(-((side - 1) % 2) * GetWidth() / 2, ((side - 2) % 2) * GetHeight() / 2);

            Weapon weapon = new Weapon(this.SPRWorld, this, relativePosition, relativeRotation, textureName, new Vector2(1,1), weaponType, health, power);
            this.m_Weapons[side] = weapon;
        }

        public Vector2 GetVelocity()
        {
            return this.Body.LinearVelocity;
        }

        public SPRAI GetControl()
        {
            return m_Control;
        }

        public void setControl(SPRAI control)
        {
            m_Control = control;
        }

        public override void Update(float dTime)
        {
            m_Control.Update(dTime, this);
            this.Body.ApplyLinearImpulse(0.05f * m_Control.Move);
            this.Body.ApplyAngularImpulse(0.05f * m_Control.Spin);

            bool[] weapons = m_Control.Weapons;
            int fire = 0; //mark the weapon to fire using the right stick
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i]) m_Weapons[i].Fire();

                Vector2 weapDir = new Vector2((float)Math.Cos(m_Weapons[i].GetAbsRotation()), (float)Math.Sin(m_Weapons[i].GetAbsRotation()));
                Vector2 maxDir = new Vector2((float)Math.Cos(m_Weapons[fire].GetAbsRotation()), (float)Math.Sin(m_Weapons[fire].GetAbsRotation()));
                if(Vector2.Dot(m_Control.Fire, weapDir) > Vector2.Dot(m_Control.Fire, maxDir)) fire = i;
            }

            if (m_Control.Fire.LengthSquared() > 0) m_Weapons[fire].Fire();

            foreach (Weapon w in m_Weapons)
            {
                w.Update(dTime);
            }
        }

        public override void Draw()
        {
            base.Draw();
            foreach (Weapon w in m_Weapons)
            {
                w.Draw();
            }
        }
    }
}
