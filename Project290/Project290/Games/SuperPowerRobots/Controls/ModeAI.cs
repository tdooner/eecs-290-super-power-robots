using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project290.Games.SuperPowerRobots.Entities;
using Microsoft.Xna.Framework;

namespace Project290.Games.SuperPowerRobots.Controls
{
    class ModeAI:SPRAI
    {
        enum Mode
        {
            DEFENSE,
            RANGED,
            MELEE
        }

        enum Action
        {
            FLEE,
            CHARGE,
            DODGE
        }

        private Mode m_Mode;
        private Bot m_Self;
        private Bot m_Player;
        private float m_ModeTimeout;

        public ModeAI(SPRWorld world)
            : base(world)
        {
            foreach(Entity e in world.GetEntities())
            {
                if(e is Bot && ((Bot)e).IsPlayer())
                    m_Player = (Bot)e;
            }
            m_ModeTimeout = 0;
        }

        public override void Update(float dTime, Bot self)
        {
            m_ModeTimeout -= dTime;
            m_Self = self;
            this.chooseMode();
            Vector2 move = this.chooseMove();
            int side = chooseSide();

            float relRot = side * (float)Math.PI / 2f;
            float ownRot = self.GetRotation();

            Vector2 facing = new Vector2((float)Math.Cos(ownRot + relRot), (float)Math.Sin(ownRot + relRot));
            Vector2 desired = m_Player.GetPosition() - self.GetPosition();

            this.Spin = Math.Min(Math.Max(SPRWorld.SignedAngle(facing, desired) * 4, -1), 1) * .5f;
            this.Move = move;
            this.Weapons = chooseFire();
        }

        private void chooseMode()
        {
            if (m_Self.getHealth() >= .5 * m_Self.getMaxHealth() || m_Self.getHealth() < .2 * m_Self.getMaxHealth())
            {
                bool hasM = false;
                bool hasR = false;
                foreach (Weapon w in m_Self.GetWeapons())
                {
                    if (w != null)
                    {
                        if (w.GetWeaponType() == WeaponType.melee)
                            hasM = true;
                        else if (w.GetWeaponType() == WeaponType.gun)
                            hasR = true;
                    }
                }

                if (!hasM)
                {
                    if (!hasR)
                    {
                        m_Mode = Mode.DEFENSE;
                    }
                    m_Mode = Mode.RANGED;
                } else if (!hasR){
                    m_Mode = Mode.MELEE;
                } else if (m_ModeTimeout <= 0) {
                    m_Mode = (m_Mode == Mode.RANGED ? Mode.MELEE : Mode.RANGED);
                    m_ModeTimeout = new Random().Next(5) + 3;
                }
            }
            else
            {
                m_Mode = Mode.DEFENSE;
            }
        }

