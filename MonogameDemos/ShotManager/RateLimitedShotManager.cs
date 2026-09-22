using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotManager
{
    class RateLimitedShotManager : ShotManager
    {
        protected float limitShotRateForMiliseconds, 
            limitShotRateMilisecondsTimer, 
            limitShotRate;
        public float LimitShotRate
        {
            get { return limitShotRate; }
            set
            {
                limitShotRate = value;
                limitShotRateForMiliseconds = limitShotRate * 1000;
            }
        } //limit shots to X per second set to 0 for no Rate limit

        public int MaxShots { get; set; } //Max shot in Collection at once set to 0 for unlimited


        public RateLimitedShotManager(Game game) : base(game)
        {
            //Nothing maybe write more constuctors with more options now all the prarmaters are set as propterties
        }

        public override Shot Shoot(Shot shot)
        {
            if (!CheckTimerToAllowShot()) return null;
            s = base.Shoot(shot);
            limitShotRateMilisecondsTimer = limitShotRateForMiliseconds;
            return s;
        }

        /// <summary>
        /// Returns True if ShotManager timer is under the value in limitShotRatetimer and max shots are less than the allowd value
        /// </summary>
        /// <returns></returns>
        private bool CheckTimerToAllowShot()
        {
            if (limitShotRate > 0)
            {
                if ((limitShotRateMilisecondsTimer > 0)
                    || (this.Shots.Count >= MaxShots)
                )
                {
                    return false;
                }
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            //If shot rate is limited use timer 0 means unlimited
            if (limitShotRateMilisecondsTimer > 0)
                this.limitShotRateMilisecondsTimer -= gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameTime);
        }
    }
}
