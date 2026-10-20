using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace OneButtonGame
{
    // Decides WHEN a wave goes out and HOW FAST it moves.
    // Keeps a "pool" of Wave objects and reuses whichever one is free,
    // so several waves can be on screen at once.
    public class ObstacleSpawner : GameComponent
    {
        Random random = new Random();
        public List<Wave> Waves { get; private set; }

        // Speed: each new wave is a bit faster than the last
        public float StartSpeed = 200f;
        public float SpeedIncrease = 20f;
        public float SpeedCap = 600f;
        float currentSpeed;

        // Spacing: random number of seconds between waves
        public float MinGap = 1.4f;
        public float MaxGap = 3.0f;
        float spawnTimer; // seconds until the next wave goes out

        public ObstacleSpawner(Game game, WaterFloor water, int poolSize) : base(game)
        {
            Waves = new List<Wave>();
            for (int i = 0; i < poolSize; i++)
            {
                Wave wave = new Wave(game, water);
                Waves.Add(wave);
                game.Components.Add(wave);   // each wave is still its own GameComponent
            }
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spawnTimer -= time;

            if (spawnTimer <= 0)   // time for the next wave
            {
                Wave freeWave = GetFreeWave();
                if (freeWave != null)   // null = every wave is already out, so try again next frame
                {
                    freeWave.Spawn(currentSpeed);
                    currentSpeed = Math.Min(currentSpeed + SpeedIncrease, SpeedCap);
                    WaveSpacer();   // pick how long until the one after this
                }
            }

            base.Update(gameTime);
        }

        // Random wait before the next wave, somewhere between MinGap and MaxGap seconds
        private void WaveSpacer()
        {
            spawnTimer = MinGap + (float)random.NextDouble() * (MaxGap - MinGap);
        }

        // The first wave that isn't out on the water, or null if they're all busy
        private Wave GetFreeWave()
        {
            foreach (Wave wave in Waves)
            {
                if (!wave.Active)
                {
                    return wave;
                }
            }
            return null;
        }

        // Start-of-game state: no waves out, slow speed, short pause before the first one
        public void Reset()
        {
            foreach (Wave wave in Waves)
            {
                wave.Despawn();
                wave.Enabled = true;   // undo Freeze()
            }
            currentSpeed = StartSpeed;
            spawnTimer = 0.5f;
            Enabled = true;
        }

        // Game over: stop spawning and freeze every wave where it is
        public void Freeze()
        {
            Enabled = false;
            foreach (Wave wave in Waves)
            {
                wave.Enabled = false;   // stops Update, but they're still drawn
            }
        }
    }
}
