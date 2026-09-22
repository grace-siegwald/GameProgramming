using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace ShotManager
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        FPS fps;
        GameConsole console;
        MouseShot mouseShot;
        MousePaint mousePaint;

        PacManShoot pac;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            console = new GameConsole(this);
            this.Components.Add(console);

            fps = new FPS(this);
            this.Components.Add(fps);

            mouseShot = new MouseShot(this);
            this.Components.Add(mouseShot);

            mousePaint = new MousePaint(this) { ShotTexture = "RedGhost" } ;
            this.Components.Add(mousePaint);

            pac = new PacManShoot(this);
            this.Components.Add(pac);
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


            console.Log("mouseShotCount", mouseShot.Shots.Count.ToString());
            //console.Log("mousePaintCount", mousePaint.Shots.Count.ToString());
            console.Log("pacSMCount", pac.SM.Shots.Count.ToString());
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
            mouseShot.Draw(spriteBatch);
            //mousePaint.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}