using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMovementWRotate
{
    class Sprite
    {
        protected Texture2D Texture;

        protected Vector2 Orgin;    //Orgin for Drawing
        protected Vector2 Location;      //Pacman location
        protected Vector2 Direction;      //Pacman direction
        protected float Speed;      //speed for the PacMan Sprite in pixels per frame per second
        protected float Rotate;     //degrees

        public string TextureName { get; set; }

        protected Game game;


        //Sprite is dependant on a Game
        public Sprite(Game game)
        {
            this.game = game;

            this.Speed = 100;

        }

        public virtual void LoadContent()
        {
            if (string.IsNullOrEmpty(TextureName))
                TextureName = "pacmanSingle";       //sneaky
            Texture = game.Content.Load<Texture2D>(TextureName);
            //Set PacMan Location to center of screen
            this.Location = new Vector2(game.GraphicsDevice.Viewport.Width / 2,
                game.GraphicsDevice.Viewport.Height / 2);
            //Vector for pacman direction
            //notice this vector has no magnitude it's noramlized
            this.Direction = new Vector2(0, 0);

            //Orgin shoud be center of texture
            this.Orgin = new Vector2(this.Texture.Width / 2, this.Texture.Height / 2);


            //Pacman spped Speed = 100; //units per second
        }

        float time;

        public virtual void Update(GameTime gameTime)
        {
            //Elapsed time since last update will be used to correct movement speed
            time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Time corrected move. MOves Sprite By Div every Second
            this.Location = this.Location + ((this.Direction * this.Speed) * (time / 1000));      //Simple Move PacMan by PacManDir

        }


        /// <summary>
        /// Draw Expects a Spritebatch that is that has begun
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)  //association
        {

            
            spriteBatch.Draw(this.Texture,  //texture2D
                new Rectangle(        //Create rectange to draw to
                    (int)this.Location.X,
                    (int)this.Location.Y,
                    (int)(this.Texture.Width),
                    (int)(this.Texture.Height)),
                null,   //no source rectangle
                Color.White,
                MathHelper.ToRadians(this.Rotate), //rotation in radians
                this.Orgin,   //0,0 is top left
                SpriteEffects.None,
                0);

        }
    }
}
