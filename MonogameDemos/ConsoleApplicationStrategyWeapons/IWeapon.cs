using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationStrategyWeapons
{
    public interface IWeapon
    {
        int Damage
        {
            get;
            set;
        }

        string Verb
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }

        string Use();
    }
}
