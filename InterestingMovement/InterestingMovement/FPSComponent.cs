using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
namespace InterestingMovement
{
    public sealed class FPSComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private bool updateTimeFixed;
        private bool synchronizeWithVerticalRetrace;

        float frameRate;
        float frameCounter;
        TimeSpan elapsedTime = TimeSpan.Zero;

        string fps;

        public FPSComponent(Game game, bool synchWithVerticalRetrace, bool isFixedTimeStep) : this(game, synchWithVerticalRetrace, isFixedTimeStep, game.TargetElapsedTime)
        {

        }

        public FPSComponent(Game game) : this(game, false, false) { }

        public FPSComponent(Game game, bool synchWithVerticalRetrace, bool isFixedTimeStep, TimeSpan targetElapsedTime) : base(game)
        {
            GraphicsDeviceManager graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));

            graphics.SynchronizeWithVerticalRetrace = synchWithVerticalRetrace;
            Game.IsFixedTimeStep = isFixedTimeStep;
            Game.TargetElapsedTime = targetElapsedTime;

            updateTimeFixed = Game.IsFixedTimeStep;
            graphics.ApplyChanges();
        }

        public void ToggleTimeFixed()
        {
            if (updateTimeFixed)
            {
                updateTimeFixed = false;

            }
            else
            {
                updateTimeFixed = true;

            }
            GraphicsDeviceManager graphics =
            (GraphicsDeviceManager)Game.Services.GetService(
            typeof(IGraphicsDeviceManager));

            Game.IsFixedTimeStep = updateTimeFixed;
            graphics.ApplyChanges();
        }

        public void ToggleSynchronizeWithVerticalRetrace()
        {
            if (synchronizeWithVerticalRetrace)
            {
                synchronizeWithVerticalRetrace = false;
            }
            else
            {
                synchronizeWithVerticalRetrace = true;
            }

            GraphicsDeviceManager graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));

            graphics.SynchronizeWithVerticalRetrace = synchronizeWithVerticalRetrace;
            graphics.ApplyChanges();
        }

        ///
        /// Allows the game component to perform any initialization 
        /// it needs to before starting to run.  This is where it can query for
        /// any required services and load content.
        /// 
        public sealed override void Initialize()
        {

            base.Initialize();
        }

        ///
        /// Allows the game component to update itself.
        /// 
        ///Provides snapshot of timing values.
        public sealed override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            base.Update(gameTime);
        }

        public sealed override void Draw(GameTime gameTime)
        {
            frameCounter++;
            fps = string.Format("fps: {0}", frameRate);

            //int fps = (int)Math.Round(1.0 / gameTime.ElapsedGameTime.TotalSeconds);    
            Game.Window.Title = "FPS: " + fps + " " + gameTime.IsRunningSlowly;
            base.Draw(gameTime);
        }
    }
}
