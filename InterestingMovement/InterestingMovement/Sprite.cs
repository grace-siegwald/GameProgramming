using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InterestingMovement
{
    public class Sprite 
    {
        // Set to defult values, can be changed on initialization
        public Vector2 Location;
        public Vector2 Direction = new Vector2(1, 0);
        public Vector2 SpawnLocation = new Vector2(0, 0);
        public float Speed = 200f;
        public Vector2 GravityDirection = new Vector2(0, 1);
        public float GravityAcceleration = 1.8f;
        public Color Color = Color.White;
        public Vector2 Origin;

        // This stuff gets set once methinks
        public Game Game ;
        public Texture2D Texture;
        public string TextureName;

        public Sprite(Game game, string textureName)
        {
            this.Game = game;
            TextureName = textureName;
        }

        public virtual void LoadContent()
        {
            Texture = Game.Content.Load<Texture2D>(TextureName);

            Origin = new Vector2(Texture.Width /2, Texture.Height /2);
        }
        public virtual void Update(GameTime gameTime)
        {
            // apply gravity before move
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // passing in the "local" time to the other update methods:
            UpdateGravity(time);
            UpdateMove(time);
        }
        private void UpdateGravity(float time)
        {
            Direction += GravityDirection * GravityAcceleration * (time / 1000);
        }
        private void UpdateMove(float time)
        {
            Location += Direction * Speed * (time / 1000);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, Color);
        }
    }
}