using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotManager.Weapons
{
    internal class ChainGun : RateLimitedShotManager
    {
        public ChainGun(Game game) : base(game)
        {
            this.LimitShotRate = .02f;
            this.MaxShots = 10;
        }
    }
}
