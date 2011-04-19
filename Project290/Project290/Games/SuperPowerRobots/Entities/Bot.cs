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

namespace Project290.Games.SuperPowerRobots.Entities
{
    public class Bot : Entity
    {
        //Bots have four Weapons, the Bodies are attached via WeldJoints
        private SortedDictionary<ulong, Weapon> m_weapons;
        private CircleShape m_shape;
        private Texture2D texture;
        //temp variable, do not include in final project
        private bool m_player;

        public Bot(SPRWorld sprWord, Body body, bool player)
            : base(sprWord, body)
        {
            this.texture = TextureStatic.Get("4SideFriendlyRobot");

            this.m_player = player;
            this.m_weapons = new SortedDictionary<ulong, Weapon>();
            
            this.AddWeapon(0f, new Vector2 (texture.Width / 2 + 20, 0), "Gun");
            this.AddWeapon((float) Math.PI / 2, new Vector2 (0, texture.Height / 2 + 20), "Gun");
            this.AddWeapon((float) Math.PI, new Vector2 (-texture.Width / 2 - 20, 0), "Axe");
            this.AddWeapon((float) (Math.PI * (3.0/2.0)), new Vector2 (0, -texture.Height / 2 - 20), "Shield");
            
        }

        public void AddWeapon(float rotation, Vector2 relativePosition, String textureName)
        {
            Body tempBody = BodyFactory.CreateBody(this.SPRWorld.World);
            tempBody.BodyType = BodyType.Dynamic;
            tempBody.Mass = 0.000001f;
            tempBody.Inertia = 0.000001f;
            Weapon weapon = new Weapon(this.SPRWorld, tempBody, this, rotation, textureName);
            //weapon.SetRotation(rotation);
            Joint joint = JointFactory.CreateWeldJoint(this.SPRWorld.World, this.Body, weapon.Body, relativePosition, Vector2.Zero);
            this.m_weapons.Add(weapon.GetID(), weapon);
        }

        public Vector2 GetPosition()
        {
            return new Vector2(this.Body.Position.X, this.Body.Position.Y);
        }

        public override void Update(float dTime)
        {
            if (this.m_player)
            {
                if (GameWorld.controller.ContainsFloat(ActionType.MoveVertical) < 0)
                {
                    this.ApplyLinearImpulse(new Vector2(0, 5000));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.MoveVertical) > 0)
                {
                    this.ApplyLinearImpulse(new Vector2(0, -5000));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.MoveHorizontal) > 0)
                {
                    this.ApplyLinearImpulse(new Vector2(5000, 0));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.MoveHorizontal) < 0)
                {
                    this.ApplyLinearImpulse(new Vector2(-5000, 0));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.LeftTrigger) > 0)
                {
                    this.ApplyAngularImpulse(-GameWorld.controller.ContainsFloat(ActionType.LeftTrigger));
                    //this.SetRotation(this.GetRotation() + GameWorld.controller.ContainsFloat(ActionType.LeftTrigger));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.RightTrigger) > 0)
                {
                    this.ApplyAngularImpulse(GameWorld.controller.ContainsFloat(ActionType.RightTrigger));
                    //this.SetRotation(this.GetRotation() - GameWorld.controller.ContainsFloat(ActionType.RightTrigger));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.LookHorizontal) != 0 || GameWorld.controller.ContainsFloat(ActionType.LookVertical) != 0)
                {
                    float rotationalFire = (float)Math.Atan2(GameWorld.controller.ContainsFloat(ActionType.LookHorizontal), GameWorld.controller.ContainsFloat(ActionType.LookVertical));
                    Weapon fireWeapon = null;
                    foreach (Weapon w in m_weapons.Values)
                    {
                        if (fireWeapon == null) fireWeapon = w;
                        else
                        {
                            float dif1 = Math.Abs((float) (rotationalFire - (w.GetRotation() % (2 * Math.PI))));
                            float dif2 = Math.Abs((float) (rotationalFire - (fireWeapon.GetRotation() % (2 * Math.PI))));
                            if (dif1 < dif2)
                            {
                                fireWeapon = w;
                            }
                        }
                    }
                    fireWeapon.SetFire(true);
                }
            }

            foreach (Weapon w in m_weapons.Values)
            {
                w.Update(dTime);
            }
        }

        public override void Draw()
        {
            Texture2D texture = TextureStatic.Get("4SideFriendlyRobot");
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

            foreach (Weapon w in m_weapons.Values)
            {
                w.Draw();
            }

        }
    }
}
