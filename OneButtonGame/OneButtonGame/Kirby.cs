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

namespace OneButtonGame
{
    public class Kirby : DrawableSprite
    {
        IInputHandler input;
        WaterFloor water;
        Texture2D defualtSprite;
        Texture2D hurtSprite;

        // Lives and such :)
        public int Lives;
        public int StartLives = 3;
        private float flashTimer = 0; // amount of time he flashes red sprite

        // Physics and state
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

            defualtSprite = Game.Content.Load<Texture2D>("kirbyPink");
            hurtSprite = Game.Content.Load<Texture2D>("kirbyRed");
            SpriteTexture = defualtSprite;

            base.LoadContent();

            Reset();
        }
        public void Reset() // Resets kirby for another round of WAVES BABY, all the stuff that puts bro in place
        {
            Lives = StartLives;
            Location = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2);
            Direction = Vector2.Zero;
            Speed = 100;
            OnFloor = false;
            flashTimer = 0; 
        }

        public void Hit() // this what happens when he gets hit, called by the manager
        {
            Lives--; // loses a life
            flashTimer = 1f; // he blinks for 1 second, can be changed later :)
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateJumpInput();
            if (!OnFloor)
            {
                Direction.Y += Gravity * time; // Gravity Logic
            }
            Location += Direction * Speed * time; // Movement Logic

            base.Update(gameTime); // must call the base update HERE to make sure it's drawn in right place

            UpdateFlash(time);
            UpdateFloorCollision();
        }

        public void UpdateFlash(float time)
        {
            if (flashTimer > 0) // only proceeds if timer has actually been set 
            {
                flashTimer -= time;

                // switches sprite every 0.1 seconds so he blinks - Wrote with the help of claude
                if ((int)(flashTimer * 10) % 2 == 0) // the % calculates the remainder after dividing the left and right side.
                {
                    SetTexture(hurtSprite);
                }
                else
                {
                    SetTexture(defualtSprite);
                }
            }
            else
            {
                SetTexture(defualtSprite);
            }
        }

        // just a helpful method so we only swap textures when... the texture actually changes
        // needed because we are setting texture in update loop, don't wanna do that 60 times per second!
        private void SetTexture(Texture2D texture)
        {
            if (SpriteTexture != texture)
            {
                spriteTexture = texture;
            }
        }

        // Checks if space is pressed (and that kirby is on the floor), and if so, sets the direction to jump up and sets OnFloor to false
        private void UpdateJumpInput()
        {
            if (input.WasKeyPressed(Keys.Space) && OnFloor)
            {
                Direction.Y = -JumpStrength; // Jump Logic, - because we want to go up it is -y
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
    }
}
