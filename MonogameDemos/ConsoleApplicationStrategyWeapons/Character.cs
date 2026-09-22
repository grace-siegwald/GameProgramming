using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationStrategyWeapons
{
    public abstract class Character : IDamageable
    {
        //instead of doing all those anoying private instance data mamabers

        public int HP { get; set; }

        protected Weapon weapon;
        
        public Weapon Weapon { 
            get
            {
                return weapon;
            }

            set
            {
                if (this.Weapons.Count < this.maxWeapons)
                {
                    this.Weapons.Add(value);
                    this.weapon = value;
                }
                else
                {

                }
                
            }
        }

        public List<Weapon> Weapons;
        private int maxWeapons;

        public string Name { get; set; }

        public Character()
        {
            this.HP = 10;
            this.maxWeapons = 10;
            
            Weapons = new List<Weapon>();
            this.Weapon = new Sword();
            
        }

        public void TakeDamage(int DamageAmount)
        {
            //maybe i care if this is negative
            //TODO Check if they are now undead
            this.HP -= DamageAmount;
        }

        public string About()
        {
            return string.Format("Hello I'm {0} and I have {1} HP and I have a {2}.", this.Name, this.HP, this.Weapon.Name);
        }

        public string Attack(IDamageable other)
        {
            return this.Name + " " + this.Weapon.Use(other);
        }
    }
}
