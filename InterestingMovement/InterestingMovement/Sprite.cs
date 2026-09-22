using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InterestingMovement
{
    public class Sprite
    {
        public Vector2 Direction;
        public Vector2 Location;
        public Vector2 SpawnLocation;
        public float Speed;
        public Vector2 GravityDirection;
        public float GravityAcceleration;
        public Color Color;

        public Game Game;
        public Texture2D Texture;
        float time;


        public string TextureName;

        public Sprite(string textureName, Color color)
        {
            TextureName = textureName;
            Color = color;
        }

        public void LoadContent(Game game, Vector2 location, float speed, Vector2 direction)
        {
            Game = game;
            Texture = Game.Content.Load<Texture2D>(TextureName);
            Location = location;
            Direction = direction;
            Speed = speed;
            GravityDirection = new Vector2(0, 1);
            GravityAcceleration = 1.8f;
        }
        public void Update(GameTime gameTime)
        {
            // apply gravity before move
            time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            UpdateGravity(gameTime);
            UpdateMove(gameTime);
        }
        private void UpdateGravity(GameTime gameTime)
        {
            time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Direction += GravityDirection * GravityAcceleration * (time / 1000);
        }
        private void UpdateMove(GameTime gameTime)
        {
            time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Location += Direction * Speed * (time / 1000);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, Color);
        }
    }
}