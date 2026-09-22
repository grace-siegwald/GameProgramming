using System;


namespace PacMan
{
    public enum PacManState { Spawning, Still, Chomping, SuperPacMan, Dying, Dead }

    public class PacMan
    {

        protected PacManState _state;
        public PacManState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    this.Log(string.Format("{0} was: {1} now {2}", this.ToString(), _state, value));
                    //react
                    _state = value;
                }
            }
        }

        
        public void MovePacMan()
        {
            this.State = PacManState.Chomping;

        }

        public void KillPacMan()
        {

            this.State = PacManState.Dying;
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
    }
}
