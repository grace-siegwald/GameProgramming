using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationStrategyWeapons
{
    public class Sword : HandToHand
    {
        public Sword()
        {
            this.name = "Sword";
            this.verb = "swing";
            this.damage = 7;
            this.WeaponType = WeaponType.HandToHand;

        }
    }
}
