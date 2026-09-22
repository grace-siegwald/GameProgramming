using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision
{

    public enum GhostState
    {
        Spawning, Stopped, Roving, Chasing, Evading, Dying, Dead
    }

    class Ghost : DrawableSprite
    {
        
        //Static
        protected static short ghostCount;

        //State
        protected GhostState ghoststate;

        //Protected
        Texture2D ghostTexture, hitTexture;     //two textures one for regular ghost and one used when ghost is hit by pacman
        GameConsole console;


        public GhostState Ghoststate
        {
            get { return ghoststate; }
            protected set { this.ghoststate = value; }
        }

        //Ghost depends on pacman
        PacMan pac;

        public Ghost(Game game, PacMan pac)
            : base(game)
        {
            ghostCount++;
            this.pac = pac;

            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            //can't be sure I got a console
            if(console==null)
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);
            }
        }

        protected override void LoadContent()
        {
            this.ghostTexture = this.Game.Content.Load<Texture2D>("TealGhost");
            this.hitTexture = this.Game.Content.Load<Texture2D>("GhostHit");
            this.spriteTexture = ghostTexture;

            this.Location = new Vector2(100 + (100 * ghostCount), 100 ); //offset based on ghostcount
            this.Speed = 100;
            base.LoadContent();
            this.Origin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);
            this.Scale = 1;
            this.Ghoststate = GhostState.Stopped;
        }

        public override void Update(GameTime gameTime)
        {


            UpdateGhostTexture();

            UpdateGhostLogToConsole();
            UpdateGhostCheckCollision();

            base.Update(gameTime);
        }

        private void UpdateGhostCheckCollision()
        {
            //Check for Collision very simple test for rectangle collision
            if (this.Intersects(pac))
            {
                //hit
                console.GameConsoleWrite("pac intersects teal ghost");

                //Maybe try per pixel collision here 
                //It's a good idea to do rectagle collision first no need to look at pixels if rectangle don't intersect
                if (this.PerPixelCollision(pac))
                {
                    console.GameConsoleWrite("pac pixel collision with teal ghost");
                    this.Hit();
                }
                else
                {
                    this.Chase();
                }
            }
            else
            {
                this.Chase();
            }
        }

        private void UpdateGhostLogToConsole()
        {
#if DEBUG
            console.DebugTextOutput["GhostState"] = this.Ghoststate.ToString();
            console.DebugTextOutput["GhostLoc"] = this.Location.ToString();
            console.DebugTextOutput["GhostRect"] = this.LocationRect.ToString();
#endif
        }

        /// <summary>
        /// Changes ghost texture based on state
        /// </summary>
        private void UpdateGhostTexture()
        {
            switch(this.Ghoststate)
            {
                case GhostState.Chasing:
                case GhostState.Evading:
                case GhostState.Roving:
                case GhostState.Spawning:
                case GhostState.Stopped:
                    this.spriteTexture = ghostTexture;
                    break;
                case GhostState.Dead:
                case GhostState.Dying:
                    this.spriteTexture = hitTexture;
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public virtual void Hit()
        {
            this.ghoststate = GhostState.Dead;
        }

        public virtual void Chase()
        {
            this.ghoststate = GhostState.Chasing;
        }
    }
}
