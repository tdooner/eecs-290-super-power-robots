using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Screens;
using Microsoft.Xna.Framework;
using Project290.Rendering;
using Project290.GameElements;
using Project290.Inputs;
using Microsoft.Xna.Framework.Graphics;
using Project290.Clock;
using Project290.Physics.Dynamics;
using Project290.Physics.Collision.Shapes;
using Project290.Physics.Factories;
using Project290.Games.SuperPowerRobots.Entities;

namespace Project290.Games.SuperPowerRobots
{
    class SPRWorld
    {
        private World m_World;
        private SortedDictionary<ulong, Entity> m_Entities;
        //private Entity testing;

        public SPRWorld(World world)
        {
            m_World = world;
            this.m_Entities = new SortedDictionary<ulong,Entity>();


            Body tempBody = BodyFactory.CreateBody(world);
            tempBody.BodyType = BodyType.Dynamic;
            tempBody.Mass = 5f;
            tempBody.Inertia = 5f;

            Entity testing = new Entity(this, tempBody);

            this.AddEntitiy(testing);

        }

        public World World { get { return m_World; } }

        public void AddEntitiy(Entity e)
        {
            this.m_Entities.Add(e.GetID(), e);
        }

        public void Update(float dTime)
        {
            //call m_World.Step() first, to update the physics
            foreach (Entity e in m_Entities.Values)
            {

                if (GameWorld.controller.ContainsFloat(ActionType.MoveVertical) < 0)
                {
                    e.ApplyLinearImpulse(new Vector2(0, 5000));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.MoveVertical) > 0)
                {
                    e.ApplyLinearImpulse(new Vector2(0, -5000));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.MoveHorizontal) > 0)
                {
                    e.ApplyLinearImpulse(new Vector2(5000, 0));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.MoveHorizontal) < 0)
                {
                    e.ApplyLinearImpulse(new Vector2(-5000, 0));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.LeftTrigger) > 0)
                {
                    e.SetRotation(e.GetRotation() + GameWorld.controller.ContainsFloat(ActionType.LeftTrigger));
                }
                if (GameWorld.controller.ContainsFloat(ActionType.RightTrigger) > 0)
                {
                    e.SetRotation(e.GetRotation() - GameWorld.controller.ContainsFloat(ActionType.RightTrigger));
                }
            }

            //then call the entity updates, take damage, listen to controls, spawn any projectiles, etc.

            //check for dead bots
        }

        public void Draw()
        {
            foreach (Entity e in m_Entities.Values)
            {
                Drawer.Draw(TextureStatic.Get("4SideFriendlyRobot"), e.GetPosition(), null, Color.White, e.GetRotation(), Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
