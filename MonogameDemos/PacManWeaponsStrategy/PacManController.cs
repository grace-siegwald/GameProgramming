using MonoGameLibrary.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManWeaponsStrategy
{
    sealed class PacManController
    {
        InputHandler input; //game service to handle input
        public Vector2 Direction {get; private set;}
        public float Rotate { get; private set;}

        //Checks to see if there is anymovement
        public bool hasInputForMoverment
        {
            get
            {
                
                if (Direction.Length() == 0) return false;
                return true;
            }
        }

        public PacManController(Game game)
        {
            //get input handler from game services
            input = (InputHandler)game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                throw new Exception("PacMan controller depends on InputHandler service please add Input Handler as a service first");
            }
        }

        public void Update()
        {
            //Input for update from analog stick
            GamePadState gamePad1State = input.GamePads[0]; //HACK hard coded player index
            #region LeftStick
            Vector2 pacStickDir = Vector2.Zero;
            if (gamePad1State.ThumbSticks.Left.Length() > 0.0f)
            {
                pacStickDir = gamePad1State.ThumbSticks.Left;
                pacStickDir.Y *= -1;      //Invert Y Axis

                float RotationAngle = (float)Math.Atan2(
                    gamePad1State.ThumbSticks.Left.X,
                    gamePad1State.ThumbSticks.Left.Y);

                Rotate = (float)MathHelper.ToDegrees(RotationAngle - (float)(Math.PI / 2));
            }
            #endregion

            //Update for input from DPad
            #region DPad
            Vector2 PacManDPadDir = Vector2.Zero;
            if (gamePad1State.DPad.Left == ButtonState.Pressed)
            {
                //Orginal Position is Right so flip X
                PacManDPadDir += new Vector2(-1, 0);
            }
            if (gamePad1State.DPad.Right == ButtonState.Pressed)
            {
                //Original Position is Right
                PacManDPadDir += new Vector2(1, 0);
            }
            if (gamePad1State.DPad.Up == ButtonState.Pressed)
            {
                //Up
                PacManDPadDir += new Vector2(0, -1);
            }
            if (gamePad1State.DPad.Down == ButtonState.Pressed)
            {
                //Down
                PacManDPadDir += new Vector2(0, 1);
            }
            if (PacManDPadDir.Length() > 0)
            {
                //Angle in radians from vector
                float RotationAngleKey = (float)Math.Atan2(
                        PacManDPadDir.X,
                        PacManDPadDir.Y * -1);
                //Find angle in degrees
                Rotate = (float)MathHelper.ToDegrees(
                    RotationAngleKey - (float)(Math.PI / 2)); //rotated right already

                //Normalize NewDir to keep agled movement at same speed as horilontal/Vert
                PacManDPadDir = Vector2.Normalize(PacManDPadDir);
            }
            #endregion

            //Update for input from Keyboard
#if !XBOX360
            #region KeyBoard
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 PacManKeyDir = new Vector2(0, 0);

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                //Flip Horizontal
                PacManKeyDir += new Vector2(-1, 0);
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                //No new sprite Effects
                PacManKeyDir += new Vector2(1, 0);
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                PacManKeyDir += new Vector2(0, -1);
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                PacManKeyDir += new Vector2(0, 1);
            }
            if (PacManKeyDir.Length() > 0)
            {
                float RotationAngleKey = (float)Math.Atan2(
                        PacManKeyDir.X,
                        PacManKeyDir.Y * -1);

                Rotate = (float)MathHelper.ToDegrees(
                    RotationAngleKey - (float)(Math.PI / 2));

                //Normalize NewDir to keep agled movement at same speed as horilontal/Vert
                PacManKeyDir = Vector2.Normalize(PacManKeyDir);
            }
            #endregion
#endif
            Direction= PacManKeyDir + PacManDPadDir + pacStickDir;
            if (Direction.Length() > 0)
            {
                Direction = Vector2.Normalize(Direction);
            }
        }
    }
}
