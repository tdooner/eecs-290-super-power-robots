using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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
using Project290.Physics.Common.PolygonManipulation;

namespace Project290.Games.SuperPowerRobots
{
    public class Battle
    {
        private XmlDocument xmlDoc;

        private SortedDictionary<ulong, Entity> m_Entities;

        public Battle(SPRWorld sprWorld, int botHalfWidth, World world)
        {
            this.m_Entities = new SortedDictionary<ulong, Entity>();

            Vector2[] edges = { new Vector2(-botHalfWidth, -botHalfWidth) * Settings.MetersPerPixel, new Vector2(botHalfWidth, -botHalfWidth) * Settings.MetersPerPixel, new Vector2(botHalfWidth, botHalfWidth) * Settings.MetersPerPixel, new Vector2(-botHalfWidth, botHalfWidth) * Settings.MetersPerPixel };

            Body tempBody = BodyFactory.CreateBody(world);
            tempBody.BodyType = BodyType.Dynamic;
            Fixture f = FixtureFactory.CreatePolygon(new Vertices(edges), 10f, tempBody);
            f.OnCollision += MyOnCollision;
            f.Friction = .5f;
            f.Restitution = 0f;
            tempBody.SetTransform(new Vector2(200, 200) * Settings.MetersPerPixel, 0);
            Bot testing = new Bot(sprWorld, tempBody, Bot.Player.Human, Bot.Type.FourSided, new HumanAI(sprWorld), TextureStatic.Get("4SideFriendlyRobot"), 2 * botHalfWidth * Settings.MetersPerPixel, 2 * botHalfWidth * Settings.MetersPerPixel, 100);
            f.UserData = testing;

            sprWorld.AddEntity(testing);
            
            // Enemy Robot
            Body enemy = BodyFactory.CreateBody(world);
            enemy.BodyType = BodyType.Dynamic;
            Fixture g = FixtureFactory.CreatePolygon(new Vertices(edges), 10f, enemy);
            g.OnCollision += MyOnCollision;
            g.Friction = .5f;
            g.Restitution = 0f;
            enemy.SetTransform(new Vector2(600, 600) * Settings.MetersPerPixel, 0);
            Bot enemyBot = new Bot(sprWorld, enemy, Bot.Player.Computer, Bot.Type.FourSided, new ModeAI(sprWorld), TextureStatic.Get("4SideEnemyRobot"), 2 * botHalfWidth * Settings.MetersPerPixel, 2 * botHalfWidth * Settings.MetersPerPixel, 100);
            g.UserData = enemyBot;

            sprWorld.AddEntity(enemyBot);

            /*xmlDoc = new XmlDocument();

            xmlDoc.Load("Games/SuperPowerRoobts/Storage/Allies.xml");

            XmlNodeList Allies = xmlDoc.GetElementsByTagName("Bot");

            xmlDoc.Load("Games/SuperPowerRobots/Storage/Battles.xml");

            XmlNodeList Enemies = xmlDoc.GetElementsByTagName("Bot");

            Console.WriteLine(Enemies[0].InnerText);*/
        }

        public void AddEntity(Entity e)
        {
            this.m_Entities.Add(e.GetID(), e);
        }

        public SortedDictionary<ulong, Entity> AllEntities()
        {
            return this.m_Entities;
        }

        public bool MyOnCollision(Fixture f1, Fixture f2, Contact c)
        {
            //f2.Body.Dispose();
            return true;
        }
    }
}
