using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OneButtonGame
{
    // THIS is the only class that is friends with both the wave and Kirby himself. That's why it's the manager class. And it's paid better than Kirby. Because manager
    // and also like most managers they're not doing much, just "watching" and "overseeing" or whatever they do all day
    public class WaveRideManager : GameComponent
    {
        Kirby kirby;
        Wave wave;

        public int Score; // I just thought it made sense to put the score here but it could also be in Game class....
        public WaveRideManager(Game game, Kirby kirby, Wave wave) : base(game)
        {
            this.kirby = kirby;
            this.wave = wave;
        }

        public override void Update(GameTime gameTime)
        {
            CheckHit();
            CheckJumped();

            Game.Window.Title = $"Score: {Score}   Lives: {kirby.Lives}";

            base.Update(gameTime);
        }

        private void CheckHit()
        {
            if (wave.Counted) {return;} // we get outa here if the wave has already been counted (either hit kirby or jumped over)

            // hitbox intersection check first, then pixel collision check because that wave is not a rectangle lol
            if (kirby.Intersects(wave) && kirby.PerPixelCollisionWTansform(wave))
            {
                kirby.Hit();    
                wave.Counted = true; //
            }
        }
        private void CheckJumped()
        {
            if (wave.Counted) {return;} // once again, we get outa here

            // if kirby's left edge is PAST the wave's right edge...
            if (kirby.LocationRect.Left > wave.LocationRect.Right) 
            {
                Score++; // increment the score
                wave.Counted = true; 
            }
        }
    }
}
