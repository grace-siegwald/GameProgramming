using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneButtonPacman
{

    /// <summary>
    /// State Enum for pac man we will use this later
    /// </summary>
    public enum PacState
    {
        Stopped, Chomping
    }


    public class OneButtonPacMan : DrawableSprite
    {
        protected OneButtonPlayerController controller;
        public PacState CurrentState { get; protected set; }

        public OneButtonPacMan(Game game) : base (game)
        {
            controller = new OneButtonPlayerController(this.Game);
            this.Game.Components.Add(controller);

            this.CurrentState = PacState.Stopped;
        }

        protected override void LoadContent()
        {
            this.SpriteTexture = this.Game.Content.Load<Texture2D>("Pacmansingle");
            this.Location = new Vector2(100, 100);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateStateFromController(gameTime);
            UpdateColorFromState(gameTime);
            base.Update(gameTime);
        }

        private void UpdateStateFromController(GameTime gameTime)
        {
            if (controller.IsKeyPressed)
            {
                this.CurrentState = PacState.Chomping;
            }
            else
            {
                this.CurrentState = PacState.Stopped;
            }
        }


        private void UpdateColorFromState(GameTime gameTime)
        {
            switch(this.CurrentState)
            {
                case PacState.Stopped:
                    this.DrawColor = Color.White;
                    break;
                case PacState.Chomping:
                    this.DrawColor = Color.Red;
                    break;
            }
        }
    }
}
