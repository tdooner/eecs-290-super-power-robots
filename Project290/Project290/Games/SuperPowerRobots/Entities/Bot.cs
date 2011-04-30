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

namespace Project290.Games.SuperPowerRobots.Entities
{
    public class Bot : Entity
    {
        //Bots have four Weapons, the Bodies are attached via WeldJoints
        private SortedDictionary<ulong, Weapon> m_weapons;
        private CircleShape m_shape;
        private Texture2D texture;
        private Vector2 thrust;
        private List<Fixture> fixtures = new List<Fixture>();
        //temp variable, do not include in final project
        private Bot.Player m_player;
        private Bot.Type m_type;

        public enum Player
        {
            Human = 0,
            Computer = 1
        }
        public enum Type
        {
            FourSided = 0
        }

        public Bot(SPRWorld sprWord, Body body, Bot.Player player, Bot.Type type)
            : base(sprWord, body)
        {
            thrust = Vector2.Zero;

            // Figure out which bot this is, and load the appropriate textures.
            this.m_player = player;
            this.m_type = type;
            if (this.m_player == Bot.Player.Human)
            {
                this.texture = TextureStatic.Get("4SideFriendlyRobot");
            }
            if (this.m_player == Bot.Player.Computer)
            {
                this.texture = TextureStatic.Get("4SideEnemyRobot");
            }

            this.m_weapons = new SortedDictionary<ulong, Weapon>();

            this.AddWeapon((float)(Math.PI * (3.0 / 2.0)), new Vector2(0, -texture.Height / 2 - 20), "Shield");
            this.AddWeapon(0f, new Vector2 (texture.Width / 2 + 20, 0), "Gun");
            this.AddWeapon((float) Math.PI / 2, new Vector2 (0, texture.Height / 2 + 20), "Gun");
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
            Fixture f = FixtureFactory.CreatePolygon(SPRWorld.computedSpritePolygons[textureName], 0.00001f, tempBody);
            fixtures.Add(f);
            tempBody.SetTransform(Vector2.Zero, rotation);
            Weapon weapon = new Weapon(this.SPRWorld, tempBody, this, rotation, textureName);
            Joint joint = JointFactory.CreateWeldJoint(this.SPRWorld.World, this.Body, weapon.Body, relativePosition, Vector2.Zero);
            this.m_weapons.Add(weapon.GetID(), weapon);
        }

        public Vector2 GetPosition()
        {
            return new Vector2(this.Body.Position.X, this.Body.Position.Y);
        }

        public Vector2 GetVelocity()
        {
            return this.thrust;
        }

        public override void Update(float dTime)
        {
            this.thrust = Vector2.Zero;
            if (this.m_player == Bot.Player.Human)
            {
                
                this.thrust.X = .05f * GameWorld.controller.ContainsFloat(ActionType.MoveHorizontal);
                this.thrust.Y = -.05f * GameWorld.controller.ContainsFloat(ActionType.MoveVertical);

                if (GameWorld.controller.ContainsFloat(ActionType.LeftTrigger) > 0)
                {
                    this.Body.ApplyAngularImpulse(-.05f * GameWorld.controller.ContainsFloat(ActionType.LeftTrigger));
                }

                if (GameWorld.controller.ContainsFloat(ActionType.RightTrigger) > 0)
                {
                    this.Body.ApplyAngularImpulse(.05f*GameWorld.controller.ContainsFloat(ActionType.RightTrigger));
                }
                if (GameWorld.controller.ContainsBool(ActionType.BButton))
                {
                    this.m_weapons.Values.ElementAt(1).Fire();
                }
                if (GameWorld.controller.ContainsBool(ActionType.AButton))
                {
                    this.m_weapons.Values.ElementAt(2).Fire();
                }
            }
            if (this.m_player == Bot.Player.Computer)
            {
                // UNCOMMENT TO HAVE AN ENEMY THAT RUNS AWAY!
                //this.thrust.X = .075f;
                //this.thrust.Y = .075f;
            }
            this.Body.ApplyLinearImpulse(thrust);
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
