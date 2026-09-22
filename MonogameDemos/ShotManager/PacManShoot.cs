using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ShotManager.Weapons;

namespace ShotManager
{
    class PacManShoot : PacMan
    {

        public ShotManager SM;

        public PacManShoot(Game game) : base(game)
        {
            SM = new ChainGun(this.Game);
            //if (SM is RateLimitedShotManager)
            //{
            //    ((RateLimitedShotManager)SM).LimitShotRate = .5f;
            //    ((RateLimitedShotManager)SM).MaxShots = 3;
            //}
            this.Game.Components.Add(SM);
        }

        public override void Update(GameTime gameTime)
        {
            if(input.KeyboardState.HasReleasedKey(Microsoft.Xna.Framework.Input.Keys.Space))
            { 
                Shot s = new Shot(this.Game);
                s.Location = this.Location;
                s.Direction = this.lastDirection;
                s.Speed = 600;
                SM.Shoot(s);
            }
            if (input.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
            {
                Shot s = new Shot(this.Game);
                s.Location = this.Location;
                s.Direction = this.lastDirection;
                s.Speed = 600;
                SM.Shoot(s);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            SM.Draw(sb);
            base.Draw(sb);
        }
    }
}
