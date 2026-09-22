using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost
{
    class GameConsoleGhost : Ghost
    {
        GameConsole console;
        public GameConsoleGhost()
        {
            this.console = null;
        }

        public GameConsoleGhost(GameConsole console)
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
