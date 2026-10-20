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
        public Vector2 PanelScale = new Vector2(1.1f, 0.6f); // pink panel: wider than tall, to fit 3 short lines
        public int PanelLeft = 8; // the padding to the left of the playing panel
        public int PanelTop = 8; // how far down from the top the pink panel sits


        // Heart things:
        Texture2D heart;
        public float HeartScale = 2.5f; // scaling
        public int HeartSpacing = 6; // gap between hearts
        public int HeartTop = 12; // hearts' distance from the top

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
            heart = Game.Content.Load<Texture2D>("heart");

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
            spriteBatch.Begin(samplerState: SamplerState.PointClamp); // keeps the scaled-up pixel art crisp instead of blurry (this is Claude recomendation)

            DrawHUDFrame();

            switch (manager.State)
            {
                // I had Claude help me with the text formating methods because HEADACHE
                case GameState.Title:
                    DrawCenteredLines(new string[] {
                        "Press SPACE to jump",
                        "Hold for a higher jump",
                        "3 hits and you lose",
                        "",
                        "Press SPACE to start"
                    }, 180);
                    break;
                case GameState.Playing:
                    // Waves + Best, centered inside the pink panel in the top-left
                    float panelCenterX = PanelLeft + pinkPanel.Width * PanelScale.X / 2;
                    DrawCenteredLines(new string[] {
                        "Waves " + manager.Score,
                        "Best " + manager.HighScore
                    }, PanelTop + 8, panelCenterX);

                    DrawHearts();   // health, top middle

                    if (ratingTimer > 0)
                    {
                        Vector2 position = new Vector2((GraphicsDevice.Viewport.Width - ratingTexture.Width * 4) / 2, 150);
                        spriteBatch.Draw(ratingTexture, position, null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                    }
                    break;
                case GameState.Lost:
                    DrawCenteredLines(new string[] {
                        "GAME OVER",
                        "",
                        "You cleared " + manager.Score + " Waves",
                        "Best " + manager.HighScore,
                        manager.NewHighScore ? "NEW BEST!" : "",
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
                // Pink panel, top-left corner
                Vector2 position = new Vector2(PanelLeft, PanelTop);
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
            DrawCenteredLines(lines, startY, GraphicsDevice.Viewport.Width / 2f);
        }
        private void DrawCenteredLines(string[] lines, float startY, float centerX)
        {
            float y = startY;
            foreach (string line in lines)
            {
                float width = spriteFont.MeasureString(line).X * TextScale;
                DrawText(line, new Vector2(centerX - width / 2, y));
                y += LineHeight();
            }
        }
        // One heart per life, centered at the top of the screen.
        // Lost lives still draw, but faded, so the remaining hearts don't slide around.
        private void DrawHearts()
        {
            float heartWidth = heart.Width * HeartScale;
            float totalWidth = manager.MaxLives * heartWidth + (manager.MaxLives - 1) * HeartSpacing;
            float x = (GraphicsDevice.Viewport.Width - totalWidth) / 2;   // left edge of the first heart

            for (int i = 0; i < manager.MaxLives; i++)
            {
                Color color;
                if (i < manager.Lives)
                {
                    color = Color.White; // still have this life: full heart
                }
                else
                {
                    color = Color.White * 0.25f; // lost this life: faded heart
                }

                spriteBatch.Draw(heart, new Vector2(x, HeartTop), null, color, 0f, Vector2.Zero, HeartScale, SpriteEffects.None, 0f);
                x += heartWidth + HeartSpacing; // move right for the next heart
            }
        }
    }
}
