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
                Texture2D a = TextureStatic.Get(texture);
                uint[] data = new uint[a.Width * a.Height];
                a.GetData<uint>(data);
                Vertices v = Melkman.GetConvexHull(PolygonTools.CreatePolygon(data, a.Width, a.Height));
                computedSpritePolygons.Add(texture, v);
            }

            // Human Player

            // To change the mass of the robot, change the density parameter
            //  in the FixtureFactory.CreatePolygon call.  (is currently 10f)
            Body tempBody = BodyFactory.CreateBody(world);
            tempBody.BodyType = BodyType.Dynamic;
            Vector2[] edges = { new Vector2(-20, -20), new Vector2(20f, -20f), new Vector2(20f, 20f), new Vector2(-20f, 20f) };
            FixtureFactory.CreatePolygon(new Vertices(edges), 1f, tempBody);

            Bot testing = new Bot(this, tempBody, Bot.Player.Human, Bot.Type.FourSided);

            this.AddEntitiy(testing);

            // Enemy Robot
            Body enemy = BodyFactory.CreateBody(world);
            enemy.BodyType = BodyType.Dynamic;
            FixtureFactory.CreatePolygon(new Vertices(edges), 1f, enemy);
            enemy.SetTransform(new Vector2(400, 400), 0);
            Bot enemyBot = new Bot(this, enemy, Bot.Player.Computer, Bot.Type.FourSided);

            this.AddEntitiy(enemyBot);

        }

        public World World { get { return m_World; } }

        public void AddEntitiy(Entity e)
        {
            this.m_Entities.Add(e.GetID(), e);
        }

        public void Update(float dTime)
        {
            //call m_World.Step() first, to update the physics
            for (int i = 0; i < m_Entities.Values.Count; i++)
            {
                m_Entities.Values.ElementAt(i).Update(dTime);
            }

            /*foreach (Entity e in m_Entities.Values)
            {
                e.Update(dTime);
            }*/

            //then call the entity updates, take damage, listen to controls, spawn any projectiles, etc.

            //check for dead bots
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
