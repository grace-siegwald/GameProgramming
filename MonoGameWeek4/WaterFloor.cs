using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;

namespace MonoGameWeek4
{
    public class WaterFloor : DrawableSprite
    {
        public float Surface;
        public WaterFloor(Game game) : base(game)
        {
            Surface = Location.Y - 100; // Just sets the surface level, we use this in the Kirby class to check is he's on the "floor" or not
            Scale = 2f; // scale to match the widow size
        }
        protected override void LoadContent()
        {
            SpriteTexture = Game.Content.Load<Texture2D>("water");
            base.LoadContent();

            Location = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height - SpriteTexture.Height / 2);
        }
    }
}
