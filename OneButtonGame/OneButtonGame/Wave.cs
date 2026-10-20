using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;

namespace OneButtonGame
{
    // ONE wave. It only knows how to move, how big it is, and when it has left the screen.
    // WaveSpawner decides WHEN waves go out and HOW FAST they move.
    public class Wave : DrawableSprite
    {
        WaterFloor water;
        Random random = new Random();

        public bool Active { get; private set; }    // true while this wave is out on the water
        public bool Counted { get; private set; }   // already hit Kirby or been jumped

        public float MinScale = 1.2f;
        public float MaxScale = 2f;

        public Wave(Game game, WaterFloor water) : base(game)
        {
            //ShowMarkers = true;
            this.water = water;
            Scale = MaxScale;
        }

        protected override void LoadContent()
        {
            SpriteTexture = Game.Content.Load<Texture2D>("wave");
            base.LoadContent();

            Direction = new Vector2(-1, 0);   // always moves left, toward Kirby
            Despawn();                        // every wave starts hidden, waiting to be sent out
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Location += Direction * Speed * time;

                if (Location.X < -Origin.X * Scale)   // all the way off the left side
                {
                    Despawn();   // back into the pool, ready to be reused
                }
            }
            base.Update(gameTime);
        }

        // Send this wave out from just past the right edge
        public void Spawn(float speed)
        {
            Speed = speed;
            Scale = MinScale + (float)random.NextDouble() * (MaxScale - MinScale);   // random size

            // size FIRST, then position (both use Scale)
            Location.X = Game.GraphicsDevice.Viewport.Width + Origin.X * Scale;
            Location.Y = water.Surface - Origin.Y * Scale;

            SetTranformAndRect();   // update the collision rectangle NOW, not next frame (see the note below)

            Counted = false;
            Active = true;
            Visible = true;
        }

        // Take this wave off the water
        public void Despawn()
        {
            Active = false;
            Visible = false;   // Visible = false doesn't get drawn
        }

        public void MarkCounted()
        {
            Counted = true;
        }
    }
}
