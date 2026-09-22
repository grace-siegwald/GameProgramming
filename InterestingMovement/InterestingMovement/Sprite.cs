using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InterestingMovement
{
    public class Sprite
    {
        public float Direction;
        public Vector2 Location;

        public int TextureName
        {
            get => default;
            set
            {
            }
        }
    }
}