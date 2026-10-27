using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutStep1
{
    public class MonogameBlock : DrawableSprite
    {
        protected Block block;
        Ball ball; // reference to ball, needed for collision

        protected string NormalTextureName, HitTextureName;
        protected Texture2D NormalTexture, HitTexture;

        private BlockState blockstate;
        public BlockState BlockState
        {
            get { return this.block.BlockState; } //encapulsate block.BlockState
            set { this.block.BlockState =  value; }
        }

        public MonogameBlock(Game game, Ball ball) : base(game)
        {
            this.block = new Block();
            this.ball = ball;
            NormalTextureName = "block_blue";
            HitTextureName = "block_bubble";
        }

        protected virtual void updateBlockTexture()
        {

            this.blockstate = this.block.BlockState;
            switch (block.BlockState)
            {
                case BlockState.Normal:
                    this.Visible = true;
                    this.spriteTexture = NormalTexture;
                    break;
                case BlockState.Hit:
                    this.spriteTexture = HitTexture;
                    break;
                case BlockState.Broken:
                    this.spriteTexture = NormalTexture;
                    //this.enabled = false;
                    this.Visible = false; //don't show block

                    break;
            }
        }

        private void UpdateCollision()
        {
            // when ball interesects block, block is hit by ball
            if (this.Intersects(ball))
            {
                HitByBall(ball);
            }

            // guards
            if (this.BlockState == BlockState.Broken) return;
            if (ball.State != BallState.Playing) return;
        }

        protected override void LoadContent()
        {
            this.NormalTexture = this.Game.Content.Load<Texture2D>(NormalTextureName);
            this.HitTexture = this.Game.Content.Load<Texture2D>(HitTextureName);
            updateBlockTexture(); //notice this is in loadcontent not the constuctor
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateCollision();
            block.UpdateBlockState();
            MonoGameBlockUpdate();
        }

        protected virtual void MonoGameBlockUpdate()
        {
            updateBlockTexture();
        }

        public void HitByBall(Ball ball)
        {
            this.block.Hit(); // iterate the hit count, update state
            ball.BounceOffBlock(this); // ball bounces off this block
        }
    }
}
