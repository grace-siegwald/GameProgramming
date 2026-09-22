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
    class RedWeapon : foodWeapon
    {
        public RedWeapon(Game game)
            : base(game)
        {
            this.Name = "red weapon";
            this.Verb = "rec chomp";
            this.color = Color.Red;
        }
    }
}
