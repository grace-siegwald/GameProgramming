using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplicationStrategyWeapons
{
    public interface IDamageable
    {
        int HP
        {
            get;
            set;
        }

        void TakeDamage(int DamageAmount);
    }
}
