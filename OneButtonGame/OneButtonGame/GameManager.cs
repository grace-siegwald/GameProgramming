using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace OneButtonGame
{
    public enum GameState // The different states the game can be in
    {
        Title,
        Playing,
        Lost
    }
    
    // THIS is the only class that is friends with both the wave and Kirby himself. That's why it's the manager class. And it's paid better than Kirby. Because manager
    // and also like most managers they're not doing much, just "watching" and "overseeing" or whatever they do all day
    public class GameManager : GameComponent
    {
        Kirby kirby;
        ObstacleSpawner spawner;
        IInputHandler input;

        // These are our PROPERTIES. Wow. So official
        // Other classes can read these, but only the manager can change their values. Encapsulation! Boom!
        public GameState State { get; private set; }
        public int Score { get; private set; }
        public int HighScore { get; private set; }
        public bool NewHighScore { get; private set; }

        public int Lives { get { return kirby.Lives; } } // just so we can get the lives on the hub without instantiating a whole new kirby


        public GameManager(Game game, Kirby kirby, ObstacleSpawner spawner) : base(game)
        {
            this.kirby = kirby;
            this.spawner = spawner;
        }

        public override void Initialize()
        {
            input = Game.Services.GetService<IInputHandler>();
            State = GameState.Title; // obviously the state we start the game in
            spawner.Enabled = false; // no waves on the title screen :)
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // HERE is where we check the two states of the ONE BUTTON in the game: pressed or not pressed
            // Space on the keyboard OR A on the gamepad
            bool buttonPressed = input.WasPressed(0, InputHandler.ButtonType.A, Keys.Space);
            bool buttonHeld = input.KeyboardState.IsKeyDown(Keys.Space) || input.GamePads[0].Buttons.A == ButtonState.Pressed;

            switch (State)
            {
                case GameState.Title:
                    if (buttonPressed) { StartGame(); }
                    break;
                case GameState.Playing:
                    if (buttonPressed) { kirby.Jump(); } // if space is pressed, jump
                    if (!buttonHeld) { kirby.ReleaseJump(); }
                    CheckHit();
                    CheckJumped();
                    CheckLose();
                    break;
                case GameState.Lost:
                    if (buttonPressed) { StartGame(); }
                    break;
            }
            base.Update(gameTime);
        }

        private void StartGame()
        {
            kirby.Reset();
            kirby.Enabled = true;   // undo EndGame turning him off
            spawner.Reset();

            Score = 0;
            NewHighScore = false;
            State = GameState.Playing;
        }

        private void CheckHit()
        {
            foreach (Wave wave in spawner.Waves)
            {
                if (!wave.Active || wave.Counted) { continue; }   // skip waves that aren't out, or already counted

                if (kirby.Intersects(wave) && kirby.PerPixelCollisionWTansform(wave))
                {
                    kirby.Hit();
                    wave.MarkCounted();
                }
            }
        }
        private void CheckJumped()
        {
            foreach (Wave wave in spawner.Waves)
            {
                if (!wave.Active || wave.Counted) { continue; }

                if (kirby.LocationRect.Left > wave.LocationRect.Right)
                {
                    Score++;
                    wave.MarkCounted();
                }
            }
        }
        
        // Endless runner: the only way a run ends is losing all your lives
        private void CheckLose()
        {
            if (kirby.Lives <= 0)
            {
                EndGame(GameState.Lost);
            }
        }

        private void EndGame(GameState state)
        {
            State = state;
            spawner.Freeze(); // stop spawning AND freeze every wave where it is
            kirby.Enabled = false;

            if (Score > HighScore)
            {
                HighScore = Score;
                NewHighScore = true;
            }
        }
    }
}
