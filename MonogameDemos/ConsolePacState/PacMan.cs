using System;

namespace PacMan
{
    public enum PacManState { Spawning, Still, Chomping, SuperPacMan }

    
    //POCO Plain old CLR Object
    //POJO Plain old Java Object
    public class PacMan
    {

        protected PacManState _state;  //Private instance Data Memeber
        public PacManState State
        {
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

        public PacMan()
        {

            //Set default state will call notify so make sure this.Ghosts is intitialized first
            this.State = PacManState.Still;

        }

        //Extra method for logging state change
        public virtual void Log(string s)
        {
            //nothing
            Console.WriteLine(s);
        }

        public virtual void PowerUP()
        {
            this.State = PacManState.SuperPacMan;
        }
    }
}