        private Vector2 chooseMove()
        {
            if (m_Mode == Mode.DEFENSE)
            {
                Vector2 toP = m_Player.GetPosition() - m_Self.GetPosition();
                Vector2[] corners = {new Vector2(300, 300) * Settings.MetersPerPixel, new Vector2(1620, 300) * Settings.MetersPerPixel, new Vector2(1620, 780) * Settings.MetersPerPixel, new Vector2(300, 780) * Settings.MetersPerPixel};
                List<Vector2> cornList = new List<Vector2>(corners);

                int bad = 0;
                for (int i = 0; i < cornList.Count; i++ )
                {
                    Vector2 pToCorn = cornList.ElementAt(i) - m_Player.GetPosition();
                    Vector2 badCorn = cornList.ElementAt(bad) - m_Player.GetPosition();
                    if (pToCorn.Length() < badCorn.Length()) bad = i;
                }

                cornList.RemoveAt(bad);

                Vector2[] toCorners = new Vector2[cornList.Count];

                int best = 0;
                for (int i = 0; i < toCorners.Length; i++)
                {
                    toCorners[i] = cornList[i] - m_Self.GetPosition();
                        if (Vector2.Dot(toCorners[i], toP) < Vector2.Dot(toCorners[best], toP))
                        {
                            best = i;
                        }
                }

                Vector2 move = toCorners[best];
                move.Normalize();
                return move;

                /*
                Vector2 toMid = new Vector2(960 * Settings.MetersPerPixel, 540 * Settings.MetersPerPixel) - m_Self.GetPosition();
                Vector2 pMid = new Vector2(960 * Settings.MetersPerPixel, 540 * Settings.MetersPerPixel) - m_Player.GetPosition();

                if (move.Length() > 300 * Settings.MetersPerPixel || Vector2.Dot(-move, toMid) <= 0)
                {
                    move.Normalize();
                    return move;
                }
                else if (pMid.Length() < 200 * Settings.MetersPerPixel)
                {
                    toMid.Normalize();
                    return -toMid;
                } else
                {
                    move.Normalize();
                    Vector2 sideStep = new Vector2(-move.Y, move.X);
                    return sideStep * Math.Sign(SPRWorld.SignedAngle(move, toMid));
                }*/
            } else if(m_Mode == Mode.RANGED)
            {
                Vector2 toP = m_Player.GetPosition() - m_Self.GetPosition();

                if (toP.Length() < 300 * Settings.MetersPerPixel)
                {
                    toP.Normalize();
                    return -toP;
                }
                else if (toP.Length() > 400 * Settings.MetersPerPixel)
                {
                    toP.Normalize();
                    return toP;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
            else if (m_Mode == Mode.MELEE)
            {
                Vector2 toP = m_Player.GetPosition() - m_Self.GetPosition();
                float[] angles = new float[4];

                int closest = 0;
                for (int i = 0; i < 4; i++)
                {
                    float rota = m_Player.GetRotation() + i * (float)Math.PI / 2;
                    float rotb = m_Player.GetRotation() + closest * (float)Math.PI / 2;
                    float a = Vector2.Dot(new Vector2((float)Math.Cos(rota), (float)Math.Sin(rota)), -toP);
                    float b = Vector2.Dot(new Vector2((float)Math.Cos(rotb), (float)Math.Sin(rotb)), -toP);

                    if (a > b) closest = i;
                }

                float best = m_Player.GetRotation() + closest * (float)Math.PI / 2;
                Vector2 rot = new Vector2((float)Math.Cos(best), (float)Math.Sin(best));
                float angle = SPRWorld.SignedAngle(rot, -toP);

                float rotation = 0;

                if (angle < 0)
                {
                    rotation = ((float)Math.PI + angle) / 2;
                }
                else
                {
                    rotation = (-(float)Math.PI + angle) / 2;
                }

                float angleToPlayer = (float)Math.Atan2(toP.Y, toP.X);
                toP.Normalize();
                Vector2 walk = toP + 2 * new Vector2((float)Math.Cos(angleToPlayer + rotation), (float)Math.Sin(angleToPlayer + rotation));
                walk.Normalize();
                return walk;
            }
            else
            {
                return Vector2.Zero;
            }
        }

        private bool[] chooseFire()
        {
            bool[] weaps = new bool[4];
            int side = chooseSide();
            if (side >= 0)
                weaps[side] = true;
            return weaps;
        }

        //choose the side of the bot to face towards the player, -1 if don't care
        private int chooseSide()
        {
            Vector2 toP = m_Player.GetPosition() - m_Self.GetPosition();
            if (m_Mode == Mode.DEFENSE || (m_Mode == Mode.MELEE && toP.Length() > 200 * Settings.MetersPerPixel))
            {
                int bestShield = -1;
                Weapon[] weapons = m_Self.GetWeapons();
                for (int i = 0; i < 4; i++)
                {
                    if (weapons[i] == null)
                        continue;
                    if (weapons[i].GetWeaponType() == WeaponType.shield && (bestShield < 0 || weapons[bestShield].GetWeaponType() != WeaponType.shield || weapons[i].GetHealth() > weapons[bestShield].GetHealth()))
                        bestShield = i;
                }

                //if no shields, just go for weapon with greatest health
                if (bestShield < 0)
                {
                    bestShield = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (weapons[i] == null || weapons[bestShield] == null)
                            continue;
                        if (weapons[i].GetHealth() > weapons[bestShield].GetHealth()) bestShield = i;
                    }
                }

                return bestShield;
            }
            else if (m_Mode == Mode.RANGED)
            {
                int bestGun = -1;
                Weapon[] weapons = m_Self.GetWeapons();
                for (int i = 0; i < 4; i++)
                {
                    if (weapons[i] == null || (bestGun >= 0 && weapons[bestGun] == null))
                        continue;
                    if (weapons[i].GetWeaponType() == WeaponType.gun && (bestGun < 0 || weapons[bestGun].GetWeaponType() != WeaponType.gun || weapons[i].GetPower() > weapons[bestGun].GetPower()))
                        bestGun = i;
                }

                return bestGun;
            }
            else if (m_Mode == Mode.MELEE)
            {
                int bestMelee = -1;
                Weapon[] weapons = m_Self.GetWeapons();
                for (int i = 0; i < 4; i++)
                {
                    if (weapons[i] == null)
                        continue;
                    if (weapons[i].GetWeaponType() == WeaponType.melee && (bestMelee < 0 || weapons[bestMelee].GetWeaponType() != WeaponType.melee || weapons[i].GetPower() > weapons[bestMelee].GetPower()))
                        bestMelee = i;
                }

                return bestMelee;
            }
            else
            {
                return 0;
            }
        }
    }
}
