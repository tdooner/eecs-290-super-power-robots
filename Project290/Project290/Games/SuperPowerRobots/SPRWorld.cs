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
using Project290.Physics.Common;
using Project290.Physics.Common.ConvexHull;
using Project290.Games.SuperPowerRobots.Controls;
using Project290.Physics.Dynamics.Contacts;

namespace Project290.Games.SuperPowerRobots
{
    public class SPRWorld
    {
        private World m_World;
        private SortedDictionary<ulong, Entity> m_Entities;
        public static Dictionary<String, Vertices> computedSpritePolygons = new Dictionary<string,Vertices>();
        //private Entity testing;

        public SPRWorld(World world)
        {
            m_World = world;
            this.m_Entities = new SortedDictionary<ulong,Entity>();

            // Make polygons out of weapons and anything that needs collision.
            // NOTE: Stores the convex hull for each item, since collision detection
            //          relies upon these verticies being convex polygons.
            String[] toPreload = new String[] { "Gun", "Axe", "Shield" };

            foreach (String texture in toPreload) {
                //Texture2D a = TextureStatic.Get(texture);
                //uint[] data = new uint[a.Width * a.Height];
                //a.GetData<uint>(data);
                //Vertices v = Melkman.GetConvexHull(PolygonTools.CreatePolygon(data, a.Width, a.Height));

                // For testing:
                Vertices v = new Vertices(new Vector2[] { new Vector2(-10, -62), new Vector2(10f, -62f), new Vector2(10f, 62f), new Vector2(-10f, 62f) });

                computedSpritePolygons.Add(texture, v);
            }
            Vector2[] edges = { new Vector2(-31, -31) * Settings.MetersPerPixel, new Vector2(31f, -31f) * Settings.MetersPerPixel, new Vector2(31f, 31f) * Settings.MetersPerPixel, new Vector2(-31f, 31f) * Settings.MetersPerPixel };

            // Human Player

            // To change the mass of the robot, change the density parameter
            //  in the FixtureFactory.CreatePolygon call.  (is currently 10f)
            Body tempBody = BodyFactory.CreateBody(world);
            tempBody.BodyType = BodyType.Dynamic;
            Fixture f = FixtureFactory.CreatePolygon(new Vertices(edges), 10f, tempBody);
            f.OnCollision += MyOnCollision;
            Bot testing = new Bot(this, tempBody, Bot.Player.Human, Bot.Type.FourSided, new HumanAI(this), TextureStatic.Get("4SideFriendlyRobot"), 31 * Settings.MetersPerPixel, 31 * Settings.MetersPerPixel);

            this.AddEntity(testing);

            // Enemy Robot
            Body enemy = BodyFactory.CreateBody(world);
            enemy.BodyType = BodyType.Dynamic;
            Fixture g = FixtureFactory.CreatePolygon(new Vertices(edges), 10f, enemy);
            g.OnCollision += MyOnCollision;
            enemy.SetTransform(new Vector2(400, 400) * Settings.MetersPerPixel, 0);
            Bot enemyBot = new Bot(this, enemy, Bot.Player.Computer, Bot.Type.FourSided, new BrickAI(this), TextureStatic.Get("4SideEnemyRobot"), 31 * Settings.MetersPerPixel, 31 * Settings.MetersPerPixel);

            this.AddEntity(enemyBot);

        }

        public World World { get { return m_World; } }

        public void AddEntity(Entity e)
        {
            this.m_Entities.Add(e.GetID(), e);
        }

        public bool MyOnCollision(Fixture f1, Fixture f2, Contact c)
        {
            //f2.Body.Dispose();
            return true;
        }

        public void Update(float dTime)
        {
            //then call the entity updates, take damage, listen to controls, spawn any projectiles, etc.

            //update all entities
            for (int i = 0; i < m_Entities.Values.Count; i++ )
            {
                m_Entities.Values.ElementAt(i).Update(dTime);
            }
            
            //check for dead bots
            List<ulong> toRemove = new List<ulong>();

            foreach (KeyValuePair<ulong, Entity> a in m_Entities)
            {
                Entity e = a.Value;
                // If the physics library has gotten rid of that entity's body, we should get rid of it as an entity.
                if (!m_World.BodyList.Contains(e.Body)) 
                {
                    toRemove.Add(a.Key);
                }
            }
            foreach (ulong key in toRemove)
            {
                m_Entities.Remove(key);
            }

        }

        public void Draw()
        {
            foreach (Entity e in m_Entities.Values)
            {
                e.Draw();
            }
        }
    }
}
