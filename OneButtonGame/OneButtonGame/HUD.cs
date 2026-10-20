using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        Texture2D purpleFrame; // title + end screens
        Texture2D pinkPanel; // playing HUD
        public Vector2 FrameScale = new Vector2(3f, 3f); // purple frame: 3x wide, 3x tall
        public Vector2 PanelScale = new Vector2(1.5f, 0.8f); // pink panel: wider than tall, to fit 3 short lines
        public int PanelTop = 8; // how far down from the top the pink panel sits

        public float TextScale = 2f; // easy to scale the font if too big or small :)

        int lastScore;
        float ratingTimer = 0f;

        public HUD(Game game, GameManager manager) : base(game) 
        { 
            this.manager = manager;
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.spriteFont = Game.Content.Load<SpriteFont>("Pixelate-Regular");
            spriteFont.DefaultCharacter = ' '; // Needed this to prevent from crashing, claude found this error, the font I downloaded has no : or -
            purpleFrame = Game.Content.Load<Texture2D>("purpleFrame");
            pinkPanel = Game.Content.Load<Texture2D>("pinkTallPanel_normal");
            ratingTexture = Game.Content.Load<Texture2D>("great");

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

            DrawHUDFrame();

            switch (manager.State)
            {
                // I had Claude help me with the text formating methods because HEADACHE
                case GameState.Title:
                    DrawCenteredLines(new string[] {
                        "Press SPACE to jump",
                    }, 250);
                    break;
                case GameState.Playing:
                    DrawCenteredLines(new string[] {
                        "Waves " + manager.Score + "/" + manager.WavesToWin,
                        "Lives " + manager.Lives,
                        "Best " + manager.HighScore
                    }, PanelTop + 16);

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
                    DrawHUDFrame();
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

        // Draws the panel BEHIND the text for whichever screen we're on
        private void DrawHUDFrame()
        {
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            if (manager.State == GameState.Playing)
            {
                // Pink panel, centered left-to-right at the top of the screen
                float width = pinkPanel.Width * PanelScale.X;
                Vector2 position = new Vector2((screenWidth - width) / 2, PanelTop);
                spriteBatch.Draw(pinkPanel, position, null, Color.White, 0f, Vector2.Zero, PanelScale, SpriteEffects.None, 0f);
            }
            else
            {
                // Purple frame, centered in the middle of the screen
                float width = purpleFrame.Width * FrameScale.X;
                float height = purpleFrame.Height * FrameScale.Y;
                Vector2 position = new Vector2((screenWidth - width) / 2, (screenHeight - height) / 2);
                spriteBatch.Draw(purpleFrame, position, null, Color.White, 0f, Vector2.Zero, FrameScale, SpriteEffects.None, 0f);
            }
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
