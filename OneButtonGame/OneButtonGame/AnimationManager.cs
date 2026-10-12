using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneButtonGame
{
    // just PLAYBACK
    // keeps track of which animation is playing and which frame it's on
    // any class can use mayhaps
    public class AnimationManager
    {
        // WE LOVE ENCAPSULATED PROPERTIES (wow properties are so cool why haven't I been using them more)
        public Animation CurrentAnimation {  get; private set; }
        
        int frameIndex; // which frame of the animation its on
        float frameTimer; // how long its been on that frame, for tracking stuff purposes

        public Texture2D CurrentFrame // the texutre that's showing right now 
        {
            get { return CurrentAnimation.Frames[frameIndex]; }
        }
        public Texture2D LastFrame // the prior texture in the animation, IDK if I'll need this I just added it because why not lol
        {
            get { return CurrentAnimation.Frames[frameIndex - 1]; }
        }

        // This bool confused me so I needed Claude's help writing
        // It returns true when a non-loopping () animation has reached its last frame, which now makes sense!
        public bool IsFinished
        {
            get { return !CurrentAnimation.Loops && frameIndex == CurrentAnimation.Frames.Length - 1; }
        }

        public AnimationManager() { } // Constructor, don't really need this tho because we're using all these getter setter properties yay!

        public void Play(Animation animation) // "plays" the animation that's passed in
        {
            CurrentAnimation = animation;
            frameIndex = 0;
            frameTimer = 0;
        }
        private void Restart()
        {
            if (CurrentAnimation.Loops) // if loop is set to true
                frameIndex = 0; // we loop back to the beginning!
            else
                frameIndex = CurrentAnimation.Frames.Length - 1; // if not, we just hold the last frame :)
        }

        public void Update(float time)
        {
            if (CurrentAnimation != null) // gotta actualy *have* a current animation for anything to happen!
            {
                frameTimer += time;
                
                // if the frameTimer is greater than the animations frame time...
                if (frameTimer >= CurrentAnimation.FrameTime) // ... it's time for the next frame!
                {
                    frameTimer -= CurrentAnimation.FrameTime; // reset the frame timer
                    frameIndex++;

                    if (frameIndex >= CurrentAnimation.Frames.Length) // now, if we're past the last frame...
                    {
                        Restart(); // restart the animation :)
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
