using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGameLibrary.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;

namespace ShotManager
{

    public class Shot : Sprite
    {
        float elapsedtime;
        public Vector2 StartLocation { get; set; }
        private string shotTexture; //private instance data menber
        public string ShotTexture
        {
            get { return this.shotTexture; }
            set
            {
                this.shotTexture = value;
                this.LoadContent();
            }
        }

        public Shot(Game game) : base(game)
        {
            if (String.IsNullOrEmpty(ShotTexture))
            {
                this.ShotTexture = "shot";
            }   
        }

        

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>(ShotTexture);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedtime = gameTime.ElapsedGameTime.Milliseconds;
            if (this.Location == null) this.Location = StartLocation;
            this.Location += (this.Direction * this.Speed * (elapsedtime / 1000));

            if(this.IsOffScreen())
            {
                this.Enabled = false;
            }

            base.Update(gameTime);
        }
    }
}
