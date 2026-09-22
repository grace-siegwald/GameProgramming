using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMovementWRotate
{
    class PacMan : Sprite
    {
       public PacMan(Game game) : base(game)
       {

       }


        float RotationAngleKey;
        public override void Update(GameTime gameTime)
        {
            UpatePacmanKeepOnScreen();

            UpdatePacmanSpeed();
            //updated direction
            UpdateKeyboardInput();

            //Angle in radians from vector2
            RotationAngleKey = (float)System.Math.Atan2(
                    this.Direction.X,
                    this.Direction.Y * -1); //y axis is flipped already
            //Find angle in degrees
            this.Rotate = (float)MathHelper.ToDegrees(
                RotationAngleKey - (float)(Math.PI / 2)); //image is rotated facing right already hence the -1/2 PI


            base.Update(gameTime); //Move code from Parent

        }

        private void UpatePacmanKeepOnScreen()
        {
            //Turn PacMan Around if it hits the edge of the screen
            if ((this.Location.X > this.game.GraphicsDevice.Viewport.Width - this.Texture.Width)
                || (this.Location.X < 0))
            {
                this.Direction *= new Vector2(-1, 0);
            }
            if ((this.Location.Y > this.game.GraphicsDevice.Viewport.Height - this.Texture.Height)
                || (this.Location.Y < 0))
            {
                this.Direction *= new Vector2(0, -1);
            }
        }

        private void UpdatePacmanSpeed()
        {
            //Speed for next frame
            if (Keyboard.GetState().GetPressedKeys().Length > 0)
            {
                this.Speed = 200;
            }
            else
            {
                this.Speed = 0;
            }

        }
        private void UpdateKeyboardInput()
        {
            #region Keyboard Movement
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                this.Direction += new Vector2(0, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                this.Direction += new Vector2(0, -1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                this.Direction += new Vector2(1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                this.Direction += new Vector2(-1, 0);
            }

            //normalize vector
            if (this.Direction.Length() >= 1.0)
            {
                this.Direction.Normalize();
            }
            #endregion


        }

        
    }

    
}
