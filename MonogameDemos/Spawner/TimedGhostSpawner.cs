using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spawner
{
    class TimedGhostSpawner : TimedSpawner
    {

        Ghost.MonogameGhost ghost;

        public TimedGhostSpawner(Game game) : base(game) {
            this.ghost = new Ghost.MonogameGhost(game);
            this.SetSpawnKey(Microsoft.Xna.Framework.Input.Keys.S);
        }

        public override GameComponent Spawn()
        {
            ghost = new Ghost.MonogameGhost(this.Game);
            ghost.Initialize();
            ghost.Location = this.GetRandLocation(ghost.spriteTexture);
            ghost.Direction = this.GetRandomDirection();
            this.instance = ghost;
            return base.Spawn();
        }

    }
}
