using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationObserver
{
    public interface IPacmanObserver : IObserver
    {
        void Update(PacState p);
    }
}
