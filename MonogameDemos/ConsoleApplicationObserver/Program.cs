using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationObserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Pacman p = new Pacman();
            Ghost redGhost = new Ghost(p);
            Ghost tealGhost = new Ghost(p);

            Console.WriteLine(p.MyState);

            p.MyState = PacState.Chomping;

            tealGhost.Die();

            p.MyState = PacState.Dying;

            Console.ReadKey();
        }
    }
}
