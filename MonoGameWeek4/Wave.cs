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
    public class Wave : DrawableSprite
    {
        // TODO: add a wave sprite that moves across the water floor and interacts with Kirby when it collides with him
        public Wave(Game game) : base(game)
        {
        }
    }
}
