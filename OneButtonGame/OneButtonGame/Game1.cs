using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;

namespace OneButtonGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public float WindowScaler = 2.0f;
        public Texture2D Background;

        InputHandler input;
        GameManager waveRideManager;
        HUD hud;
        WaterFloor water;
        Wave wave;
        Kirby kirby;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = (int)(256 * WindowScaler);   // same size as the background sprite
            _graphics.PreferredBackBufferHeight = (int)(258 * WindowScaler);
            Window.Title = "Kirby Wave Ride";

            input = new InputHandler(this);
            Components.Add(input);

            water = new WaterFloor(this);
            Components.Add(water);

            kirby = new Kirby(this, water);
            Components.Add(kirby);

            wave = new Wave(this, water);
            Components.Add(wave);

            waveRideManager = new GameManager(this, kirby, wave); // last thing we add because we need kirby and wave
            Components.Add(waveRideManager);

            hud = new HUD(this, waveRideManager);
            Components.Add(hud);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Background = Content.Load<Texture2D>("background");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(Background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, WindowScaler, SpriteEffects.None, 0f); // using one of the many draw overloads to scale the background to fit the window
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
