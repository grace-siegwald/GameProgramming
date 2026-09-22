using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotManager
{
    class MouseShot : ShotManager
    {
        InputHandler input;
        

        public MouseShot(Game game) : base(game)
        {
            
            input = (InputHandler)this.Game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                input = new InputHandler(this.Game);
                input.Initialize();
                this.Game.Components.Add(input);
            }
            //this.LimitShotRate = .01f;
        }

        public override void Update(GameTime gameTime)
        {
            if(input.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                this.Shoot();
            }
            base.Update(gameTime);
        }

        public override Shot Shoot()
        {
            s = base.Shoot();
            
            s.Direction = s.Location - this.input.MouseState.Position.ToVector2();
            s.Direction = s.Direction * -1;
            s.Direction.Normalize();
            s.Speed = 300;
            
            return s;
        }
    }
}
