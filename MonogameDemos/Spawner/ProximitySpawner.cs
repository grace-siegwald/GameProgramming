using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spawner
{

    enum ProximitySpawnerType {  SingleSpawn, ContinousWithCoolDown, Reset };

    class ProximitySpawner : TimedSpawner
    {

        public float DistanceToSpawn { get; set; }
        public Vector2 Location { get; set; }

        public Sprite SpriteToCheck { get; set; }

        public ProximitySpawnerType ProximitySpawnerType { get; set; } 


        public ProximitySpawner(Game game, Sprite spriteToCheck,
            ProximitySpawnerType proximitySpawnerType = ProximitySpawnerType.SingleSpawn,
            float distanceToSpawn = 100) : base(game)
        {
            this.DistanceToSpawn = distanceToSpawn;
            this.SpriteToCheck = spriteToCheck;
            this.ProximitySpawnerType = ProximitySpawnerType;

            //Additional setup based on type
            switch (ProximitySpawnerType)
            {
                case ProximitySpawnerType.ContinousWithCoolDown:
                    //set timeout to spanw right away
                    this.SpawnTime = this.lastSpawn;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(Vector2.Distance(this.Location, SpriteToCheck.Location) > this.DistanceToSpawn)
            {
                switch(this.ProximitySpawnerType)
                {
                    case ProximitySpawnerType.ContinousWithCoolDown :
                        base.Update(gameTime);
                    break;
                }
                
            }
            
        }


    }
}
