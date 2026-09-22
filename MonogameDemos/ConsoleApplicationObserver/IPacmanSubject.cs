using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationObserver
{
    public interface IPacmanSubject : ISubject
    {
        List<IPacmanObserver> observers { get; set; }

        void Attach(IPacmanObserver o);

        void Detach(IPacmanObserver o);
    }
}
