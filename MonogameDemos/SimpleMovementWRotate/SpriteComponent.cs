using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMovementWRotate
{

    // Demo Notes
    public class SpriteComponent : GameComponent
    {
        int count;
        
        public SpriteComponent(Game game) : base(game)
        { 
        }
        public override void Initialize()
        {
            count = 0;
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            count++;
            base.Update(gameTime);
        }
    }
}
