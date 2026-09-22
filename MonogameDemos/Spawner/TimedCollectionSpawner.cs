using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spawner
{

    public enum SpawnType {  Random, Queue, RepeatingQueue }

    class TimedCollectionSpawner : TimedSpawner
    {
        public SpawnType spawnType { get; set; }

        int randIndex = 0;
        
        //For Random
        List<GameComponent> components;
        public List<GameComponent> Components
        {
            get { return this.components; }
            //set { this.components = value; }
        }

        //For Queue and RepeatingQueue
        Queue<GameComponent> queueComponents;
        Queue<GameComponent> currentQueueComponents;
        public Queue<GameComponent> QueueComponents
        {
            get { return queueComponents; }
            //set { queueComponents = value; }
        }

        public TimedCollectionSpawner(Game game) : this(game, SpawnType.Random) {} 

        public TimedCollectionSpawner(Game game, SpawnType spawnType) : this(game, spawnType, new List<GameComponent>(), new Queue<GameComponent>()) { }

        public TimedCollectionSpawner(Game game, SpawnType spawnType, List<GameComponent> list) : this(game, spawnType, list, new Queue<GameComponent>()) { }

        public TimedCollectionSpawner(Game game, SpawnType spawnType, Queue<GameComponent> queue) : this(game, spawnType, new List<GameComponent>(), queue) { }

        public TimedCollectionSpawner(Game game, SpawnType spawnType, List<GameComponent> list, Queue<GameComponent> queue) : base(game)
        {
            this.components = list;
            this.queueComponents = queue;
            this.spawnType = spawnType;
           
        }

        public void AddComponent(GameComponent component)
        {
            switch(this.spawnType)
            {
                case SpawnType.Random:
                        this.components.Add(component);
                    break;
                case SpawnType.Queue:
                case SpawnType.RepeatingQueue:
                    this.EnqueueComponent(component);
                    break;
            } 
        }

        public void EnqueueComponent(GameComponent component)
        {
            switch (this.spawnType)
            {
                case SpawnType.Random:
                    this.AddComponent(component);
                    break;
                case SpawnType.Queue:
                case SpawnType.RepeatingQueue:
                    this.queueComponents.Enqueue(component);
                    break;
            }
        }

        public override GameComponent Spawn()
        {
            GameComponent spawnComp = null;
            switch (this.spawnType)
            {
                case SpawnType.Random:
                    if (components.Count > 0)
                    {
                        randIndex = r.Next(components.Count);
                        spawnComp = components[randIndex];
                    }
                    break;
                case SpawnType.Queue:
                    //Copy Queue first time
                    if (currentQueueComponents == null)
                    {
                        currentQueueComponents = queueComponents;
                    }
                    spawnComp = currentQueueComponents.Dequeue();
                    break;
                case SpawnType.RepeatingQueue:
                    if (currentQueueComponents == null || currentQueueComponents.Count == 0)
                    {
                        currentQueueComponents = queueComponents;
                    }
                    spawnComp = currentQueueComponents.Dequeue();
                    break;
            }
            this.Game.Components.Add(spawnComp);
            return spawnComp;
        }
    }
}
