using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost
{

    public enum GhostState { Chasing, Evading, Roving, Dead }

    class Ghost
    {
        GhostState _state;
        public GhostState State {
            get { return _state; }
            set
            {
                    if (_state != value)
                    {
                        this.Log(string.Format("{0} was: {1} now {2}", this.ToString(), _state, value));

                        _state = value;
                    }
                }
        }

        public Ghost()
        {
            this._state = GhostState.Roving;
        }


        public void Evade()
        {
            this._state = GhostState.Evading;
        }

        //Extra method for logging state change
        public virtual void Log(string s)
        {
            //nothing
            Console.WriteLine(s);
        }

        
    }
}
