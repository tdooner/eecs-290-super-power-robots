using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project290.Physics.Dynamics;
using Project290.Rendering;

namespace Project290.Games.SuperPowerRobots.Entities
{
    public class Weapon : Entity
    {
        private Bot m_owner;

        public Weapon(SPRWorld sprWorld, Body body, Bot bot, float rotation)
            : base(sprWorld, body)
        {
            this.SetRotation(rotation);
            this.m_owner = bot;
        }
        //Weapons can spawn Projectiles, attached or unattached.

        public override void Update(float dTime)
        {
        }

        public override void Draw()
        {
            Texture2D texture = TextureStatic.Get("BlankSide");
            Drawer.Draw(
                texture,
                this.GetPosition(),
                null,
                Color.White,
                this.GetRotation(),
                new Vector2(texture.Width / 2, texture.Height / 2),
                1f,
                SpriteEffects.None,
                0f);
        }
    }
}
