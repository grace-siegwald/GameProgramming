using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Content;
using StrategyPacMan.weapons;

namespace PacManWeaponsStrategy
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FoodManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<food> foods;
        Vector2 foodGrid = new Vector2(2, 2);
        int xOffset;
        int yOffset;
        Game g;
        MonoGamePacMan PacMan;
        

        SpriteBatch sb;
        
        public FoodManager(Game game, MonoGamePacMan p)
            : base(game)
        {
            
            g = game;
            PacMan = p;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            
            foods = new List<food>();
            Vector2 startLoc = new Vector2(10,10);

            xOffset = 150;
            yOffset = 150;

            for (int i = 0; i < foodGrid.X; i++)
            {
                for (int ii = 0; ii < foodGrid.Y; ii++)
                {
                    food f = new NoWeapon(g);
                    f.Initialize();
                    f.Location = new Vector2(startLoc.X + (xOffset * i), startLoc.Y + (yOffset * ii));
                    foods.Add(f);
                }
            }
            food r = new RedWeapon(g);
            r.Initialize();
            r.Location = new Vector2(200, 200);
            foods.Add(r);
            r = new TealWeapon(g);
            r.Initialize();
            r.Location = new Vector2(300, 200);
            foods.Add(r);
            sb = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            
            foreach  (var f in foods)
            {
                if (f.Enabled)
                {
                    f.Update(gameTime);
                    if (f.Intersects(PacMan))
                    {
                        f.Enabled = false;
                        f.Visible = false;
                        //if food is a weapon give it to pacman
                        if (f is IWeapon)
                        {
                            PacMan.GiveWeapon((foodWeapon)f);
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            foreach (food f in foods)
            {
                if (f.Visible)
                {
                    f.Draw(sb);
                }
            }
            sb.End();
            base.Draw(gameTime);
        }
    }
}