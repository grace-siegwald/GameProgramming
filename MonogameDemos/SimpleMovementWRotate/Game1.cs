using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SimpleMovementWRotate
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        PacMan pac;
        Ghost  purpleGhost;
        Ghost purpleGhostFaster;

        Sprite deadGhost;

        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            pac = new PacMan(this) { TextureName = "pacmanSingle" };
            purpleGhost = new Ghost(this);
            purpleGhostFaster = new Ghost(this, 200);

            deadGhost = new Sprite(this) { TextureName = "deadGhost" };

            //graphics.PreferredBackBufferHeight = 720;
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.ToggleFullScreen();  //changes to fullscreen

            //Change the framerate of the game to 30 frames per second
            //This is used to show how time changes animation speed or better yet that is shouldn't
            //TargetElapsedTime = TimeSpan.FromTicks(333333);
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

            pac.LoadContent();
            purpleGhost.LoadContent();
            deadGhost.LoadContent();
            purpleGhostFaster.LoadContent();
            purpleGhost.StartMovin();
            purpleGhostFaster.StartMovin();

            font = Content.Load<SpriteFont>("SpriteFont1");
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


            //Elapsed time since last update will be used to correct movement speed
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;


            pac.Update(gameTime);
            purpleGhost.Update(gameTime);
            deadGhost.Update(gameTime);
            purpleGhostFaster.Update(gameTime);

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


            pac.Draw(spriteBatch);
            purpleGhost.Draw(spriteBatch);
            deadGhost.Draw(spriteBatch);
            purpleGhostFaster.Draw(spriteBatch);
            
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
