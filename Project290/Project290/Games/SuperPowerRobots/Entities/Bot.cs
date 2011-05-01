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
        private SortedDictionary<ulong, Weapon> m_weapons;
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

        public Bot(SPRWorld sprWord, Body body, Bot.Player player, Bot.Type type, SPRAI control)
            : base(sprWord, body)
        {

            // Figure out which bot this is, and load the appropriate textures.
            this.m_player = player;
            this.m_type = type;
            this.m_Control = control;
            if (this.m_player == Bot.Player.Human)
            {
                this.texture = TextureStatic.Get("4SideFriendlyRobot");
            }
            if (this.m_player == Bot.Player.Computer)
            {
                this.texture = TextureStatic.Get("4SideEnemyRobot");
            }

            this.m_weapons = new SortedDictionary<ulong, Weapon>();

            this.AddWeapon(0f, new Vector2 (texture.Width / 2 + 20, 0), "Gun");
            this.AddWeapon((float) Math.PI / 2, new Vector2 (0, texture.Height / 2 + 20), "Gun");
            this.AddWeapon((float)(Math.PI * (3.0 / 2.0)), new Vector2(0, -texture.Height / 2 - 20), "Shield");
            this.AddWeapon((float) Math.PI, new Vector2 (-texture.Width / 2 - 20, 0), "Axe");
            
        }   

        public void AddWeapon(float rotation, Vector2 relativePosition, String textureName)
        {
            Body tempBody = BodyFactory.CreateBody(this.SPRWorld.World);
            tempBody.BodyType = BodyType.Dynamic;
//          tempBody.Mass = .05f;
//          tempBody.Inertia = .00000005f;
            Vertices v = SPRWorld.computedSpritePolygons[textureName];
            // Simplify the object until it has few enough verticies.
            while (v.Count > Physics.Settings.MaxPolygonVertices) // Infinite loop potential?
            {
                v = SimplifyTools.DouglasPeuckerSimplify(v, 2); // Where 2 is a completely arbitrary number?
            }
            Fixture f = FixtureFactory.CreatePolygon(SPRWorld.computedSpritePolygons[textureName], 0.0000001f, tempBody);
            fixtures.Add(f);
            //tempBody.SetTransform(Vector2.Zero, rotation);
            Weapon weapon = new Weapon(this.SPRWorld, tempBody, this, rotation, textureName);
            Joint joint = JointFactory.CreateWeldJoint(this.SPRWorld.World, this.Body, weapon.Body, relativePosition, Vector2.Zero);
            this.m_weapons.Add(weapon.GetID(), weapon);
        }

        public Vector2 GetPosition()
        {
            return this.Body.Position;
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
            this.Body.ApplyLinearImpulse(.05f * m_Control.Move);
            this.Body.ApplyAngularImpulse(.05f * m_Control.Spin);

            bool[] weapons = m_Control.Weapons;
            int fire = 0; //mark the weapon to fire using the right stick
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i]) this.m_weapons.Values.ElementAt(i).Fire();

                Vector2 weapDir = new Vector2((float)Math.Cos(this.m_weapons.Values.ElementAt(i).GetRotation()), (float)Math.Sin(this.m_weapons.Values.ElementAt(i).GetRotation()));
                Vector2 maxDir = new Vector2((float)Math.Cos(this.m_weapons.Values.ElementAt(fire).GetRotation()), (float)Math.Sin(this.m_weapons.Values.ElementAt(fire).GetRotation()));
                if(Vector2.Dot(m_Control.Fire, weapDir) > Vector2.Dot(m_Control.Fire, maxDir)) fire = i;
            }

            if (m_Control.Fire.LengthSquared() > 0) this.m_weapons.Values.ElementAt(fire).Fire();

            foreach (Weapon w in m_weapons.Values)
            {
                w.Update(dTime);
            }
        }

        public override void Draw()
        {
            //Texture2D texture = TextureStatic.Get("4SideFriendlyRobot");
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
            foreach (Fixture f in fixtures)
            {
                
            }
            foreach (Weapon w in m_weapons.Values)
            {
                w.Draw();
            }

        }
    }
}
