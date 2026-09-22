using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Sprite;
using PacManWeaponsStrategy;

namespace StrategyPacMan.weapons
{
    class NoWeapon : foodWeapon
    {
        public NoWeapon(Game game)
            : base(game)
        {
            this.Name = "no weapon";
            this.Verb = "no verb";
            this.color = Color.White;
        }

        public override string Use()
        {
            return string.Empty; //base.Use();
        }
    }
}
