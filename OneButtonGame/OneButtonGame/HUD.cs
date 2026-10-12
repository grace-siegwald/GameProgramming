using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OneButtonGame
{
    // Bro only draws: reads all its info from the manager but never changes anything (Because it can't! Because encapsulation!)
    public class HUD : DrawableGameComponent
    {
        // all the objects this class needs to draw stuff
        GameManager manager;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D ratingTexture;

        public float TextScale = 1f; // easy to scale the font if too big or small :)

        int lastScore;
        float ratingTimer = 0f;

        public HUD(Game game, GameManager manager) : base(game) 
        { 
            this.manager = manager;
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.spriteFont = Game.Content.Load<SpriteFont>("daydreamFont");
            spriteFont.DefaultCharacter = ' '; // Needed this to prevent from crashing, claude found this error, the font I downloaded has no : or -
            this.ratingTexture = Game.Content.Load<Texture2D>("great");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds; // always gotta get time! time follows all!

            // little rating texture pops up if player gets a point
            if (manager.State == GameState.Playing && manager.Score > lastScore)
            {
                ratingTimer = 0.75f;
            }
            lastScore = manager.Score; // need to store the last score so we know what to compare to

            if (ratingTimer > 0) // needed to keep timer synced correctly
            {
                ratingTimer -= time;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            switch (manager.State)
            {
                // I had Claude help me with the text formating methods because HEADACHE
                case GameState.Title:
                    DrawCenteredLines(new string[] {
                        "Press SPACE to jump",
                        "",
                        "Clear " + manager.WavesToWin + " waves to win",
                        "3 hits and lose",
                        "Bonus points for keeing your lives!",
                        "",
                        "Press SPACE to start"
                    }, 90);
                    break;
                case GameState.Playing:
                    DrawText("Waves " + manager.Score + "/" + manager.WavesToWin, new Vector2(12, 12));
                    DrawText("Lives " + manager.Lives, new Vector2(12, 12 + LineHeight()));
                    DrawText("Best " + manager.HighScore, new Vector2(12, 12 + LineHeight() * 2));

                    if (ratingTimer > 0)
                    {
                        Vector2 position = new Vector2((GraphicsDevice.Viewport.Width - ratingTexture.Width * 4) / 2, 150);
                        spriteBatch.Draw(ratingTexture, position, null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                    }
                    break;
                case GameState.Won:
                    DrawCenteredLines(new string[] {
                        "YOU WIN!",
                        "",
                        "Score " + manager.Score,
                        "Best " + manager.HighScore,
                        manager.NewHighScore ? "NEW BEST!" : "",
                        "",
                        "Press SPACE to play again"
                    }, 120);
                    break;
                case GameState.Lost:
                    DrawCenteredLines(new string[] {
                        "GAME OVER",
                        "",
                        "You cleared " + manager.Score + " waves",
                        "Best " + manager.HighScore,
                        "",
                        "Press SPACE to try again"
                    }, 120);
                    break;
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        // ----------- Here are the drawing helpers that Claude wrote ---------------
        private float LineHeight()
        {
            return spriteFont.LineSpacing * TextScale + 6;
        }
        // Draws text with a dark shadow so it's readable over the starry background
        private void DrawText(string text, Vector2 position)
        {
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(2, 2), Color.Black,
                0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, text, position, Color.White,
                0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
        }
        // Draws each line centered left-to-right, one under the other
        private void DrawCenteredLines(string[] lines, float startY)
        {
            float y = startY;
            foreach (string line in lines)
            {
                float width = spriteFont.MeasureString(line).X * TextScale;
                DrawText(line, new Vector2((GraphicsDevice.Viewport.Width - width) / 2, y));
                y += LineHeight();
            }
        }
    }
}
