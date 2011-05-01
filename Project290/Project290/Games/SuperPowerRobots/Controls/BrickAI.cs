using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project290.Games.SuperPowerRobots.Controls
{
    public class BrickAI:SPRAI
    {
        public BrickAI(SPRWorld world)
            : base(world)
        { }

        public override void Update(float dTime, Entities.Bot self)
        {
            //sit there like a brick
        }
    }
}
