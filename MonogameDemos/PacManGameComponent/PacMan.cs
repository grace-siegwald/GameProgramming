using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacManGameComponent
{
    //PacMan States
    public enum PacState
    {
        Spawning, Stopped, Chomping, Dying, Dead, PoweredUp
    }

    public class PacMan : DrawableSprite
    {
        //Services
        protected GameConsole console;   //game console servive will try to get from the game class
        protected ScoreService score;   //score servive will try to get from the game class

        //State
        protected PacState pacstate;    //Private instance data member for sate
        public PacState Pacstate    //Public accessor for protected pacstate
        {
            get { return pacstate; }
            protected set { this.pacstate = value; }
        }

        public PacMan(Game game)
            : base(game)
        {
            //PacMan Depends on some game sevices
            #region Dependancy Services

            console = (GameConsole)game.Services.GetService(typeof(IGameConsole));
            //Make sure input service exists
            if (console == null)
            {
                console = new GameConsole(this.Game);
                console.Initialize();
                this.Game.Services.AddService(console);
            }

            score = (ScoreService)game.Services.GetService<IScoreService>();
            //Make sure score service exists
            if (score == null)
            {
                score = new ScoreService(this.Game);
                this.Game.Services.AddService(score);
            }
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Call differnt update methods depending on state
            switch (this.Pacstate)
            {
                case PacState.Chomping:
                case PacState.Stopped:
                    UpdatePacMan(lastUpdateTime, gameTime);
                    break;
            }
        }

        private void DoSomeAwesomAnimation()
        {
            throw new NotImplementedException();
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("pacManSingle");

            this.Location = new Vector2(300, 300);
            this.Speed = 100;
            base.LoadContent();
            this.Origin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);     //center orgin needs to be after base.LoadContnet() 
                                                                                                        //which defaults to top left
            this.Scale = 1;
            this.Pacstate = PacState.Stopped;   //Default state
        }

        protected virtual void UpdatePacMan(float lastUpdateTime, GameTime gameTime)
        {
            console.DebugText = this.Pacstate.ToString();   
        }
    }

}

