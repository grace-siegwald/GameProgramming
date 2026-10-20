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

/*
 * After I built the core features of the game, I had Claude help me build an Animation system to spice things up.
 * The conversation can be found in "OneButtonGame\Kirby_Animation_States_Conversation.md\"
 * But a quick rundown:
 * 
 * Classes: 
 *      Animation - Just handles the data (the frames, how fast it goes, and whether it loops)
 *      AnimationManager - Same concept as the GameManager, choses *which* animation is playing, which frame, and the timer
 *      Kirby - Decides what state he’s in, tells the manager what to play, and shows the current frame
 * 
 * Have it split up this way so that only KIRBY knows about kirby. Animation and AnimationManager know nothing about kirby,
 * so any other class could use them (wave perhaps???)
 */
namespace OneButtonGame
{
    public enum KirbyState
    {
        Grounded,
        Ascending,
        Falling,
        Hurt
    }
    public class Kirby : DrawableSprite
    {
        WaterFloor water; // needs to know about the water for the floor

        // Lives and such :)
        public int Lives {  get; private set; }
        public int StartLives = 3;
        public float HurtTime = 1f; // how long he stays in the Hurt state, 1 second for now, can be changed mayhaps
        private float hurtTimer = 0; // seconds left in the Hurt state

        // Physics and state
        public float Gravity = 8f;
        public float JumpStrength = 5f; // the strength of a FULL jump (holding button)
        public float JumpCutSpeed = 2f; // the speed of a jump after letting go of space early, allows for two jump variations (lower = shorter tap jumps)
        public bool OnFloor {  get; private set; }

        // Animations and animation manager! Kirby decides what to play, the animator handes the "how"
        public KirbyState State { get; private set; } // encapsulated because why not bruv
        AnimationManager animator = new AnimationManager(); 
        Animation groundedAnim;
        Animation ascendingAnim;
        Animation fallingAnim;
        Animation hurtAnim;

        public Kirby(Game game, WaterFloor water) : base(game)
        {
            //ShowMarkers = true; // uncomment to turn on sprite markers
            this.water = water; // grabbing reference to water floor because we neeeeeeed it!
            Scale = 2f; // scale to match the widow size
        }

        protected override void LoadContent()
        {
            Location = new Vector2(-100, 100); // just spawn away from screen lil dude
            
            // --- Grounded Animation ---
            Texture2D[] groundedFrames =
            {
                Game.Content.Load<Texture2D>("Kirby/ride0"), Game.Content.Load<Texture2D>("Kirby/ride1"), Game.Content.Load<Texture2D>("Kirby/ride2")
            };
            groundedAnim = new Animation(groundedFrames, 0.15f, true);
            
            // --- Ascending Animation ---
            Texture2D[] ascendingFrames =
            {
                Game.Content.Load<Texture2D>("Kirby/rise0"), Game.Content.Load<Texture2D>("Kirby/rise1"), Game.Content.Load<Texture2D>("Kirby/rise2")
            };
            ascendingAnim = new Animation(ascendingFrames, 0.08f, false);

            // --- Falling Animation ---
            Texture2D[] fallingFrames =
            {
                Game.Content.Load<Texture2D>("Kirby/fall0"), Game.Content.Load<Texture2D>("Kirby/fall1"), Game.Content.Load<Texture2D>("Kirby/fall2")
            };
            fallingAnim = new Animation(fallingFrames, 0.08f, false);

            // --- Hurt Animation --- 
            Texture2D[] hurtFrames =
            {
                Game.Content.Load<Texture2D>("Kirby/hurt0"), Game.Content.Load<Texture2D>("Kirby/hurt1")
            };
            hurtAnim = new Animation(hurtFrames, 0.1f, true);

            SpriteTexture = groundedAnim.Frames[0];   // needs a texture BEFORE base.LoadContent sets the Origin so that it actually has a texture to assess lol
            base.LoadContent();
        }
        public void Reset() // Resets kirby for another round of WAVES BABY, all the stuff that puts bro in place
        {
            Lives = StartLives;
            this.Enabled = true;
            Location = new Vector2(GraphicsDevice.Viewport.Width / 4, water.Surface - 100);
            ChangeState(KirbyState.Grounded);
            Direction = Vector2.Zero;
            Speed = 100;
            OnFloor = false;
            hurtTimer = 0;
        }

        public void Hit() // this what happens when he gets hit, called by the manager
        {
            Lives--; // loses a life
            hurtTimer = HurtTime;
        }
        public void Jump()
        {
            if (OnFloor)
            {
                Direction.Y = -JumpStrength; // -y is UP
                OnFloor = false;
            }
        }
        public void ReleaseJump()
        {
            if (Direction.Y < -JumpCutSpeed) // if kirby is "rising" faster than JumpCutSpeed (negative because -y is up)
            {
                Direction.Y = -JumpCutSpeed;
            }
        }
        public override void Update(GameTime gameTime)
        {
            // -- Gravity, Movement, and Floor Collision --
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!OnFloor)
            {
                Direction.Y += Gravity * time; // Gravity Logic
            }
            Location += Direction * Speed * time; // Movement Logic
            base.Update(gameTime); // must call the base update HERE to make sure it's drawn in right place
            UpdateFloorCollision(); 

            // --- State Stuff ---
            if (hurtTimer > 0)
            {
                hurtTimer -= time;
            }
            UpdateState(); // THREE IMPORTANT THINGS: deciding what state we're in...
            animator.Update(time); // ...moving to the next frame with the animator...
            SpriteTexture = animator.CurrentFrame; // ...and setting the texture to the current frame's texture
        }
        private void UpdateState()
        {
            KirbyState newState;

            if (hurtTimer > 0) // if hurt timer is greater than zero we know he's been hurt!
            {
                newState = KirbyState.Hurt;
            }
            else if (OnFloor)
            {
                newState = KirbyState.Grounded;
            }
            else if (this.Direction.Y < 0) // if direction is negitive, otherwise know as UP
            {
                newState = KirbyState.Ascending;
            } 
            else // last posibility, no need to define conditions!
            {
                newState = KirbyState.Falling;
            }

            if (newState != State) // only restarts the animation when the state ACTUALLY CHANGES
            {
                ChangeState(newState);
            }
        }
        private void ChangeState(KirbyState newState)
        {
            State = newState;
            animator.Play(GetAnimation(newState));
        }
        private Animation GetAnimation(KirbyState state)
        {
            switch (state)
            {
                case KirbyState.Hurt:
                    return hurtAnim;
                case KirbyState.Falling:
                    return fallingAnim;
                case KirbyState.Ascending:
                    return ascendingAnim;
                default:
                    return groundedAnim;
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
