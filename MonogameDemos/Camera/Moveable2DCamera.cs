using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Camera
{
    public class Moveable2DCamera : Simple2DCamera
    {
        KeyBinds _keyBinds;

        public Moveable2DCamera(Viewport viewport) : base(viewport)
        {
            _keyBinds = new KeyBinds(); //setup keybings with default keys
        }

        private float currentMouseWheelValue, previousMouseWheelValue;

        protected override void UpdateCameraMove(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(KeyBinds.CameraMoveUp))
            {
                cameraMovement.Y = -moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(KeyBinds.CameraMoveDown))
            {
                cameraMovement.Y = moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(KeyBinds.CameraMoveLeft))
            {
                cameraMovement.X = -moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(KeyBinds.CameraMoveRight))
            {
                cameraMovement.X = moveSpeed;
            }

            previousMouseWheelValue = currentMouseWheelValue;
            currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;

            if (currentMouseWheelValue > previousMouseWheelValue)
            {
                AdjustZoom(.05f);
                //Console.WriteLine(moveSpeed);
            }

            if (currentMouseWheelValue < previousMouseWheelValue)
            {
                AdjustZoom(-.05f);
                //Console.WriteLine(moveSpeed);
            }
        }
    }
}
