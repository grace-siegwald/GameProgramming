using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManWeaponsStrategy
{
    class GameConsolePacMan : PacMan
    {
        GameConsole console;
        public GameConsolePacMan()
        {
            this.console = null;
        }

        public GameConsolePacMan(GameConsole console)
        {
            this.console = console;
        }

        public override void Log(string s)
        {
            if (console != null)
            {
                console.GameConsoleWrite(s);
            }
            else
            {
                base.Log(s);
            }
        }
    }
}
