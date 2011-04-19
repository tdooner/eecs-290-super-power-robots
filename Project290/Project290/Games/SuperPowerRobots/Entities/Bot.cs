using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project290.GameElements;
using Project290.Physics.Dynamics;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Factories;
using Project290.Inputs;
using Project290.Rendering;

namespace Project290.Games.SuperPowerRobots.Entities
{
    public class Bot : Entity
    {
        //Bots have four Weapons, the Bodies are attached via WeldJoints
        //private Weapon[] m_weapons;
        private CircleShape m_shape;
        //temp variable, do not include in final project
        private bool m_player;

        public Bot(SPRWorld sprWord, Body body, bool player)
            : base(sprWord, body)
        {
            this.m_player = player;
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
                    this.SetRotation(this.GetRotation() + GameWorld.controller.ContainsFloat(ActionType.LeftTrigger));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.RightTrigger) > 0)
                {
                    this.SetRotation(this.GetRotation() - GameWorld.controller.ContainsFloat(ActionType.RightTrigger));
                }
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
        }
    }
}
