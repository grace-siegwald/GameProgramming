using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationStrategyWeapons
{
    public class BFG : ProjectileWeapon
    {
        public BFG()
        {
            this.name = "BFG";
            this.verb = "shoot";
            this.damage = 10000000;
            this.WeaponType = WeaponType.Projectile;
        }
    }
}
