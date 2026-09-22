using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneButtonPacman
{
    public class OneButtonPlayerController : GameComponent
    {
        InputHandler inputHander;

        Keys controllerKey;

        public bool IsKeyPressed { get; set; }

        public OneButtonPlayerController(Game game) : base(game)
        {
            inputHander = (InputHandler)game.Services.GetService<IInputHandler>();
            if(inputHander == null)
            {
                inputHander = new InputHandler(game);
                game.Components.Add(inputHander);
            }

            controllerKey = Keys.Space;
            
        }


        public override void Update(GameTime gameTime)
        {
            IsKeyPressed = inputHander.KeyboardState.IsKeyDown(controllerKey);
            base.Update(gameTime);
        }

    }
}
