using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;

namespace GhostManagerDemo
{
    public enum GhostState
    {
        Roving, //moving around normally
        Bumped, //just hit another ghost (shows the hit texture for a moment)
        Dead //finished, GhostManager will remove it
    }

    // A single ghost. It contains and manages its own state:
    // textures, movement, bouncing off the screen edges, and what happens when it gets bumped.
    public class Ghost : DrawableSprite
    {
        // One Random shared by every ghost so ghosts made in the same frame don't get the same values
        static readonly Random random = new Random();

        public const float DefaultSpeed = 100f; // pixels per second
        const float BumpedTime = 0.25f; // seconds to show the hit texture after a bump

        //Textures
        readonly string ghostTextureName;
        Texture2D ghostTexture, hitTexture;

        //State
        GhostState state;
        float bumpedTimer;

        public GhostState State
        {
            get { return state; }
            protected set { state = value; }
        }

        public bool IsDead { get { return state == GhostState.Dead; } }

        public string GhostTextureName { get { return ghostTextureName; } }

        public Ghost(Game game) : this(game, "RedGhost") { }

        public Ghost(Game game, string ghostTextureName)
            : base(game)
        {
            this.ghostTextureName = ghostTextureName;
            this.state = GhostState.Roving;
        }

        protected override void LoadContent()
        {
            ghostTexture = Game.Content.Load<Texture2D>(ghostTextureName);
            hitTexture = Game.Content.Load<Texture2D>("GhostHit");

            //spriteTexture has to be set before base.LoadContent because Sprite uses it to center the Origin
            spriteTexture = ghostTexture;

            Speed = DefaultSpeed;
            Direction = GetRandomDirection();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (state == GhostState.Dead)
                return; // dead ghosts don't move, GhostManager removes them

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateState(elapsed);
            UpdateTexture();
            Move(elapsed);
            KeepOnScreen();

            base.Update(gameTime); // Sprite.Update rebuilds LocationRect, which is used for collision
        }


        // Bumped only lasts a short time, then the ghost goes back to Roving
        private void UpdateState(float elapsed)
        {
            if (state == GhostState.Bumped)
            {
                bumpedTimer -= elapsed;
                if (bumpedTimer <= 0)
                {
                    state = GhostState.Roving;
                }
            }
        }

        // Changes ghost texture based on state
        private void UpdateTexture()
        {
            Texture2D textureForState = (state == GhostState.Bumped) ? hitTexture : ghostTexture;
            if (spriteTexture != textureForState)
            {
                spriteTexture = textureForState;
            }
        }

        private void Move(float elapsed)
        {
            Location += Direction * Speed * elapsed;
        }

        // Bounce off the edges of the screen.
        // Location is the center of the ghost (Origin is centered), so use half the texture size.
        // Using Math.Abs instead of *= -1 means a ghost can never get stuck flipping back and forth on an edge.
        private void KeepOnScreen()
        {
            Viewport vp = Game.GraphicsDevice.Viewport;
            float halfWidth = spriteTexture.Width / 2f;
            float halfHeight = spriteTexture.Height / 2f;

            if (Location.X - halfWidth < 0)
            {
                Location.X = halfWidth;
                Direction.X = Math.Abs(Direction.X);
            }
            else if (Location.X + halfWidth > vp.Width)
            {
                Location.X = vp.Width - halfWidth;
                Direction.X = -Math.Abs(Direction.X);
            }

            if (Location.Y - halfHeight < 0)
            {
                Location.Y = halfHeight;
                Direction.Y = Math.Abs(Direction.Y);
            }
            else if (Location.Y + halfHeight > vp.Height)
            {
                Location.Y = vp.Height - halfHeight;
                Direction.Y = -Math.Abs(Direction.Y);
            }
        }

        // Called by GhostManager when this ghost runs into another ghost.
        // The ghost decides for itself what to do: turn to head directly away from the other ghost.
        public virtual void CollideWith(Ghost other)
        {
            if (state == GhostState.Dead)
                return;

            Vector2 away = Location - other.Location;
            if (away == Vector2.Zero)
            {
                away = GetRandomDirection(); // exactly on top of each other, just pick any direction
            }

            Direction = Vector2.Normalize(away);
            state = GhostState.Bumped;
            bumpedTimer = BumpedTime;
        }


        // Kill the ghost. GhostManager will remove it on its next Update.
        public virtual void Kill()
        {
            state = GhostState.Dead;
        }

        // Random unit length direction
        public static Vector2 GetRandomDirection()
        {
            double angle = random.NextDouble() * MathHelper.TwoPi;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
