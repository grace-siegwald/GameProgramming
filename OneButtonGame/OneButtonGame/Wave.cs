using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;

namespace OneButtonGame
{
    public class Wave : DrawableSprite
    {
        // TODO: add a wave sprite that moves across the water floor and interacts with Kirby when it collides with him

        WaterFloor water;
        Random random = new Random();

        public float StartSpeed = 200f; // speed at the start of the game
        public float SpeedIncrease = 20f; // incrememnt of speed increase per wave
        public float SpeedCap = 400f; // so we don't get TOO crazy ykwim :0
        public bool Counted; // keep track of whether kirby has hit the wave or jumped over it, needed for counting score and so getting hit only takes ONE life :)

        public Wave(Game game, WaterFloor water) : base(game)
        {
            this.water = water;
            Scale = 2f;
            ShowMarkers = true;
        }
        protected override void LoadContent()
        {
            SpriteTexture = Game.Content.Load<Texture2D>("wave");
            base.LoadContent();

            Direction = new Vector2(-1, 0); // Facing the left
            Reset();
        }
        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Location += Direction * Speed * time; // move the wave to the left
            if (Location.X < -this.Origin.X * Scale) // if the wave is off the screen...
            {
                // And we're not at the speed cap yet...
                if (this.Speed < SpeedCap)
                {
                    // Make the wave faster and send it again!
                    Speed += SpeedIncrease;
                    NextWave();
                }
                else
                {
                    NextWave();
                }
            }
            base.Update(gameTime);
        }
        public void Reset() // setting back to "defualt" state
        {
            Speed = StartSpeed;
            NextWave();
        }
        private void NextWave()
        {
            this.Location.X = Game.GraphicsDevice.Viewport.Width + this.Origin.X * Scale + random.Next(0, 200); // adding random so that the pattern of waves is less predictable
            Location.Y = water.Surface - Origin.Y * Scale;  
            Counted = false;
        }
    }
}
