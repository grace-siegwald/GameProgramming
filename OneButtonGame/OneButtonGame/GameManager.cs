using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OneButtonGame
{
    public enum GameState // The different states the game can be in
    {
        Title,
        Playing,
        Won,
        Lost
    }
    
    // THIS is the only class that is friends with both the wave and Kirby himself. That's why it's the manager class. And it's paid better than Kirby. Because manager
    // and also like most managers they're not doing much, just "watching" and "overseeing" or whatever they do all day
    public class GameManager : GameComponent
    {
        Kirby kirby;
        Wave wave;
        IInputHandler input;

        // These are our PROPERTIES. Wow. So official
        // Other classes can read these, but only the manager can change their values. Encapsulation! Boom!
        public GameState State { get; private set; }
        public int Score { get; private set; }
        public int HighScore { get; private set; }
        public bool NewHighScore { get; private set; }

        public int WavesToWin = 10;
        public int LifeBonus = 5; // extra points for if you keep more lives :)

        public int Lives { get { return kirby.Lives; } } // just so we can get the lives on the hub without instantiating a whole new kirby

        public GameManager(Game game, Kirby kirby, Wave wave) : base(game)
        {
            this.kirby = kirby;
            this.wave = wave;
        }

        public override void Initialize()
        {
            input = Game.Services.GetService<IInputHandler>();
            State = GameState.Title; // obviously the state we start the game in
            wave.Enabled = false; // so there's no waves on the title screen :)
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // HERE is where we check the two states of the ONE BUTTON in the game: pressed or not pressed
            // Space on the keyboard OR A on the gamepad
            bool buttonPressed = input.WasPressed(0, InputHandler.ButtonType.A, Keys.Space);

            switch (State)
            {
                case GameState.Title:
                    if (buttonPressed) { StartGame(); }
                    break;
                case GameState.Playing:
                    if (buttonPressed) { kirby.Jump(); } // if space is pressed, jump
                    CheckHit();
                    CheckJumped();
                    CheckWinOrLose();
                    break;
                case GameState.Won:
                    if (buttonPressed) { StartGame(); } // loop back to the start of the game
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
            wave.Reset();
            wave.Enabled = true;

            Score = 0;
            NewHighScore = false;
            State = GameState.Playing;
        }

        private void CheckHit()
        {
            if (wave.Counted) {return;} // we get outa here if the wave has already been counted (either hit kirby or jumped over)

            // hitbox intersection check first, then pixel collision check because that wave is not a rectangle lol
            if (kirby.Intersects(wave) && kirby.PerPixelCollisionWTansform(wave))
            {
                kirby.Hit();
                wave.MarkCounted(); 
            }
        }
        private void CheckJumped()
        {
            if (wave.Counted) {return;} // once again, we get outa here

            // if kirby's left edge is PAST the wave's right edge...
            if (kirby.LocationRect.Left > wave.LocationRect.Right) 
            {
                Score++; // increment the score
                wave.MarkCounted();
            }
        }
        private void CheckWinOrLose()
        {
            if (kirby.Lives <= 0)
            {
                EndGame(GameState.Lost); 
            }
            else if (Score >= WavesToWin)
            {
                Score += kirby.Lives * LifeBonus; // fun lil bonus for keeping lives
                EndGame(GameState.Won);
            }
        }

        private void EndGame(GameState state)
        {
            State = state;
            wave.Enabled = false; // freezes wave, claude helped me find this it's COOL

            if (Score > HighScore)
            {
                HighScore = Score;
                NewHighScore = true;
            }
        }
    }
}
