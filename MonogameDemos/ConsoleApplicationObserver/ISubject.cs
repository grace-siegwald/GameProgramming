using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationObserver
{
    public interface ISubject
    {

        void Attach(IObserver o);

        void Detach(IObserver o);

        void Notify();
    }
}
