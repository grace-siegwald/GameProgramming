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
    abstract class foodWeapon : food , IWeapon
    {

        private int damage;
        private string verb, name;
        
        
        public foodWeapon(Game game)
            : base(game)
        {
            this.damage = 1;
            this.verb = "chomp";
            this.name = "pacman food weapon";
            
        }

        public int Damage
        {
            get
            {
                return this.damage;
            }
            set
            {
                this.damage = value;
            }
        }

        public string Verb
        {
            get
            {
                return this.verb;
            }
            set
            {
                this.verb = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public virtual string Use()
        {
            throw new NotImplementedException();
        }
    }
}
