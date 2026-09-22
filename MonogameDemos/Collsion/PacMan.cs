using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;

namespace Collision
{
    /// <summary>
    /// State Enum for pac man we will use this later
    /// </summary>
    public enum PacState
    {
        Spawning, Stopped, Chomping, Dying, Dead, PoweredUp
    }

    class PacMan : DrawableSprite
    {
        int playerIndex; //player index for controller

        
        //Services PacMan depends on three services from game
        InputHandler input;
        GameConsole console;
        ScoreService score;

        //State
        protected PacState pacstate;
        public PacState Pacstate
        {
            get { return pacstate; }
            protected set { this.pacstate = value; }
        }
        
        public PacMan(Game game)
            : base(game)
        {
            // TODO: Construct any child components her
            playerIndex = 0;

            //PacMan Depends on some game sevices
        #region Dependancy Services
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));

            //Make sure input service exists
            if (input == null)  //if it doesn't add it this is lazy loading
            {
                throw new Exception("PacMan Depends on Input service please add input service before you add PacMan.");
            }

            console = (GameConsole)game.Services.GetService(typeof(IGameConsole));
            //Make sure input service exists
            if (console == null) //if it doesn't add it
            {
                throw new Exception("PacMan Depends on Console service please add Console service before you add PacMan.");
            }

            score = (ScoreService)game.Services.GetService(typeof(IScoreService));
            //Make sure input service exists
            if (console == null) //if it doesn't add it
            {
                throw new Exception("PacMan Depends on Score service please add Score service before you add PacMan.");
            }
        #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            switch ( this.Pacstate)
            {
                case PacState.Chomping:
                case PacState.Stopped:
                    UpdatePacMan(lastUpdateTime, gameTime);
                    break;
            }
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("pacManSingle");
            
            this.Location = new Vector2(300, 300);
            this.Speed = 100;
            base.LoadContent(); //set to zero
            this.Origin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);
            this.Scale = 1;
            this.Pacstate = PacState.Stopped;
        }
        
        private void UpdatePacMan(float lastUpdateTime, GameTime gameTime)
        {
            //GamePad
            
            //Input for update from analog stick
            #region LeftStick
            if (input.GamePads[playerIndex].ThumbSticks.Left.Length() > 0.0f)
            {

                Direction = input.GamePads[playerIndex].ThumbSticks.Left;
                Direction.Y *= -1;      //Invert Y Axis

                float RotationAngle = (float)Math.Atan2(
                    input.GamePads[playerIndex].ThumbSticks.Left.X,
                    input.GamePads[playerIndex].ThumbSticks.Left.Y);

                Rotate = (float)MathHelper.ToDegrees(RotationAngle - (float)(Math.PI / 2));


                //Time corrected move. MOves PacMan By PacManDiv every Second
                Location += ((Direction * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir

                //Keep PacMan On Screen
                if (Location.X > this.Game.GraphicsDevice.Viewport.Width - (spriteTexture.Width / 2))
                    Location.X = this.Game.GraphicsDevice.Viewport.Width - (spriteTexture.Width / 2);

                if (Location.X < (spriteTexture.Width / 2))
                    Location.X = (spriteTexture.Width / 2);
            }
            #endregion

            //Update for input from DPad
            #region DPad
            Vector2 PacManDDir = Vector2.Zero;
            if (input.GamePads[playerIndex].DPad.Left == ButtonState.Pressed)
            {
                //Orginal Position is Right so flip X
                PacManDDir += new Vector2(-1, 0);
            }
            if (input.GamePads[playerIndex].DPad.Right == ButtonState.Pressed)
            {
                //Original Position is Right
                PacManDDir += new Vector2(1, 0);
            }
            if (input.GamePads[playerIndex].DPad.Up == ButtonState.Pressed)
            {
                //Up
                PacManDDir += new Vector2(0, -1);
            }
            if (input.GamePads[playerIndex].DPad.Down == ButtonState.Pressed)
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
                
                Location += ((Vector2.Normalize(PacManDDir) * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir
            }
            #endregion

            //Update for input from Keyboard
#if !XBOX360
            #region KeyBoard
           
            Vector2 PacManKeyDir = new Vector2(0, 0);

            if (input.KeyboardState.IsKeyDown(Keys.Left))
            {
                //Flip Horizontal
                if (input.KeyboardState.WasKeyPressed(Keys.Left))
                {
                    //console.GameConsoleWrite("Pressed Left!" + gameTime.TotalGameTime.Seconds + "." + gameTime.TotalGameTime.Milliseconds);
                    
                }
                //score.CurrentScore++;
                PacManKeyDir += new Vector2(-1, 0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Right))
            {
                //No new sprite Effects
                if (input.KeyboardState.WasKeyPressed(Keys.Right))
                {
                    //console.GameConsoleWrite("Pressed Right!" + gameTime.TotalGameTime.Seconds + "." + gameTime.TotalGameTime.Milliseconds);
                }
                PacManKeyDir += new Vector2(1, 0);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Up))
            {

                PacManKeyDir += new Vector2(0, -1);
            }
            if (input.KeyboardState.IsKeyDown(Keys.Down))
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


                Location += ((Vector2.Normalize(PacManKeyDir) * (lastUpdateTime / 1000)) * Speed);      //Simple Move PacMan by PacManDir
            }
            #endregion
#endif

            console.DebugText = this.Location.ToString();
            console.DebugText += "what ever!!!";
            console.DebugText += "\nwhat ever!!!";

            console.DebugTextOutput["PacLocation"] = this.Location.ToString(); ;

        }

    }
}
