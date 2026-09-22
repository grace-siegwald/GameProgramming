using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleMovementJump
{
    public class PacMan : Sprite
    {
        bool isOnGround;                //Hack to stop falling at a certian point 

        KeyboardHandler inputKeyboard;  //Instance of class that handles keyboard input

        public int jumpHeight = -200;          //Jump impulse

        public float GravityAccel;             //Acceloration from gravity
        public float Friction;                 //Friction to slow down when no input is set
        public float Accel = 10;               //Acceloration
        public Vector2 GravityDir;             //Direction for gravity

        public PacMan()
        {
            inputKeyboard = new KeyboardHandler();
            isOnGround = false; //Techical Debt should maybe be in the the LoadContent
            GravityDir = new Vector2(0, 1);     //Gravity direction starts as down
            GravityAccel = 200.0f;
            Friction = 10.0f;
        }

        public void UpdatePacManMove(float time)
        {
            //Time corrected move. MOves PacMan By PacManDiv every Second
            this.Loc = this.Loc + ((this.Dir * (time / 1000)));      //Simple Move PacMan by PacManDir

            //Gravity also affects pacman
            this.Dir = this.Dir + (GravityDir * GravityAccel) * (time / 1000);
        }

        /// <summary>
        /// Keeps pac man on screen
        /// </summary>
        public void UpdateKeepPacmanOnScreen(GraphicsDevice graphics)
        {
            //Keep PacMan On Screen
            if (
                //X right
                (this.Loc.X >
                    graphics.Viewport.Width - this.Texture.Width)
                ||
                //X left
                (this.Loc.X < 0)
                )
            {
                //Negate X
                this.Dir = this.Dir * new Vector2(-1, 1);
            }

            //Y stop at 400
            //Hack Floor location is hard coded
            //TODO viloates single resposibilty principle should be moved to it's own method
            if (this.Loc.Y > 400) //HACK
            {
                this.Loc.Y = 400;
                this.Dir.Y = 0;
                isOnGround = true;
            }
        }

        public void UpdateInputFromKeyboard()
        {
            inputKeyboard.Update();
            //Jump
            if (inputKeyboard.WasKeyPressed(Keys.Up))
            {
                this.Dir = this.Dir + new Vector2(0, jumpHeight);
                isOnGround = false; //remove onGround bool so gravity will kick in again
            }

            if (isOnGround)
            {
                //Allows left and right movement on ground 
                //This way you cannot change direction in the air this is a design descision
                if ((!(inputKeyboard.IsHoldingKey(Keys.Left))) &&
                    (!(inputKeyboard.IsHoldingKey(Keys.Right))))
                {
                    if (this.Dir.X > 0) //If the pacman has a positive direction in X(moving right)
                    {
                        this.Dir.X = Math.Max(0, this.Dir.X - Friction); //Reduce X by friction amount but don't go below 0 
                    }
                    else //Else pacman has a negative direction.X (moving left)
                    {

                        this.Dir.X = Math.Min(0, this.Dir.X + Friction); //Add friction amount until X is 0
                    }
                    //Zero X is stopped so if you're no holding a key friction will slow down the movement until pacman stops
                }

                //If keys left or Right key is down acceorate up to make speed
                if (inputKeyboard.IsHoldingKey(Keys.Left))
                {
                    this.Dir.X = Math.Max((this.SpeedMax * -1.0f), this.Dir.X - Accel);
                }
                if (inputKeyboard.IsHoldingKey(Keys.Right))
                {
                    this.Dir.X = Math.Min(this.SpeedMax, this.Dir.X + Accel);
                }
            }

#if DEBUG
            /* 
             * These settings are for testing gravity values and should be removed before final game release
             * The preprocessor directive #if DEBIG will remove this section when built in release mode
             */
            //A key changes gravity up
            if (inputKeyboard.WasKeyPressed(Keys.A))
            {
                GravityAccel = GravityAccel + 0.2f;
            }
            //Z key changes gravity down
            if (inputKeyboard.WasKeyPressed(Keys.Z))
            {
                GravityAccel = GravityAccel - 0.2f;
            }
            //S key changes jump up
            if (inputKeyboard.WasKeyPressed(Keys.S))
            {
                jumpHeight = jumpHeight + 10;
            }
            if (inputKeyboard.WasKeyPressed(Keys.X))
            {
                jumpHeight = jumpHeight - 10;
            }
            /*
             * We could also change gravity direction here
             */
#endif
        }

    }
}
