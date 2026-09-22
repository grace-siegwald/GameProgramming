using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManSpriteComponent
{
    class Ghost : DrawableSprite
    {

        Texture2D redTexture;
        Texture2D hitTexture;
      
      
        
        
        public Ghost(Game game)
            : base(game)
        {
            
        }

        protected override void LoadContent()
        {
            
            spriteTexture = redTexture = this.Game.Content.Load<Texture2D>("RedGhost");
            hitTexture = this.Game.Content.Load<Texture2D>("GhostHit");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

           
            UpdateGhost(lastUpdateTime);
            base.Update(gameTime);
        }

        private void UpdateGhost(float lastUpdateTime)
        {
            //Do something with Ghost
        }
    }
}
