using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;

namespace MonoGameWeek4
{
    public class Kirby : DrawableSprite
    {
        IInputHandler input;
        WaterFloor water;

        public float Gravity = 6f;
        public float JumpStrength = 3.6f;
        public bool OnFloor;

        public Kirby(Game game, WaterFloor water) : base(game)
        {
            this.water = water; // grabbing reference to water floor because we neeeeeeed it!
            Scale = 2f; // scale to match the widow size
        }

        protected override void LoadContent()
        {
            input = Game.Services.GetService<IInputHandler>();
            SpriteTexture = Game.Content.Load<Texture2D>("kirbyPink");
            base.LoadContent();
            Location = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            UpdateJumpInput();
            Direction.Y += Gravity * (time / 1000); // Gravity Logic
            Location += Direction * Speed * (time / 1000); // Movement Logic

            base.Update(gameTime); // must call the base update HERE to make sure it's drawn in right place

            UpdateFloorCollision();
        }

        private void UpdateJumpInput()
        {
            if (input.WasKeyPressed(Keys.Space) && OnFloor)
            {
                Direction.Y = -JumpStrength; // Jump Logic
                OnFloor = false;
            }
        }

        private void UpdateFloorCollision()
        {
            // Only land while falling (not on the way up) and when touching the water
            if (Direction.Y >= 0 && Intersects(water))
            {
                Location.Y = water.Surface - Origin.Y;   // put Kirby's bottom on the surface
                Direction.Y = 0;
                OnFloor = true;
            }
        }
    }
}
