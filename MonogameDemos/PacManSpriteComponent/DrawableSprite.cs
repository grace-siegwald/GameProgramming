using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace PacManSpriteComponent

{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DrawableSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Vector2 Location, Direction;
        public float Speed, Rotate;
        public SpriteEffects SpriteEffects;

        protected Texture2D spriteTexture;
        protected ContentManager content;
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected float lastUpdateTime;
        
        public DrawableSprite(Game game)
            : base(game)
        {
            
            content = game.Content;
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));
            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
           
            Location = new Vector2(this.Game.GraphicsDevice.Viewport.Width / 2, this.Game.GraphicsDevice.Viewport.Height / 2);
            Direction = new Vector2(1, 0);
            Speed = 100.0f;
            Rotate = 0.0f;
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            //Elapsed time since last update
            lastUpdateTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //GamePad1
            SpriteEffects = SpriteEffects.None;       //Default Sprite Effects

            base.Update(gameTime);
        }

        

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(spriteTexture,
                new Rectangle((int)Location.X, (int)Location.Y,
                    spriteTexture.Width,
                    spriteTexture.Height),
                null,
                Color.White,
                MathHelper.ToRadians(Rotate),
                new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2),
                SpriteEffects,
                0);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}