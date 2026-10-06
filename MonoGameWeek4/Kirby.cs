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
        public float JumpStrength = 5f;
        public bool OnFloor;

        public Kirby(Game game, WaterFloor water) : base(game)
        {
            ShowMarkers = true;
            this.water = water; // grabbing reference to water floor because we neeeeeeed it!
            Scale = 2f; // scale to match the widow size
        }

        protected override void LoadContent()
        {
            input = Game.Services.GetService<IInputHandler>();
            SpriteTexture = Game.Content.Load<Texture2D>("kirbyPink");
            base.LoadContent();
            Speed = 100;
            Direction = Vector2.Zero;
            Location = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            UpdateJumpInput();
            if (!OnFloor)
            {
                Direction.Y += Gravity * (time / 1000); // Gravity Logic
            }
            Location += Direction * Speed * (time / 1000); // Movement Logic

            base.Update(gameTime); // must call the base update HERE to make sure it's drawn in right place

            UpdateFloorCollision();
            UpdateSpriteColor(); // updates the sprite color based on whether Kirby is on the floor or not
        }

        private void UpdateJumpInput()
        {
            if (input.WasKeyPressed(Keys.Space) && OnFloor)
            {
                Direction.Y = -JumpStrength; // Jump Logic, -because we want to go up which is -y
                OnFloor = false;
            }
        }

        private void UpdateFloorCollision()
        {
            if (Direction.Y >= 0 && Intersects(water)) // if Kirby is "falling" and his rectangle intersects with the water rectangle
            {
                // gets the overlapping rectangle between Kirby and the water
                // (not really sure how the Intersection method works, just read the description in the Sprite class...)
                Rectangle overlap = Intersection(LocationRect, water.LocationRect);

                // Sets location to the top of the water minus the sink depth
                Location.Y -= overlap.Height - water.SinkDepth;

                Direction.Y = 0; // stops bro from jittering, direction must be set to 0 when he hits the floor
                OnFloor = true; // needs to be true so Kirby can jump again, otherwise he would be STUCK
            }
        }

        private void UpdateSpriteColor()
        {
            if (OnFloor)
            {
                SpriteTexture = Game.Content.Load<Texture2D>("kirbyRed");
            }
            else
            {
                SpriteTexture = Game.Content.Load<Texture2D>("kirbyPink");
            }
        }
    }
}
