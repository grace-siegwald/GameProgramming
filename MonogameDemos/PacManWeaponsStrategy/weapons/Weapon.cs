using StrategyPacMan.weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacManWeaponsStrategy
{
    public abstract class Weapon : IWeapon
    {
        //private instance data members
        protected int damage;
        protected string verb, name;

        //public WeaponType WeaponType;
        
        public int Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }

        public string Verb
        {
            get
            {
                return verb;
            }
            set
            {
                verb = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Use()
        {
            return string.Format("{0} that {1}", this.verb, this.name);
        }

        public string Use(IDamageable other)
        {
            other.TakeDamage(this.Damage);

            return this.Use();
        }
    }
}
