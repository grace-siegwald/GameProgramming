using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OneButtonGame
{
    // just DATA
    // which frames to show, what order to show them, and whether or not it should loop
    // the digital "content" of the animation
    // small ahh class
    public class Animation
    {
        // All of these are encapsulated properties because ECAPSULATION AND SUCH!!
        public Texture2D[] Frames {  get; private set; } // array of the actualy frame textures
        public float FrameTime { get; private set; } // number of seconds each frame stays on screen :)
        public bool Loops { get; private set; }
        
        public Animation(Texture2D[] frames, float frameTime, bool loops) 
        {
            Frames = frames;
            FrameTime = frameTime;
            Loops = loops;
        }
    }
}
