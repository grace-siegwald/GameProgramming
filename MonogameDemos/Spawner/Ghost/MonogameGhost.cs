using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghost
{
    class MonogameGhost : DrawableSprite
    {
        protected GameConsoleGhost ghost;
        public GameConsoleGhost Ghost
        {
               get { return this.ghost; }
               protected set { this.ghost = value; }
        }
        
        public Texture2D ghostHit;
        public Texture2D ghostTexture;
        public string strGhostTexture;

        Random r;

        public MonogameGhost(Game game)
            : base(game)
        {
            this.ghost = new GameConsoleGhost((GameConsole)game.Services.GetService<IGameConsole>());    
            strGhostTexture = "RedGhost";
            this.ghost.State = GhostState.Roving;
            
            r = new Random();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.ShowMarkers = false;
            this.Origin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);
        }

        protected override void LoadContent()
        {

            this.ghostTexture = this.Game.Content.Load<Texture2D>(strGhostTexture);
            this.ghostHit = this.Game.Content.Load<Texture2D>("GhostHit");
            this.spriteTexture = ghostTexture;
            this.Direction = new Vector2(0, 1);

            this.Speed = 100.0f;

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            

            switch (this.ghost.State)
            {
                case GhostState.Dead:

                    //TODO Dead moving and dead animation.
                    //Until then
                    this.ghost.State = GhostState.Roving;
                    //Pick random direction
                    this.Direction = this.GetRandomDirection(); ;                 //Assign random direction


                    break;
                case GhostState.Chasing:
                    //Change texture if Chasing
                    if (!(this.spriteTexture == this.ghostTexture))
                    {
                        this.ghost.Log(this.ToString() + " Chasing changed texture to spriteTexture");
                        this.spriteTexture = this.ghostTexture;
                    }
                    
                    break;
                case GhostState.Evading:
                    //Change texture if evading
                    if (this.spriteTexture != this.ghostHit)
                    {
                        this.ghost.Log(this.ToString() + " Evading changed texture to ghostHit");
                        this.spriteTexture = this.ghostHit;
                    }

                    
                    break;
                case GhostState.Roving:
                    //Change texture if Chasing
                    if (!(this.spriteTexture == this.ghostTexture))
                    {
                        this.ghost.Log(this.ToString() + " Roving changed texture to spriteTexture");
                        this.spriteTexture = this.ghostTexture;
                    }
                    

                    break;
            }

            Location += ((this.Direction * (lastUpdateTime / 1000)) * Speed);      //Simple Move

            //Borders Keep Ghost on the Screen
            if ((this.Location.Y + this.spriteTexture.Height / 2 > this.Game.GraphicsDevice.Viewport.Height)
                ||
                (this.Location.Y - this.spriteTexture.Height / 2 < 0)
                )
            {
                this.Direction.Y *= -1;
                this.ghost.State = GhostState.Roving;
            }
            if ((this.Location.X + this.spriteTexture.Width / 2 > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X - this.spriteTexture.Width / 2 < 0)
                )
            {
                this.Direction.X *= -1;
                this.ghost.State = GhostState.Roving;
            }

            

            base.Update(gameTime);
        }


        //Possible move to Sprite
        public Vector2 GetRandomDirection()
        {
            Vector2 v = new Vector2((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f);
            Vector2.Normalize(ref v, out v);    //Normalize
            return v;
        }

        public Vector2 GetRandLocation()
        {
            //System.Threading.Thread.Sleep(1);
            Vector2 loc;
            loc.X = r.Next(Game.Window.ClientBounds.Width - this.spriteTexture.Width) + this.spriteTexture.Width;
            loc.Y = r.Next(Game.Window.ClientBounds.Height - this.spriteTexture.Height) + this.spriteTexture.Height;
            return loc;
        }


    }
}
