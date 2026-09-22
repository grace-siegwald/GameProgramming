using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SimpleMovementWGravity
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PacMan PacMan1 = new PacMan("pacmanSingle", Color.White);
        PacMan PacMan2 = new PacMan("pacmanSingle", Color.Green);
        PacMan PacMan3 = new PacMan("pacmanSingle", Color.Red);


        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Middle of the screen:
            Vector2 middle = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            PacMan1.LoadContent(Content, GraphicsDevice, middle, 200, new Vector2(1, 0));
            PacMan2.LoadContent(Content, GraphicsDevice, middle / 1.5f, 200, new Vector2(1, 0));
            PacMan3.LoadContent(Content, GraphicsDevice, middle / 1.25f, 200, new Vector2(1, 0));

            font = Content.Load<SpriteFont>("Arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        float time;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            PacMan1.Update(gameTime);
            PacMan2.Update(gameTime);
            PacMan3.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            PacMan1.Draw(spriteBatch);
            PacMan2.Draw(spriteBatch);
            PacMan3.Draw(spriteBatch);
            spriteBatch.DrawString(font,
                string.Format("Speed:{0}\nDir:{1}\nGravityDir:{2}\nGravtyAccel:{3}",
                PacMan1.Speed, PacMan1.Direction, PacMan1.GravityDirection, PacMan1.GravityAcceleration),
                new Vector2(10, 10),
                Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
