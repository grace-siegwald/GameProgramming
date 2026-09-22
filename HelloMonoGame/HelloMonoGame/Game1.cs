using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HelloMonoGame
{
    public class Game1 : Game
    {
        // these are the two things that we're gonna draw with
        private GraphicsDeviceManager _graphics; // Take away around 1200 lines of c++ lol
        private SpriteBatch _spriteBatch;

        public SpriteFont Font;
        public Texture2D PacMan;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this); // "this" is the game1 class: the class is getting a copy of the graphics card basically
            Content.RootDirectory = "Content"; // Content is static, you can rename this folder if you want!
            IsMouseVisible = true;
        }

        
        // Initialize and LoadContent happen before we start the traditional game loop
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Font = Content.Load<SpriteFont>("Font"); // We don't put the extension on it, which means the name needs to be unique
            PacMan = Content.Load<Texture2D>("PacmanSingle"); // Once again, no file name extension, but this is the PacmanSingle.png!
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.DrawString(Font, "Hello MonoGame!!!", new Vector2(102, 102), Color.DarkGray);
            _spriteBatch.DrawString(Font, "Hello MonoGame!!!", new Vector2(101, 101), Color.LightGray);
            _spriteBatch.DrawString(Font, "Hello MonoGame!!!", new Vector2(100, 100), Color.AntiqueWhite);

            _spriteBatch.Draw(PacMan, new Vector2(200, 200), Color.White);
            _spriteBatch.Draw(PacMan, new Vector2(250, 200), Color.Green);
            _spriteBatch.Draw(PacMan, new Vector2(300, 200), Color.Blue);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
