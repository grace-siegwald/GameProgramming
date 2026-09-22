using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationObserver
{
    public class Ghost : IPacmanObserver
    {
        IPacmanSubject pacSub;
        
        public Ghost(IPacmanSubject pac)
        {
            this.pacSub = pac;
            this.pacSub.Attach(this);
        }
        
        #region IObserver Members

        public void Update(PacState p)
        {
            Console.WriteLine(string.Format("{0} has been updated pacman state is {1}", this, p));
        }

        public void Update()
        {
            Console.WriteLine(string.Format("{0} has been updated", this));
        }

        #endregion

        public void Die()
        {
            this.pacSub.Detach(this);
        }
    }
}
