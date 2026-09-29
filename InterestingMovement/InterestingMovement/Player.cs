using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace InterestingMovement
{
    public class Player : Sprite
    {
        public Player(string textureName, Color color) : base(textureName, color) // Pass the required textureName to the base Sprite constructor
        {

        }
        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateKeepOnScreen();
            UpdateKeyboardInput();
        }
        private void UpdateKeepOnScreen()
        {
            //Keep PacMan On Screen
            //Turns around and stays at edges
            //X right
            if (Location.X > Game.GraphicsDevice.Viewport.Width - Texture.Width)
            {
                //Negate X
                Direction.X *= -1;
                Location.X = Game.GraphicsDevice.Viewport.Width - Texture.Width;
            }

            //X left
            if (Location.X < 0)
            {
                //Negate X
                Direction.X *= -1;
                Location.X = 0;
            }

            //Y top
            if (Location.Y > Game.GraphicsDevice.Viewport.Height - Texture.Height)
            {
                //Negate Y
                Direction.Y *= -1;
                Location.Y = Game.GraphicsDevice.Viewport.Height - Texture.Height;
            }

            //Y bottom
            if (Location.Y < 0)
            {
                //Negate Y
                Direction.Y *= -1;
                Location.Y = 0;
            }
        }
        private void UpdateSpeed()
        {
            //Speed for next frame
            if (Keyboard.GetState().GetPressedKeys().Length > 0) //If there is any key press the length of the Array of keys returned by GetPressedKeys wil be greater that 0
            {
                Speed = 200;  //Key down pacman has speed
            }
            else
            {
                Speed = 0;    //No key down stop
            }
        }
        private void UpdateKeyboardInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                GravityDirection = new Vector2(0, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                GravityDirection = new Vector2(0, -1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                GravityDirection = new Vector2(1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                GravityDirection = new Vector2(-1, 0);
            }
        }

    }
}
