using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace Week5Demo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        InputHandler inputHandler;
        GameConsole gameConsole;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            inputHandler = new InputHandler(this);
            this.Components.Add(inputHandler);

            gameConsole = new GameConsole(this);
            this.Components.Add(gameConsole);

            FPS fps = new FPS(this, true, true);
            this.Components.Add(fps);
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            gameConsole.DebugTextOutput.Add("Q pressed", inputHandler.KeyboardState.IsHoldingKey(Keys.Q).ToString());
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (inputHandler.KeyboardState.HasReleasedKey(Keys.C))
            {
                gameConsole.GameConsoleWrite("hi hi hi hi hi");
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
