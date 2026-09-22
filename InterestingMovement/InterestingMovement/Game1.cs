using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InterestingMovement
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D Background;
        Player Player = new Player("kirbyRidingStar", Color.White);
        SpriteFont font;

        // Framerate Stuff
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
            
            _graphics.PreferredBackBufferHeight = 258;
            _graphics.PreferredBackBufferWidth = 260;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        Vector2 middle;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            middle = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            Background = Content.Load<Texture2D>("kirbyBackgroundCutout");
            
            Player.LoadContent(this, middle, 200, new Vector2(1, 0));

            font = Content.Load<SpriteFont>("Arial");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(Background, new Vector2(0,0), Color.White);
            Player.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
