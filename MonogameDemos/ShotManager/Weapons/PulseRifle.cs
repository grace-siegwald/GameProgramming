using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotManager.Weapons
{
    internal class PulseRifle : RateLimitedShotManager
    {
        public PulseRifle(Game game) : base(game)
        {
            this.LimitShotRate = .1f;
            this.MaxShots = 3;
        }
    }
}
