using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationStrategyWeapons
{
    public class WarHammer : HandToHand
    {
        public WarHammer()
        {
            this.name = "War Hammer";
            this.verb = "swing";
            this.damage = 8;
            this.WeaponType = WeaponType.HandToHand;
        }
    }
}
