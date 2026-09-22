using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ShotManager 
{
    
    public interface IShotManager 
    {
        List<Shot> Shots { get; set; }
        Shot Shoot();
        Shot Shoot(Shot s);
        string ShotTexture { get; set; }
        
    }
    
    public class ShotManager : GameComponent, IShotManager
    {
        List<Shot> shots; //private instance Data Member
        public List<Shot> Shots
        {
            get
            {
                return shots;
            }
            set
            {
                shots = value;
            }
        }
        List<Shot> shotsToRemove { get; set; }  //list of shots to be removed on Update is a shot is not enabled
        public string ShotTexture { get; set; } //Texture for shot
        
        protected Shot s = null; //default reference to shot to be reused
        
        public ShotManager(Game game) : base(game)
        {
            this.shots = new List<Shot>();
            this.shotsToRemove = new List<Shot>();
            s = new Shot(game);
        }

        #region Shoot
        public virtual Shot Shoot() 
        {
            return Shoot(new Shot(this.Game));
        }

        public virtual Shot Shoot(Vector2 direction, float speed)
        {
            s.Direction = direction;
            s.Speed = speed;
            return this.Shoot(s);
        }    

        public virtual Shot Shoot(Shot shot)
        {
            s = shot;
            if (!String.IsNullOrEmpty(this.ShotTexture))
            {
                s.ShotTexture = this.ShotTexture;
            }
           
            s.Initialize();
            this.addShot(s);
            return s;
        }

        #endregion
        protected virtual void addShot(Shot s)
        {
            this.shots.Add(s);
        }

        protected virtual void removeShot(Shot s)
        {
            this.shots.Remove(s);
        }

        public override void Update(GameTime gameTime)
        {
            shotsToRemove.Clear(); //clear old shots to be removed

            //Update each shot in the Shots Collection
            foreach (var s in Shots)
            {
                if (s.Enabled)
                {
                    s.Update(gameTime); //Only update enabled shots
                }
                else //If the shot is not enabled 
                {
                    shotsToRemove.Add(s);
                }              
            }

            //Remove shots that are not enalbled anymore
            foreach(Shot s in shotsToRemove)
            {
                this.removeShot(s);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Shot managet is a Game Cpmponent not a Drawable Game Component somehting else must call draw and pass in a open spritebatch
        /// </summary>
        /// <param name="sb">open sprite batch for drawing</param>
        public void Draw(SpriteBatch sb)
        {
            //Draw each shot in the shots collection
            foreach (var s in Shots)
            {
                if(s.Visible) //Maybe someone will want invisible shots?
                    s.Draw(sb);
            }
        }
    }
}
