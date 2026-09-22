using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotManager
{
    class MousePaint : ShotManager
    {
        InputHandler input;
        

        float shrinkTime, shrinkTimer;

        

        public MousePaint(Game game) : base(game)
        {

            input = (InputHandler)this.Game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                input = new InputHandler(this.Game);
                input.Initialize();
                this.Game.Components.Add(input);
            }

            this.shrinkTime = 200;
        }

        public override void Update(GameTime gameTime)
        {
            if (input.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                this.Shoot();
            }
            
            if (shrinkTimer > shrinkTime)
            {
                //reset timer
                shrinkTimer -= shrinkTime;
                //Shrink shots
                foreach (Shot s in this.Shots)
                {
                    s.Scale -= .02f;
                    if(s.Scale < .1) //too small time to remove
                    {
                        s.Enabled = false;
                    }
                }
            }
            shrinkTimer += gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameTime);
        }

        public override Shot Shoot()
        {
            s = base.Shoot();
            s.Location = this.input.MouseState.Position.ToVector2();
            return s;
        }
    }
}

