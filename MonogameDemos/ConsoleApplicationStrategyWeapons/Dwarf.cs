using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationStrategyWeapons
{
    public class Dwarf : Character
    {
        public Dwarf()
        {
            this.Name = "Dwarf";
            this.Weapon = new WarHammer();
        }
    }
}
