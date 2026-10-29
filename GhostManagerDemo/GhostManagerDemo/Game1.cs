using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GhostManagerDemo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        GhostManager ghostManager;

        KeyboardState previousKeyboard;
        MouseState previousMouse;
        int lastGhostCount = -1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // GhostManager makes the 4 starting ghosts itself in Initialize
            ghostManager = new GhostManager(this);
            this.Components.Add(ghostManager);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                Exit();

            // Add and remove ghosts at any time
            if (WasPressed(keyboard, Keys.A) || WasPressed(keyboard, Keys.Up))
                ghostManager.AddGhost();

            if (WasPressed(keyboard, Keys.R) || WasPressed(keyboard, Keys.Down))
                ghostManager.RemoveGhost();

            if (WasPressed(keyboard, Keys.C))
                ghostManager.RemoveAllGhosts();

            if (WasPressed(keyboard, Keys.M))
                ghostManager.ShowMarkers = !ghostManager.ShowMarkers;

            // Click a ghost to kill it. The ghost changes its own state to Dead and the manager removes it.
            if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                for (int i = ghostManager.Ghosts.Count - 1; i >= 0; i--) // top ghost is drawn last
                {
                    Ghost ghost = ghostManager.Ghosts[i];
                    if (ghost.LocationRect.Contains(mouse.Position))
                    {
                        ghost.Kill();
                        break;
                    }
                }
            }

            previousKeyboard = keyboard;
            previousMouse = mouse;

            base.Update(gameTime); // updates GhostManager

            if (ghostManager.Count != lastGhostCount)
            {
                lastGhostCount = ghostManager.Count;
                Window.Title = $"Ghost Manager - {lastGhostCount} ghosts   (A add, R remove, C clear, click kill, M markers)";
            }
        }

        private bool WasPressed(KeyboardState keyboard, Keys key)
        {
            return keyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime); // draws GhostManager
        }
    }
}
