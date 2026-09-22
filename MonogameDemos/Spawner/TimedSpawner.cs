using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spawner
{
    class TimedSpawner : Spawner
    {
        protected float spawnTime, lastSpawn;

        public float SpawnTime
        {
            get { return spawnTime; }
            set { this.spawnTime = value; }
        }

        /// <summary>
        /// Set time it takes spawner to spawn new GameComponent
        /// </summary>
        /// <param name="timeTillNextSpawn">time in miliseonds till next GameComponent is spawned</param>
        public void SetSpawnTime(float timeTillNextSpawn)
        {
            this.spawnTime = timeTillNextSpawn;
        }

        public TimedSpawner(Game game )
            : base(game)
        {
            spawnTime = 3000;
            lastSpawn = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (lastSpawn > spawnTime)
            {
                this.Spawn();
                lastSpawn = 0;
            }

            lastSpawn += gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameTime);
        }
    }
}
