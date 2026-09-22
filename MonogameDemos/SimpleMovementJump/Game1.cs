using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SimpleMovementJump
{
    /// <summary>
    /// Simple Movement For Jumping
    /// Uses a simple class called KeyboardHandler for input
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PacMan pac;

        SpriteFont font;
        string OutputData; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            pac = new PacMan();
            
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

            pac.Texture = Content.Load<Texture2D>("pacManSingle");
            //Place pacman at the center of the screen
            pac.Loc = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            //In the sample Direction also has magnitude
            pac.Dir = new Vector2(50, 0);

            pac.SpeedMax = 200;

            
            

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
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Elapsed time since last update
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            pac.UpdatePacManMove(time);

            pac.UpdateKeepPacmanOnScreen(GraphicsDevice);
            pac.UpdateInputFromKeyboard();

            OutputData = string.Format("PacDir:{0}\nPacLoc:{1}\nGravityDir:{2}\nGravityAccel:{3}\nTime:{4}\njumpHeight:{5}", pac.Dir.ToString(),
                pac.Loc.ToString(), pac.GravityDir.ToString(), pac.GravityAccel.ToString(), time, pac.jumpHeight);

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
            spriteBatch.Draw(pac.Texture, pac.Loc, Color.Green);

            /*
             * Draw parameters on screen
             */

            spriteBatch.DrawString(font, OutputData , new Vector2(10, 10), Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
