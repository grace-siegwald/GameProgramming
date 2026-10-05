using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace InterestingMovement
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D Background;
        string playerTexture = "kirbyRidingStar";
        
        // For spawning lots of player objects
        List<Player> players = new List<Player>(); // empty list of players
        int numPlayers = 1;


        // Framerate Stuff
        SpriteFont font;
        float CumulativeFrameTime;
        int NumFrames;
        int FramesPerSecond;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Unlocks the framerate:
            _graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;

            // Sets the window size to 260x258 pixels:
            _graphics.PreferredBackBufferHeight = 258;
            _graphics.PreferredBackBufferWidth = 260;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Vector2 middle = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            Background = Content.Load<Texture2D>("kirbyBackgroundCutout");

            // Loads player texture and creates multiple player objects based on specfied number of players!
            for (int i = 0; i < numPlayers; i++)
            {
                Player player = new Player(this, playerTexture);
                {
                    player.Location = middle + new Vector2(i * 20 + 10, i * 10 + 20);
                    player.Speed = 150;
                }
                player.LoadContent();
                players.Add(player);
            }

            font = Content.Load<SpriteFont>("Arial");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var player in players)
            {
                player.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // We want to calculate FPS before drawing it
            calculateFPS(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(Background, new Vector2(0,0), Color.White);
            // Draw all the players in the list
            foreach (var player in players)
            {
                player.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            DrawFPS();
            DrawNumPlayers();

            base.Draw(gameTime);
        }
        private void DrawFPS()
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, $"FPS: {FramesPerSecond}", new Vector2(10, 10), Color.White);
            _spriteBatch.End();
        }
        private void DrawNumPlayers()
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, $"Players: {numPlayers}", new Vector2(10, 30), Color.White);
            _spriteBatch.End();
        }
        private void calculateFPS(GameTime gameTime)
        {
            CumulativeFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            NumFrames++;
            if (CumulativeFrameTime >= 1f)
            {
                FramesPerSecond = NumFrames;
                NumFrames = 0;
                CumulativeFrameTime -= 1f;
            }
        }
    }
}
