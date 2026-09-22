using Microsoft.Xna.Framework.Input;

namespace Camera
{
    internal class KeyBinds
    {
        public static Keys CameraMoveUp { get; internal set; }
        public static Keys CameraMoveDown { get; internal set; }
        public static Keys CameraMoveLeft { get; internal set; }
        public static Keys CameraMoveRight { get; internal set; }

        public KeyBinds()
        {
            CameraMoveUp = Keys.Up;
            CameraMoveDown = Keys.Down;
            CameraMoveLeft = Keys.Left;
            CameraMoveRight = Keys.Right;
        }
    }
}