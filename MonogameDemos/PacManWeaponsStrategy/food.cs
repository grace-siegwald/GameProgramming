using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Sprite;

namespace PacManWeaponsStrategy
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class food : Sprite
    {
        public Color color;
        
        public food(Game game)
            : base(game)
        {
            
            this.color = Color.White;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteTexture = this.Game.Content.Load<Texture2D>("food");
            Speed = 0;
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
         //Don't nee this the spriteTransform handles the rectangle now
            //this.locationRect = new Rectangle(
            //        (int)Location.X - (int)this.Orgin.X,
            //        (int)Location.Y - (int)this.Orgin.Y,
            //        (int)(spriteTexture.Width * this.Scale),
            //        (int)(spriteTexture.Height * this.Scale));

            
              sb.Draw(spriteTexture,
                new Rectangle(
                    (int)Location.X,
                    (int)Location.Y,
                    (int)(spriteTexture.Width * this.Scale),
                    (int)(spriteTexture.Height * this.Scale)),
                null,
                color,
                MathHelper.ToRadians(Rotate),
                this.Origin,
                SpriteEffects,
                0);
            
            
            DrawMarkers(sb);
        }
    }
}