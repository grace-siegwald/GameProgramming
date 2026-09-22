using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrollingBackgound
{
    

    class ScrollingBackground : DrawableGameComponent
    {

        public string TextureName { get; set; }

        float scrollSpeed;

        public Color Color;
        protected Texture2D currentBG;
        Vector2 currentOffset;
        protected Rectangle currentDR, nextDR, prevDR;

        private int width, height;
        SpriteBatch spriteBatch;


        GameConsole console;

        public ScrollingBackground(Game game) : base (game)
        {
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            if (TextureName == String.Empty || TextureName == null)
            {
                TextureName = "ocean-waves-wallpaper-800x480-334";
            }

            Color = Color.White;
            currentOffset = Vector2.Zero;

            //Size is only set on creation will not resize after game is running
            //default size is  800 x 480
            this.width = this.Game.GraphicsDevice.Viewport.Width;
            this.height = this.GraphicsDevice.Viewport.Height;

            LoadBackGround();
            this.scrollSpeed = -5;

            base.LoadContent();
        }

        private void LoadBackGround()
        {
            this.currentBG = this.Game.Content.Load<Texture2D>(TextureName);

            SetDrawRectangles();
        }

        private void SetDrawRectangles()
        {
            this.currentDR = new Rectangle(0 + (int)this.currentOffset.X, 0 + (int)this.currentOffset.Y, this.width, this.height);
            this.prevDR = new Rectangle(-this.width + (int)this.currentOffset.X, 0 + (int)this.currentOffset.Y, this.width, this.height);
            this.nextDR = new Rectangle(this.width + (int)this.currentOffset.X, 0 + (int)this.currentOffset.Y, this.width, this.height);
        }

        
        public override void Update(GameTime gameTime)
        {
            UpdateMoveBackGround(gameTime);
            this.SetDrawRectangles();
            base.Update(gameTime);
        }

        protected virtual void UpdateMoveBackGround(GameTime gameTime)
        {
            this.currentOffset.X -= this.scrollSpeed * ((gameTime.ElapsedGameTime.Milliseconds)/10);
            if(this.currentOffset.X < -this.width)
            {
                this.currentOffset.X = 0;
            }
            if (this.currentOffset.X > this.width)
            {
                this.currentOffset.X = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //prev
            spriteBatch.Draw(this.currentBG,
                prevDR, Color);
            //current
            spriteBatch.Draw(this.currentBG,
                currentDR, Color);
            //next

            spriteBatch.Draw(this.currentBG,
                nextDR, Color);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
