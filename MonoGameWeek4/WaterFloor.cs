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
        public WaterFloor(Game game) : base(game)
        {
            Scale = 2f; // scale to match the window size
        }

        // I'm not sure why I couldn't just set this value in load content, but I couldn't get it to work that way. This way, every time the surface is called, it will calculate the value based on the current location and scale.
        public float Surface
        {
            get { return Location.Y - Origin.Y * Scale + (6 * Scale); }
        }

        protected override void LoadContent()
        {
            SpriteTexture = Game.Content.Load<Texture2D>("water");
            base.LoadContent();

            Location = new Vector2(Game.GraphicsDevice.Viewport.Width / 2,
                Game.GraphicsDevice.Viewport.Height - SpriteTexture.Height * Scale / 2);
        }
    }
}
