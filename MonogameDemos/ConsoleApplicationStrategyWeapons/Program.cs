using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationStrategyWeapons
{
    class Program
    {
        static void Main(string[] args)
        {
            Ninja n = new Ninja();
            SpaceMarine sm = new SpaceMarine();
            Dwarf d = new Dwarf();

            Console.WriteLine(sm.Attack(n));
            Console.WriteLine(n.About());
            n.Weapon = new BFG();
            Console.WriteLine(n.Attack(d));
            Console.WriteLine(d.About());
            Console.WriteLine(sm.About());


            Console.ReadKey();
        }
    }
}
