using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationObserver
{
    public class Pacman : IPacmanSubject
    {

        PacState myState;
        List<IPacmanObserver> _observers;

        public PacState MyState
        {
            get { return myState; }
            set
            {
                this.myState = value;
                Notify();
            }
        }

        public Pacman()
        {
            _observers = new List<IPacmanObserver>();
            MyState = PacState.Spawning;
            
        }

        
        #region ISubject Members

        public List<IPacmanObserver> observers
        {
            get
            {
                return _observers;
            }
            set
            {
                _observers = value;
            }
        }

        public void Attach(IPacmanObserver o)
        {
            _observers.Add(o);
        }

        public void Detach(IPacmanObserver o)
        {
            _observers.Remove(o);
        }

        public void Attach(IObserver o)
        {
            //_observers.Add(o);
        }

        public void Detach(IObserver o)
        {
            //_observers.Remove(o);
        }

        public void Notify()
        {
            foreach (IPacmanObserver o in _observers)
            {
                o.Update(this.MyState);
            }
        }

        

        #endregion

        
    }

    public enum PacState { Spawning, Chomping, SuperChomp, Dying };
}
