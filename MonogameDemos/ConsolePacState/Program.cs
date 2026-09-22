using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacMan;

namespace ConsolePacState
{
    class Program
    {
        static void Main(string[] args)
        {
            PacMan.PacMan pac = new PacMan.PacMan();
            pac.State = PacManState.Chomping;

            pac.PowerUP();
            pac.State = PacManState.Chomping;

            Console.ReadKey();
        }
    }
}
