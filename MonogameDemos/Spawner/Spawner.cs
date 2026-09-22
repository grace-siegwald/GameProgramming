using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Spawner
{


    public interface ISpawner
    {
        GameComponent instance { get; set; }

        GameComponent Spawn();
    }


    abstract class Spawner :  GameComponent, ISpawner
    {
        InputHandler input;
        public GameComponent instance { get; set; }

        public bool CreateNewObectEverySpawn;
        protected Microsoft.Xna.Framework.Input.Keys spawnKey;

        protected Random r; 

        public GameComponent Instance
        {
            get {
                if (instance == null || this.CreateNewObectEverySpawn)
                {
                    instance = new GameComponent(this.Game);
                    instance.Initialize();
                }
                return instance;
            }  
            set {
                instance = value; }
        }
        
        public Spawner(Game game)
            :base(game)
        {
            input = (InputHandler)this.Game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                input = new InputHandler(this.Game);
                this.Game.Components.Add(input);
            }
            CreateNewObectEverySpawn = false;
            SetSpawnKey(Microsoft.Xna.Framework.Input.Keys.None);
            r = new Random();
        }

        protected void SetSpawnKey(Microsoft.Xna.Framework.Input.Keys key)
        {
            spawnKey = key;
        }

        public override void Update(GameTime gameTime)
        {
            if(input.KeyboardState.HasReleasedKey(spawnKey))
            {
                this.Spawn();
            }
            base.Update(gameTime);
        }

        public virtual GameComponent Spawn()
        {
            if (instance != null)
            {
                this.Game.Components.Add(instance);
            }
            return instance;
        }

        public Vector2 GetRandomDirection()
        {
            Vector2 v = new Vector2((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f);
            Vector2.Normalize(ref v, out v);    //Normalize
            return v;
        }

        public Vector2 GetRandLocation(Texture2D texture)
        {
            //System.Threading.Thread.Sleep(1);
            Vector2 loc;
            loc.X = r.Next(Game.Window.ClientBounds.Width - texture.Width) + texture.Width;
            loc.Y = r.Next(Game.Window.ClientBounds.Height - texture.Height) + texture.Height;
            return loc;
        }

    }
}
