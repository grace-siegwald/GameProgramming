using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpritebatchOptions
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D PacMan;
        Vector2 PacManLoc;
        Vector2 PacManLocMoveAddition;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            PacMan = Content.Load<Texture2D>("pacManSingle");
            PacManLoc = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            PacManLocMoveAddition = Vector2.Zero;

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

            PacManLocMoveAddition = PacManLocMoveAddition + new Vector2(1.0f, 1.0f);
            if (PacManLocMoveAddition.X > 175)
            {
                PacManLocMoveAddition = new Vector2(-50.0f, -50.0f);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //We are going to draw a bunch of pacman objects


            //Simple Batch no options AlphaBlend is default
            spriteBatch.Begin();
            Draw4PacMans(spriteBatch, new Vector2(100, 100));
            spriteBatch.End();

            //No blend Much faster than AlphaBlend
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //spriteBatch.Begin(SpriteBlendMode.None);
            Draw4PacMans(spriteBatch, new Vector2(200, 100));
            Draw4PacMans(spriteBatch, new Vector2(300, 100) + PacManLocMoveAddition); //OverLap with additive
            spriteBatch.End();

            //Additive blends with the No Blend Above
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Draw4PacMans(spriteBatch, new Vector2(300, 100));
            Draw4PacMans(spriteBatch, new Vector2(310, 100));
            Draw4PacMans(spriteBatch, new Vector2(320, 100));
            spriteBatch.End();

            //Pacman with alpha and color
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //Draw 4 Pacmans
            int offset = 25;
            PacManLoc = new Vector2(400, 100);
            int i = 0;

            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset),
                Color.Violet);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset),
                Color.SkyBlue);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset),
                Color.Pink);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset),
                Color.OldLace);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //Draw 4 Pacmans
            offset = 25;
            PacManLoc = new Vector2(500, 100);
            i = 0;

            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset) + PacManLocMoveAddition,
                Color.Violet);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset) + PacManLocMoveAddition,
                Color.SkyBlue);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset) + PacManLocMoveAddition,
                Color.Pink);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset) + PacManLocMoveAddition,
                Color.SteelBlue);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            //Draw 4 Pacmans
            offset = 25;
            PacManLoc = new Vector2(500, 100);
            i = 0;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset),
                Color.Violet);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset),
                Color.Purple);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset),
                Color.SlateGray);
            i++;
            spriteBatch.Draw(PacMan,
                PacManLoc + new Vector2(i * offset, i * offset),
                Color.Maroon);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque);
            Draw4PacMansFull(spriteBatch, new Vector2(100, 300));
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
            Draw4PacMansFull(spriteBatch, new Vector2(200, 300));
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque);
            Draw4PacMansFull(spriteBatch, new Vector2(300, 300));
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            Draw4PacMansFull(spriteBatch, new Vector2(400, 300));
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            Draw4PacMansFull(spriteBatch, new Vector2(500, 300));
            spriteBatch.End();


            //Scale
            spriteBatch.Begin();
            PacManLoc = new Vector2(100, 450);


            Rectangle pacRect = new Rectangle((int)PacManLoc.X,
                (int)PacManLoc.Y, PacMan.Width * 4, PacMan.Height * 2);


            


            spriteBatch.Draw(PacMan,
                pacRect, null, Color.Blue, 0.0f, Vector2.Zero,
                SpriteEffects.None, (float)i / 4);

            //clip
            PacManLoc.X += 100;
            pacRect = new Rectangle((int)PacManLoc.X,
                (int)PacManLoc.Y, PacMan.Width, PacMan.Height);

            spriteBatch.Draw(PacMan,
                pacRect,
                new Rectangle(0, 0, 40, 40),
                Color.PowderBlue, 0.0f, Vector2.Zero,
                SpriteEffects.FlipHorizontally, (float)i / 4);
            spriteBatch.End();

            //Sprite Effects Flip
            spriteBatch.Begin();
            PacManLoc.X += 100;

            pacRect = new Rectangle((int)PacManLoc.X,
                (int)PacManLoc.Y, (int)(PacMan.Width * 1.5), (int)(PacMan.Height * 1.5));

            spriteBatch.Draw(PacMan,
                pacRect, null, Color.Red, 0.0f, Vector2.Zero,
                SpriteEffects.FlipVertically, (float)i / 4);

            //Rotate Corner Orgin
            PacManLoc.X += 100;
            pacRect = new Rectangle((int)PacManLoc.X,
                (int)PacManLoc.Y, PacMan.Width, PacMan.Height);

            spriteBatch.Draw(PacMan,
                pacRect, null, Color.SaddleBrown, MathHelper.ToRadians(45.0f), Vector2.Zero,
                SpriteEffects.None, (float)i / 4);

            //Rotate Center Orgin
            PacManLoc.X += 100;

            pacRect = new Rectangle((int)PacManLoc.X,
                (int)PacManLoc.Y, PacMan.Width, PacMan.Height);

            spriteBatch.Draw(PacMan,
                pacRect, null, Color.SeaGreen, MathHelper.ToRadians(45.0f),
                new Vector2(PacMan.Width / 2, PacMan.Height / 2),
                SpriteEffects.None, (float)i / 4);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void Draw4PacMans(SpriteBatch sb, Vector2 location)
        {
            //Draw 4 Pacmans
            int offset = 25;
            PacManLoc = location;
            for (int i = 0; i < 4; i++)
            {
               
                spriteBatch.Draw(PacMan,
                    PacManLoc + new Vector2(i * offset, i * offset),
                    Color.White);
            }
        }

        protected void Draw4PacMansFull(SpriteBatch sb, Vector2 location)
        {
            //Draw 4 Pacmans
            int offset = 25;
            PacManLoc = location;
            for (int i = 0; i < 4; i++)
            {
                Rectangle pacRect = new Rectangle((int)PacManLoc.X + i * offset,
                    (int)PacManLoc.Y + i * offset, PacMan.Width, PacMan.Height);

                spriteBatch.Draw(PacMan,
                    pacRect, null, Color.White, 0.0f, Vector2.Zero,
                    SpriteEffects.None, (float)i / 4);
            }
        }
    }
}
