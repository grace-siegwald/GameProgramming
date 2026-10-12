using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;

namespace OneButtonGame
{
    public class WaterFloor : DrawableSprite
    {
        public int SinkDepth;

        // the y position that things "stand on"
        public float Surface // property so it's always calculated from the water surfaces current location
        {
            get { return this.Location.Y - Origin.Y * Scale + SinkDepth; }
        }
        public WaterFloor(Game game) : base(game)
        {
            Scale = 2f; // scale to match the window size
            ShowMarkers = true;
        }

        protected override void LoadContent()
        {
            SpriteTexture = Game.Content.Load<Texture2D>("water");
            base.LoadContent();

            Location = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height - SpriteTexture.Height * Scale / 2);
            SinkDepth = (int)(10 * Scale);
        }
    }
}
