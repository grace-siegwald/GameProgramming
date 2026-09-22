using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationStrategyWeapons
{
    public class SpaceMarine : Character
    {
        public SpaceMarine()
        {
            this.Name = "Space Marine";
            this.Weapon = new BFG();
        }
    }
}
