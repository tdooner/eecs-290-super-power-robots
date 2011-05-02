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

        private SPRWorld sprWorld;

        private World world;

        private int botHalfWidth;

        public Battle(SPRWorld sprWorld, int botHalfWidth, World world)
        {
            this.sprWorld = sprWorld;
            this.botHalfWidth = botHalfWidth;
            this.world = world;

            xmlDoc = new XmlDocument();

            xmlDoc.Load("Games/SuperPowerRobots/Storage/Allies.xml");

            Vector2[] edges = { new Vector2(-botHalfWidth, -botHalfWidth) * Settings.MetersPerPixel, new Vector2(botHalfWidth, -botHalfWidth) * Settings.MetersPerPixel, new Vector2(botHalfWidth, botHalfWidth) * Settings.MetersPerPixel, new Vector2(-botHalfWidth, botHalfWidth) * Settings.MetersPerPixel };

            CreateBots(xmlDoc, edges);

            xmlDoc.Load("Games/SuperPowerRobots/Storage/Battles.xml");

            CreateBots(xmlDoc, edges);

            // Enemy Robot
            /*Body enemy = BodyFactory.CreateBody(world);
            enemy.BodyType = BodyType.Dynamic;
            Fixture g = FixtureFactory.CreatePolygon(new Vertices(edges), 10f, enemy);
            g.OnCollision += MyOnCollision;
            g.Friction = .5f;
            g.Restitution = 0f;
            g.UserData = SPRWorld.ObjectTypes.Bot;
            enemy.SetTransform(new Vector2(600, 600) * Settings.MetersPerPixel, 0);
            Bot enemyBot = new Bot(sprWorld, enemy, Bot.Player.Computer, Bot.Type.FourSided, new BrickAI(sprWorld), TextureStatic.Get("4SideEnemyRobot"), 2 * botHalfWidth * Settings.MetersPerPixel, 2 * botHalfWidth * Settings.MetersPerPixel, 100);

            sprWorld.AddEntity(enemyBot);*/
        }

        public void CreateBots(XmlDocument xmlDoc, Vector2[] edges)
        {
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("Bot");

            foreach (XmlNode botNode in nodes)
            {
                XmlNodeList innerNodes = botNode.ChildNodes;
                
                Bot.Player AIType;

                Console.WriteLine(innerNodes[0].InnerText);

                SPRAI control;

                switch (innerNodes[0].InnerText)
                {
                    case "HumanAI":
                        AIType = Bot.Player.Human;
                        control = new HumanAI(sprWorld);
                        break;
                    case "BrickAI":
                        AIType = Bot.Player.Computer;
                        control = new BrickAI(sprWorld);
                        break;
                    default:
                        AIType = Bot.Player.Human;
                        control = new HumanAI(sprWorld);
                        break;
                }

                float health = float.Parse(innerNodes[1].InnerText);
                Texture2D texture = TextureStatic.Get(innerNodes[2].InnerText);
                Vector2 position = new Vector2(int.Parse(innerNodes[3].InnerText), int.Parse(innerNodes[4].InnerText));

                Body tempBody = BodyFactory.CreateBody(world);
                tempBody.BodyType = BodyType.Dynamic;
                Fixture f = FixtureFactory.CreatePolygon(new Vertices(edges), 10f, tempBody);
                f.OnCollision += MyOnCollision;
                f.Friction = .5f;
                f.Restitution = 0f;
                f.UserData = SPRWorld.ObjectTypes.Bot;
                tempBody.SetTransform(position * Settings.MetersPerPixel, 0);

                Bot newBot = new Bot(sprWorld, tempBody, AIType, Bot.Type.FourSided, control, texture, 2 * botHalfWidth * Settings.MetersPerPixel, 2 * botHalfWidth * Settings.MetersPerPixel, health);

                sprWorld.AddEntity(newBot);
            }
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
