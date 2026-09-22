using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacManGameComponent
{
    public class Ghost : DrawableSprite
    {

        public enum GhostState { Chasing, Evading, Roving, Dead }
        GhostState _state;

        Texture2D ghostTexture, ghostHitTexture;
        PacMan pacMan;

        GameConsole console;

        public Ghost(Game game, PacMan pac) : base(game)
        {
            _state = GhostState.Evading;
            this.pacMan = pac;

            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
        }

        public void Evade()
        {
            this._state = GhostState.Evading;
        }

        protected override void LoadContent()
        {
            this.ghostTexture = this.SpriteTexture = this.Game.Content.Load<Texture2D>("PurpleGhost");
            this.ghostHitTexture = this.Game.Content.Load<Texture2D>("GhostHit");
            //TODO add ghost starting location
            this.Location = new Vector2(100, 100); //temp starting locations 
            this.Direction = new Vector2(1, 0);
            this.Speed = 20;
            base.LoadContent();
        }

        const float turnAmount = .04f;

        public override void Update(GameTime gameTime)
        {
            UpdateDirection();

            UpdateKeepGhostOnScreen();

            UpdateCollision();

            UpdateTexture();

            Location += ((this.Direction * (lastUpdateTime / 1000)) * Speed);      //Simple Move

            UpdateLogStateToConsole();

            base.Update(gameTime);
        }

        private void UpdateLogStateToConsole()
        {
            console.DebugTextOutput["GhostState"] = this._state.ToString();
            console.DebugTextOutput["GhostSpeed"] = this.Speed.ToString() ;
        }

        protected virtual void UpdateDirection()
        {
            switch (this._state)
            {
                case GhostState.Dead:
                    UpdateGhostDead();
                    break;
                case GhostState.Chasing:
                    UpdateGhostChasing();
                    break;
                case GhostState.Evading:
                    UpdateGhostEvading();
                    break;
                case GhostState.Roving:
                    UpdateGhostRoving();

                    break;
            }
        }

        protected virtual void UpdateGhostDead()
        {
            //TODO Dead moving and dead animation.
            //Until then
            this._state = GhostState.Roving;
        }

        protected virtual void UpdateGhostChasing()
        {
            if (pacMan.Location.Y > this.Location.Y)
            {
                //this.Direction.Y = 1;
                this.Direction.Y = MathHelper.Clamp(this.Direction.Y += turnAmount, -1, 1);
            }
            else
            {
                //this.Direction.Y = -1;
                this.Direction.Y = MathHelper.Clamp(this.Direction.Y -= turnAmount, -1, 1);
            }
            if (pacMan.Location.X > this.Location.X)
            {
                //this.Direction.X = 1;
                this.Direction.X = MathHelper.Clamp(this.Direction.X += turnAmount, -1, 1);
            }
            else
            {
                //this.Direction.X = -1;
                this.Direction.X = MathHelper.Clamp(this.Direction.X -= turnAmount, -1, 1);
            }
        }

        protected virtual void UpdateGhostEvading()
        {
            if (pacMan.Location.Y > this.Location.Y)
            {
                this.Direction.Y = -1;
            }
            else
            {
                this.Direction.Y = 1;
            }
            if (pacMan.Location.X > this.Location.X)
            {
                this.Direction.X = -1;
            }
            else
            {
                this.Direction.X = -1;
            }
        }

        protected virtual void UpdateGhostRoving()
        {
            //check if ghost can see pacman
            Vector2 normD = Vector2.Normalize(this.Direction);
            Vector2 p = new Vector2(this.Location.X, this.Location.Y);
            while (p.X < this.Game.GraphicsDevice.Viewport.Width &&
               p.X > 0 &&
               p.Y < this.Game.GraphicsDevice.Viewport.Height &&
               p.Y > 0)
            {
                if (pacMan.LocationRect.Contains(new Point((int)p.X, (int)p.Y)))
                {
                    this._state = GhostState.Chasing;
                    //this.ghost.Log(this.ToString() + " saw pacman");
                    break;
                }
                p += this.Direction;
            }
        }

        protected virtual void UpdateTexture()
        {
            switch (this._state)
            {
                case GhostState.Roving :
                case GhostState.Chasing:
                    this.SpriteTexture = ghostTexture;
                    break;
                case GhostState.Dead:
                case GhostState.Evading:
                    this.SpriteTexture = ghostHitTexture;
                    break;
            }
        }

        protected virtual void UpdateCollision()
        {
            //Collision
            if (this.Intersects(pacMan))
            {
                //this.ghost.Log(this.ToString() + " Intersects PacsMan" + gameTime.TotalGameTime.Seconds + "." + gameTime.TotalGameTime.Milliseconds);
                //PerPixel Collision
                if (this.PerPixelCollision(pacMan))
                {
                    //this.ghost.Log(this.ToString() + " Pixels touched PacsMan" + gameTime.TotalGameTime.Seconds + "." + gameTime.TotalGameTime.Milliseconds);
                    this._state = GhostState.Dead;

                }
            }
        }

        protected virtual void UpdateKeepGhostOnScreen()
        {
            //Borders Keep Ghost on the Screen
            if ((this.Location.Y + this.spriteTexture.Height / 2 > this.Game.GraphicsDevice.Viewport.Height)
                ||
                (this.Location.Y - this.spriteTexture.Height / 2 < 0)
                )
            {
                this.Direction.Y *= -1;
                this._state = GhostState.Roving;
            }
            if ((this.Location.X + this.spriteTexture.Width / 2 > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X - this.spriteTexture.Width / 2 < 0)
                )
            {
                this.Direction.X *= -1;
                this._state = GhostState.Roving;
            }
        }
    }
}
