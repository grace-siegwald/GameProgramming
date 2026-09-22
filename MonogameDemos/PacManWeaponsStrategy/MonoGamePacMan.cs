using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Util;
using StrategyPacMan.weapons;

namespace PacManWeaponsStrategy
{
    public class MonoGamePacMan : MonoGameLibrary.Sprite.DrawableSprite
    {
        internal PacManController controller { get; private set; }

        foodWeapon pacWeapon;

        internal GameConsolePacMan PacMan
        {
            get;
            private set;
        }
        protected PacManState pacState;
        public PacManState PacState
        {
            get { return this.pacState; }
            //Change pacman state also
            set { 
                if(this.pacState != value)
                    this.pacState = this.PacMan.State = value; 
            }
        }
                
        public MonoGamePacMan(Game game)
            : base(game)
        {
            this.controller = new PacManController(game);
            PacMan = new GameConsolePacMan((GameConsole)game.Services.GetService<IGameConsole>());
            this.pacWeapon = new NoWeapon(game);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.SpriteTexture = this.Game.Content.Load<Texture2D>("pacManSingle");
            this.Origin = new Vector2(this.SpriteTexture.Width / 2, this.SpriteTexture.Height / 2);
            this.Location = new Microsoft.Xna.Framework.Vector2(100, 100);
            this.Speed = 200;
        }

        public override void Update(GameTime gameTime)
        {
            //Elapsed time since last update
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            this.controller.Update();

            this.Location += ((this.controller.Direction * (time / 1000)) * Speed);      //Simple Move 
            this.Rotate = this.controller.Rotate;

            //Change State based on movement
            if (this.controller.hasInputForMoverment)
            {
                if(pacState != PacManState.Spawning && pacState != PacManState.SuperPacMan)
                    this.PacState = PacManState.Chomping;
            }
            else
            {
                if (pacState != PacManState.Spawning && pacState != PacManState.SuperPacMan)
                    this.PacState = PacManState.Still;
            }

            //Keep PacMan On Screen
            if (this.Location.X > Game.GraphicsDevice.Viewport.Width - (this.spriteTexture.Width / 2))
            {
                this.Location.X = Game.GraphicsDevice.Viewport.Width - (this.spriteTexture.Width / 2);
            }
            if (this.Location.X < (this.spriteTexture.Width / 2))
                this.Location.X = (this.spriteTexture.Width / 2);

            if (this.Location.Y > Game.GraphicsDevice.Viewport.Height - (this.spriteTexture.Height / 2))
                this.Location.Y = Game.GraphicsDevice.Viewport.Height - (this.spriteTexture.Height / 2);

            if (this.Location.Y < (this.spriteTexture.Height / 2))
                this.Location.Y = (this.spriteTexture.Height / 2);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            //base.Draw(sb);

            sb.Draw(spriteTexture,
                new Rectangle(
                    (int)Location.X,
                    (int)Location.Y,
                    (int)(spriteTexture.Width * this.Scale),
                    (int)(spriteTexture.Height * this.Scale)),
                null,
                this.pacWeapon.color,
                MathHelper.ToRadians(Rotate),
                this.Origin,
                SpriteEffects,
                0);


            DrawMarkers(sb);
        }

        internal void GiveWeapon(foodWeapon foodWeapon)
        {
            this.pacWeapon = foodWeapon;
        }
    }
}
