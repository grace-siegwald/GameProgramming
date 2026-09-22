using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace PacManSpriteComponent
{
    class PacMan : DrawableSprite
    {
        KeyboardHandler keyboard;  //depands on keyboarHandler class

        public PacMan(Game game)
            : base(game)
        {
            keyboard = new KeyboardHandler();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteTexture = this.Game.Content.Load<Texture2D>("pacManSingle");
        }

        public override void Update(GameTime gameTime)
        {
            
            UpdatePacMan(lastUpdateTime);
            base.Update(gameTime);
        }
        
        private void UpdatePacMan(float lastUpdateTime)
        {
            GamePadState gamePad1State = GamePad.GetState(PlayerIndex.One);

            //Input for update from analog stick
            #region LeftStick
            UpdateGamePasSticks(gamePad1State, lastUpdateTime);
            #endregion

            //Update for input from DPad
            #region DPad
            UpdateGamePad(gamePad1State, lastUpdateTime);
            #endregion

            //Update for input from Keyboard
#if !XBOX360
            #region KeyBoard
            UpdateKeyboard(lastUpdateTime);
            #endregion
#endif
        }

        private GamePadState UpdateGamePasSticks(GamePadState gamePad1State, float lastUpdateTime)
        {
            if (gamePad1State.ThumbSticks.Left.Length() > 0.0f)
            {

                Direction = gamePad1State.ThumbSticks.Left;
                Direction.Y *= -1;      //Invert Y Axis

                float RotationAngle = (float)Math.Atan2(
                    gamePad1State.ThumbSticks.Left.X,
                    gamePad1State.ThumbSticks.Left.Y);

                Rotate = (float)MathHelper.ToDegrees(RotationAngle - (float)(Math.PI / 2));


                //Time corrected move. MOves PacMan By PacManDiv every Second
                Location += ((Direction * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir

                //Keep PacMan On Screen
                if (Location.X > this.Game.GraphicsDevice.Viewport.Width - (spriteTexture.Width / 2))
                    Location.X = this.Game.GraphicsDevice.Viewport.Width - (spriteTexture.Width / 2);

                if (Location.X < (spriteTexture.Width / 2))
                    Location.X = (spriteTexture.Width / 2);
            }

            return gamePad1State;
        }

        private void UpdateGamePad(GamePadState gamePad1State, float lastUpdateTime)
        {
            Vector2 PacManDDir = Vector2.Zero;
            if (gamePad1State.DPad.Left == ButtonState.Pressed)
            {
                //Orginal Position is Right so flip X
                PacManDDir += new Vector2(-1, 0);
            }
            if (gamePad1State.DPad.Right == ButtonState.Pressed)
            {
                //Original Position is Right
                PacManDDir += new Vector2(1, 0);
            }
            if (gamePad1State.DPad.Up == ButtonState.Pressed)
            {
                //Up
                PacManDDir += new Vector2(0, -1);
            }
            if (gamePad1State.DPad.Down == ButtonState.Pressed)
            {
                //Down
                PacManDDir += new Vector2(0, 1);
            }
            if (PacManDDir.Length() > 0)
            {

                //Angle in radians from vector
                float RotationAngleKey = (float)Math.Atan2(
                        PacManDDir.X,
                        PacManDDir.Y * -1);
                //Find angle in degrees
                Rotate = (float)MathHelper.ToDegrees(
                    RotationAngleKey - (float)(Math.PI / 2)); //rotated right already

                //move the pacman
                Location += ((PacManDDir * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir
            }
        }

        private void UpdateKeyboard(float lastUpdateTime)
        {
            Vector2 PacManKeyDir = new Vector2(0, 0);
            keyboard.Update();

            if (keyboard.IsKeyDown(Keys.Left))
            {
                //Flip Horizontal

                PacManKeyDir += new Vector2(-1, 0);
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                //No new sprite Effects

                PacManKeyDir += new Vector2(1, 0);
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {

                PacManKeyDir += new Vector2(0, -1);
            }
            if (keyboard.IsKeyDown(Keys.Down))
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


                Location += ((PacManKeyDir * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir
            }
        }
    }
}
