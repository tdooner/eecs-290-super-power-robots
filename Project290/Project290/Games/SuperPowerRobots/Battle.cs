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

        public int winReward { get; private set; }

        public static string nextAllies = ""; // This will be populated so the user can customize their bot.

        public Battle(SPRWorld sprWorld, int botHalfWidth, World world, int currentLevel)
        {
            this.sprWorld = sprWorld;
            this.botHalfWidth = botHalfWidth;
            this.world = world;

            xmlDoc = new XmlDocument();

            if (nextAllies == "")
            {
                xmlDoc.Load("Games/SuperPowerRobots/Storage/Allies.xml");
            }
            else
            {
                xmlDoc.LoadXml(nextAllies);
            }

            //xmlDoc.

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("Bot");

            Vector2[] edges = { new Vector2(-botHalfWidth, -botHalfWidth) * Settings.MetersPerPixel, new Vector2(botHalfWidth, -botHalfWidth) * Settings.MetersPerPixel, new Vector2(botHalfWidth, botHalfWidth) * Settings.MetersPerPixel, new Vector2(-botHalfWidth, botHalfWidth) * Settings.MetersPerPixel };

            CreateBots(nodes, edges);

            xmlDoc.Load("Games/SuperPowerRobots/Storage/Battles.xml");

            nodes = xmlDoc.GetElementsByTagName("Level");

            nodes = nodes[currentLevel].ChildNodes;

            CreateBots(nodes, edges);
        }

        public void CreateBots(XmlNodeList nodes, Vector2[] edges)
        {
            Console.WriteLine(nodes.Count);

            foreach (XmlNode botNode in nodes)
            {
                if (botNode.Name == "Award")
                {
                    this.winReward = int.Parse(botNode.InnerText);
                    continue;
                }
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
                    case "ModeAI":
                        AIType = Bot.Player.Computer;
                        control = new ModeAI(sprWorld);
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
                f.Friction = .5f;
                f.Restitution = 0f;
                tempBody.SetTransform(position * Settings.MetersPerPixel, 0);

                Bot newBot = new Bot(sprWorld, tempBody, AIType, Bot.Type.FourSided, control, texture, 2 * botHalfWidth * Settings.MetersPerPixel, 2 * botHalfWidth * Settings.MetersPerPixel, health);
                f.UserData = newBot;

                for (int weaponNumber = 0; weaponNumber < 4; weaponNumber++)
                {
                    XmlNodeList weaponChilds = innerNodes[weaponNumber + 5].ChildNodes;
                    int side = weaponNumber;
                    WeaponType weaponType;
                    switch (weaponChilds[0].InnerText)
                    {
                        case "gun":
                            weaponType = WeaponType.gun;
                            break;
                        case "shield":
                            weaponType = WeaponType.shield;
                            break;
                        case "melee":
                            weaponType = WeaponType.melee;
                            break;
                        default:
                            weaponType = WeaponType.gun;
                            break;
                    }
                    String weaponTexture = weaponChilds[1].InnerText;
                    float weaponHealth = float.Parse(weaponChilds[2].InnerText);
                    float weaponPower = float.Parse(weaponChilds[3].InnerText);

                    newBot.AddWeapon(weaponNumber, weaponTexture, weaponType, weaponHealth, weaponPower);
                }

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
    }
}
