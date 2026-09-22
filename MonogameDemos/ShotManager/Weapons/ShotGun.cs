using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotManager.Weapons
{
    internal class ShotGun : RateLimitedShotManager
    {
        public ShotGun(Game game) : base(game)
        {
            this.LimitShotRate = 3;
            this.MaxShots = 1;
        }
    }
}
