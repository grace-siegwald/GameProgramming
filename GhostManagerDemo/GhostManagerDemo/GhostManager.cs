using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GhostManagerDemo
{
    // Owns a List of Ghosts
    public class GhostManager : DrawableGameComponent
    {
        public const int StartingGhostCount = 4;

        static readonly string[] ghostTextureNames = { "RedGhost", "PurpleGhost", "TealGhost" };

        const int SpawnPadding = 10; // extra space kept between ghosts when spawning
        const int MaxSpawnAttempts = 100; // give up after this many random tries (screen is full)

        readonly List<Ghost> ghosts = new List<Ghost>();

        // Changing a List while looping over it throws an exception,
        // so adds and removes that happen during Update (or before Initialize) wait here
        readonly List<Ghost> ghostsToAdd = new List<Ghost>();
        readonly List<Ghost> ghostsToRemove = new List<Ghost>();

        readonly Random random = new Random();
        SpriteBatch spriteBatch;
        bool isInitialized;
        bool isUpdating;

        // Read only view of the ghosts (use AddGhost/RemoveGhost to change it)
        public IReadOnlyList<Ghost> Ghosts { get { return ghosts; } }

        public int Count { get { return ghosts.Count; } }

        bool showMarkers;
        // Turns the Sprite collision markers on/off for every ghost
        public bool ShowMarkers
        {
            get { return showMarkers; }
            set
            {
                showMarkers = value;
                foreach (Ghost ghost in ghosts)
                {
                    ghost.ShowMarkers = value;
                }
            }
        }

        public GhostManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize(); // calls LoadContent
            isInitialized = true;

            // Any ghosts added before Initialize (from Game1's constructor for example)
            ProcessPendingChanges();

            // Starting ghosts in random spots
            for (int i = 0; i < StartingGhostCount; i++)
            {
                AddGhost();
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        #region Add and Remove

        /// Adds a new ghost with a random color at a random open spot
        public Ghost AddGhost()
        {
            string textureName = ghostTextureNames[random.Next(ghostTextureNames.Length)];
            return AddGhost(new Ghost(Game, textureName));
        }

        /// <summary>
        /// Adds a ghost at a random open spot
        /// </summary>
        /// <returns>the ghost, or null if there was no room for it</returns>
        public Ghost AddGhost(Ghost ghost)
        {
            if (ghost == null || ghosts.Contains(ghost) || ghostsToAdd.Contains(ghost))
                return ghost;

            //Can't load textures before the game is initialized and can't change the list mid Update
            if (!isInitialized || isUpdating)
            {
                ghostsToAdd.Add(ghost);
                return ghost;
            }

            return SpawnGhost(ghost) ? ghost : null;
        }

        /// <summary>
        /// Removes a specific ghost
        /// </summary>
        /// <returns>true if the ghost was in the manager</returns>
        public bool RemoveGhost(Ghost ghost)
        {
            if (ghost == null)
                return false;

            if (ghostsToAdd.Remove(ghost))  //never got spawned, just cancel it
                return true;

            if (!ghosts.Contains(ghost))
                return false;

            if (isUpdating)
            {
                if (!ghostsToRemove.Contains(ghost))
                    ghostsToRemove.Add(ghost);
            }
            else
            {
                ghosts.Remove(ghost);
            }
            return true;
        }

        /// <summary>
        /// Removes the most recently added ghost
        /// </summary>
        public bool RemoveGhost()
        {
            if (ghosts.Count == 0)
                return false;
            return RemoveGhost(ghosts[ghosts.Count - 1]);
        }

        public void RemoveAllGhosts()
        {
            ghostsToAdd.Clear();
            if (isUpdating)
            {
                ghostsToRemove.AddRange(ghosts);
            }
            else
            {
                ghosts.Clear();
            }
        }

        /// <summary>
        /// Loads the ghost, finds a spot where it doesn't overlap any other ghost, and adds it to the list
        /// </summary>
        private bool SpawnGhost(Ghost ghost)
        {
            ghost.ShowMarkers = showMarkers;
            ghost.Initialize();     //loads the ghost's textures (DrawableGameComponent only runs LoadContent once)

            Vector2 spot;
            if (!TryFindOpenSpot(ghost, out spot))
            {
                System.Diagnostics.Debug.WriteLine("GhostManager: no room to spawn another ghost");
                return false;
            }

            ghost.Location = spot;
            ghost.SetTranformAndRect();     //update LocationRect now so the next ghost spawned checks against this spot
            ghosts.Add(ghost);
            return true;
        }

        /// <summary>
        /// Picks random spots on screen until it finds one that doesn't overlap any existing ghost
        /// </summary>
        private bool TryFindOpenSpot(Ghost ghost, out Vector2 spot)
        {
            Viewport vp = GraphicsDevice.Viewport;
            int width = ghost.spriteTexture.Width;
            int height = ghost.spriteTexture.Height;

            for (int attempt = 0; attempt < MaxSpawnAttempts; attempt++)
            {
                //Location is the center of the ghost, keep the whole ghost on screen
                int x = random.Next(width / 2, Math.Max(width / 2 + 1, vp.Width - width / 2));
                int y = random.Next(height / 2, Math.Max(height / 2 + 1, vp.Height - height / 2));

                //Same rectangle Sprite.SetTranformAndRect would make for this Location
                Rectangle candidate = new Rectangle(x - (int)ghost.Origin.X, y - (int)ghost.Origin.Y, width, height);
                candidate.Inflate(SpawnPadding, SpawnPadding);

                if (!OverlapsAnyGhost(candidate))
                {
                    spot = new Vector2(x, y);
                    return true;
                }
            }

            spot = Vector2.Zero;
            return false;
        }

        private bool OverlapsAnyGhost(Rectangle rect)
        {
            foreach (Ghost other in ghosts)
            {
                if (rect.Intersects(other.LocationRect))
                    return true;
            }
            return false;
        }

        // Applies adds and removes that had to wait
        private void ProcessPendingChanges()
        {
            //Remove first so removed ghosts free up space for new ones
            foreach (Ghost ghost in ghostsToRemove)
            {
                ghosts.Remove(ghost);
            }
            ghostsToRemove.Clear();

            if (ghostsToAdd.Count > 0)
            {
                Ghost[] adding = ghostsToAdd.ToArray();
                ghostsToAdd.Clear();
                foreach (Ghost ghost in adding)
                {
                    SpawnGhost(ghost);
                }
            }
        }

        #endregion  // Claude helped me write this region 

        public override void Update(GameTime gameTime)
        {
            isUpdating = true;
            try
            {
                // Move every ghost
                foreach (Ghost ghost in ghosts)
                {
                    ghost.Update(gameTime);
                }

                CheckGhostCollisions();

                // Ghosts manage their own state, the manager just cleans up the dead ones
                foreach (Ghost ghost in ghosts)
                {
                    if (ghost.IsDead && !ghostsToRemove.Contains(ghost))
                        ghostsToRemove.Add(ghost);
                }
            }
            finally
            {
                isUpdating = false;
            }

            ProcessPendingChanges();

            base.Update(gameTime);
        }

        // Checks every pair of ghosts once (Claude also helped me write this)
        private void CheckGhostCollisions()
        {
            for (int i = 0; i < ghosts.Count; i++)
            {
                Ghost a = ghosts[i];
                if (a.IsDead) continue;

                for (int j = i + 1; j < ghosts.Count; j++)
                {
                    Ghost b = ghosts[j];
                    if (b.IsDead) continue;

                    // just rectangle collision
                    if (a.LocationRect.Intersects(b.LocationRect))
                    {
                        // Each ghost decides how to change its own direction
                        a.CollideWith(b);
                        b.CollideWith(a);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // One SpriteBatch for all the ghosts
            spriteBatch.Begin();
            foreach (Ghost ghost in ghosts)
            {
                if (ghost.Visible)
                {
                    ghost.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
