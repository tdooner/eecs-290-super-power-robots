using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Project290.GameElements;
using Project290.Inputs;
using Project290.Games.SuperPowerRobots.Entities;

namespace Project290.Games.SuperPowerRobots.Controls
{
    class HumanAI:SPRAI
    {
        public HumanAI(SPRWorld world)
            : base(world)
        {

        }

        public override void Update(float dTime, Bot self)
        {
            this.Move = new Vector2(GameWorld.controller.ContainsFloat(ActionType.MoveHorizontal), GameWorld.controller.ContainsFloat(ActionType.MoveVertical));
            this.Fire = new Vector2(GameWorld.controller.ContainsFloat(ActionType.LookHorizontal), GameWorld.controller.ContainsFloat(ActionType.LookVertical));

            this.Spin = GameWorld.controller.ContainsFloat(ActionType.LeftTrigger) - GameWorld.controller.ContainsFloat(ActionType.RightTrigger);

            this.Weapons[0] = GameWorld.controller.ContainsBool(ActionType.BButton);
            this.Weapons[1] = GameWorld.controller.ContainsBool(ActionType.YButton);
            this.Weapons[2] = GameWorld.controller.ContainsBool(ActionType.XButton);
            this.Weapons[3] = GameWorld.controller.ContainsBool(ActionType.AButton);
        }
    }
}
