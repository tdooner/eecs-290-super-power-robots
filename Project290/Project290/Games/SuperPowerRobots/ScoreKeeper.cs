using System;

namespace Project290.Games.SuperPowerRobots
{
    class ScoreKeeper
    {
        static int score { get; private set; }
        static int money { get; private set; }

        public ScoreKeeper()
        {

        }
        public ScoreKeeper(bool reset)
        {
            if (reset)
            {
                score = 0;
                money = 0;
            }
        }
        public static int AddScore(int dScore)
        {
            score += dScore;
            return score;
        }
        public static int AddMoney(int dMoney)
        {
            money += dMoney;
            return money;
        }
    }
}