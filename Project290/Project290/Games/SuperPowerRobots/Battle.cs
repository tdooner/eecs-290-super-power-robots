using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna;
using Project290.Games.SuperPowerRobots.Entities;
using System.IO;

namespace Project290.Games.SuperPowerRobots
{
    public class Battle
    {
        private XmlReader reader;

        private XmlDocument xmlDoc;

        private SortedDictionary<ulong, Entity> m_Entities;

        public Battle()
        {
            xmlDoc = new XmlDocument();

            xmlDoc.Load("Games/SuperPowerRobots/Storage/Battles.xml");

            XmlNodeList Enemies = xmlDoc.GetElementsByTagName("Enemy");

            Console.WriteLine(Enemies[0].InnerText);
        }

        public SortedDictionary<ulong, Entity> Entities()
        {
            return this.m_Entities;
        }
    }
}
