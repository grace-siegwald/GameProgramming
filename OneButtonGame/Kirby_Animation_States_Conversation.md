# Kirby Animation States — Conversation Excerpt

---

# Prompt 1

> I'd like to implement different play states to the Kirby class so that I can "animate" him to take different positions in his jumping and falling and riding and such. I think you have the sprite sheet I've been using, but here it is again just in case.
>
> Can you cut out the necessary sprites from the sheet, and help me implement this?

*[Attached image: the Kirby Amazing Mirror "Kirby Wave Ride" sprite sheet]*

---

# Response 1

*[Files sent with this response: `KirbyFrames.zip` and `kirby_states_preview.gif`]*

I cut 12 frames from the pink and red Kirby rows. **KirbyFrames.zip** holds them, and **kirby_states_preview.gif** shows each animation playing.

| State | Frames | Plays |
|---|---|---|
| Grounded | `ride0 → ride1 → ride2 → ride1` (a gentle bob) | Loops |
| Ascending | `rise0 → rise1 → rise2` (board tilts up) | Once, then holds the last frame |
| Falling | `fall0 → fall1 → fall2` (board tips nose-down) | Once, then holds the last frame |
| Hurt | `hurt0 ↔ hurt1` (the surprised "o" face with a splash, pink then red) | Loops; this is now the blink |

`land.png` (the crouch pose) is a spare for later.

All 12 frames are on the **same 32×31 canvas**, with the board lined up at the bottom. That matters because the library sets `Origin` once in `LoadContent`. If the frames were different sizes, Kirby would jump around a few pixels every time the frame changed, and his bottom edge wouldn't sit on the water the same way.

I skipped the library's `DrawableAnimatableSprite`. It re-copies the collision pixels every frame, ignores `DrawColor`, and its per-pixel check uses the whole sprite sheet's size instead of one frame's. It would break your wave hits.

## Step 1: Add the frames to Content

Unzip it so you have `Content/Kirby/ride0.png` and so on. In the MGCB Editor, right-click **Content → Add → Existing Folder** and pick the `Kirby` folder. The asset names will be `Kirby/ride0`, `Kirby/rise1`, etc. You can delete `kirbyPink` and `kirbyRed` from Content afterwards; nothing uses them anymore.

## Step 2: Replace Kirby.cs

Everything about physics, the floor and lives stays the same. What's new is the **Animation** section, and it replaces `UpdateFlash`, `defualtSprite` and `hurtSprite`. You'd already started the `KirbyState` enum; I added `Hurt` to it.

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;

namespace OneButtonGame
{
    public enum KirbyState
    {
        Grounded,   // riding on the water
        Ascending,  // going up after a jump
        Falling,    // coming back down
        Hurt        // just got hit by a wave
    }

    public class Kirby : DrawableSprite
    {
        WaterFloor water;

        // Lives
        public int Lives { get; private set; }
        public int StartLives = 3;
        public float HurtTime = 1f;      // how long he stays in the Hurt state
        private float hurtTimer = 0;     // seconds left in the Hurt state

        // Physics
        public float Gravity = 6f;
        public float JumpStrength = 5f;
        public bool OnFloor { get; private set; }

        // Animation
        public KirbyState State { get; private set; }
        Texture2D[] groundedFrames;
        Texture2D[] ascendingFrames;
        Texture2D[] fallingFrames;
        Texture2D[] hurtFrames;
        int frameIndex;                  // which frame of the current animation we're on
        float frameTimer;                // how long we've been on that frame
        public float FrameTime = 0.1f;   // seconds per frame (0.1 = 10 frames per second)

        public Kirby(Game game, WaterFloor water) : base(game)
        {
            this.water = water;
            Scale = 2f;
        }

        protected override void LoadContent()
        {
            // Each state's animation is just an array of textures, played in order
            groundedFrames  = new Texture2D[] { LoadFrame("ride0"), LoadFrame("ride1"), LoadFrame("ride2"), LoadFrame("ride1") };
            ascendingFrames = new Texture2D[] { LoadFrame("rise0"), LoadFrame("rise1"), LoadFrame("rise2") };
            fallingFrames   = new Texture2D[] { LoadFrame("fall0"), LoadFrame("fall1"), LoadFrame("fall2") };
            hurtFrames      = new Texture2D[] { LoadFrame("hurt0"), LoadFrame("hurt1") };

            SpriteTexture = groundedFrames[0];   // needs a texture BEFORE base.LoadContent sets the Origin
            base.LoadContent();

            Reset();
        }

        private Texture2D LoadFrame(string name)
        {
            return Game.Content.Load<Texture2D>("Kirby/" + name);
        }

        public void Reset()
        {
            Lives = StartLives;
            Location = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2);
            Direction = Vector2.Zero;
            Speed = 100;
            OnFloor = false;
            hurtTimer = 0;
            ChangeState(KirbyState.Falling);   // he starts in the air
        }

        public void Hit()
        {
            Lives--;
            hurtTimer = HurtTime;
        }

        public void Jump()
        {
            if (OnFloor)
            {
                Direction.Y = -JumpStrength;
                OnFloor = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!OnFloor)
            {
                Direction.Y += Gravity * time;
            }
            Location += Direction * Speed * time;

            base.Update(gameTime);

            UpdateFloorCollision();

            if (hurtTimer > 0)
            {
                hurtTimer -= time;
            }

            UpdateState();          // 1) figure out WHICH animation he should be in
            UpdateAnimation(time);  // 2) advance THAT animation's frames
        }

        // ---------------- State ----------------

        // Look at what Kirby is doing and pick the matching state
        private void UpdateState()
        {
            KirbyState newState;

            if (hurtTimer > 0)
                newState = KirbyState.Hurt;        // being hurt overrides everything else
            else if (OnFloor)
                newState = KirbyState.Grounded;
            else if (Direction.Y < 0)
                newState = KirbyState.Ascending;   // negative Y = moving UP the screen
            else
                newState = KirbyState.Falling;

            if (newState != State)
            {
                ChangeState(newState);
            }
        }

        private void ChangeState(KirbyState newState)
        {
            State = newState;
            frameIndex = 0;   // start the new animation from its first frame
            frameTimer = 0;
        }

        // Which array of frames goes with which state
        private Texture2D[] GetFrames(KirbyState state)
        {
            switch (state)
            {
                case KirbyState.Ascending: return ascendingFrames;
                case KirbyState.Falling:   return fallingFrames;
                case KirbyState.Hurt:      return hurtFrames;
                default:                   return groundedFrames;
            }
        }

        // ---------------- Animation ----------------

        private void UpdateAnimation(float time)
        {
            Texture2D[] frames = GetFrames(State);

            frameTimer += time;
            if (frameTimer >= FrameTime)   // time for the next frame
            {
                frameTimer -= FrameTime;
                frameIndex++;

                if (frameIndex >= frames.Length)   // past the last frame?
                {
                    if (State == KirbyState.Grounded || State == KirbyState.Hurt)
                        frameIndex = 0;                   // these LOOP
                    else
                        frameIndex = frames.Length - 1;   // jump/fall play ONCE and hold the last pose
                }
            }

            SetTexture(frames[frameIndex]);
        }

        // Only swap when the frame actually changes.
        // Capital-S SpriteTexture this time: the frames are DIFFERENT SHAPES now,
        // so the collision pixel data has to update along with the picture.
        private void SetTexture(Texture2D texture)
        {
            if (SpriteTexture != texture)
            {
                SpriteTexture = texture;
            }
        }

        // ---------------- Floor ----------------

        private void UpdateFloorCollision()
        {
            if (Direction.Y >= 0 && Intersects(water))
            {
                Rectangle overlap = Intersection(LocationRect, water.LocationRect);
                Location.Y -= overlap.Height - water.SinkDepth;

                Direction.Y = 0;
                OnFloor = true;
            }
        }
    }
}
```

## How it works

This is a **state machine**, and it splits the work into two separate jobs:

1. **`UpdateState()` answers "what is Kirby doing?"** It only looks at facts Kirby already has: is `hurtTimer` running, is he `OnFloor`, and which way `Direction.Y` points. No other class has to tell Kirby what pose to be in.
2. **`UpdateAnimation()` answers "which frame of that animation?"** It's the same timer pattern as your old `flashTimer`: add up time, and when it passes `FrameTime`, move to the next frame.

`ChangeState()` runs only when the state actually changes, and it rewinds to frame 0. That's why the jump animation starts from the beginning on every jump instead of picking up wherever it left off.

There's also a bonus for collision. The `SetTexture` swap uses capital-S `SpriteTexture`, so the per-pixel wave check now uses Kirby's **current pose**. When his board tilts up during a jump, the collision shape tilts with it. Earlier, lowercase was right because pink and red were the same shape; now the frames differ, so the collision data has to update too. It's only about 10 tiny swaps a second, so it's cheap.

## Check that it works

1. **On the title screen,** Kirby falls with his board nose-down (Falling), then bobs gently once he lands (Grounded).
2. **Jump:** the board tilts up while he rises and tips down past the top of the arc. He should land back in the same spot, not a few pixels higher or lower. If he's off, the frames aren't on the same canvas, so check that you're using the files from the zip.
3. **Get hit:** for about 1 second he blinks between the pink and red surprised faces, even mid-air. Then he goes back to the right pose.
4. **Replay** after a loss: he should start fresh in the Falling pose, not stuck on Hurt.

If you want more polish later, `land.png` can become a short **Landing** state. When Kirby goes from Falling to Grounded, show `land` for about 0.15 s before the ride bob starts. It uses the same timer pattern as `hurtTimer`.

---

# Prompt 2

> I'm loving this idea already, but what if I did it what an AnimationManager class that would add a bit more seperation of concerns?

---

# Response 2

Splitting it out is a good call. The version I gave you had Kirby doing three jobs: deciding his state, storing the animations, and stepping through frames. Pulling the last two out leaves Kirby with only the part that's actually about Kirby.

## How it splits up

| Class | Job | Knows about Kirby? |
|---|---|---|
| `Animation` | Data only: the frames, how fast, and whether it loops | No |
| `AnimationManager` | Playback: which animation is playing, which frame it's on, and the timer | No |
| `Kirby` | Decides what state he's in, tells the manager what to play, and shows the current frame | Yes |

Since `Animation` and `AnimationManager` know nothing about Kirby, the wave or anything else could reuse them.

There's also a nice side effect: each animation gets **its own speed**. The ride bob can be slow and lazy, while the hurt blink stays quick.

**Why `AnimationManager` is a plain class, not a GameComponent:** it belongs to Kirby, and Kirby has to swap his texture right *after* it updates. As a component, you'd have to add it to Game1 and make sure it updates in the right order, with no benefit. Your criteria say "game components *where appropriate*," and this is a good example to point to of deciding when one *isn't* appropriate.

The library already has classes called `SpriteAnimation` and `CelAnimationManager`, so I picked different names to avoid mixing them up.

## Animation.cs

```csharp
using Microsoft.Xna.Framework.Graphics;

namespace OneButtonGame
{
    // Just DATA: which frames to show, in what order, how fast, and whether to loop.
    // It doesn't play anything by itself; AnimationManager does that.
    public class Animation
    {
        public Texture2D[] Frames { get; private set; }
        public float FrameTime { get; private set; }   // seconds each frame stays on screen
        public bool Loops { get; private set; }        // true = start over at the end, false = hold the last frame

        public Animation(Texture2D[] frames, float frameTime, bool loops)
        {
            Frames = frames;
            FrameTime = frameTime;
            Loops = loops;
        }
    }
}
```

## AnimationManager.cs

```csharp
using Microsoft.Xna.Framework.Graphics;

namespace OneButtonGame
{
    // PLAYBACK: keeps track of which animation is playing and which frame it's on.
    // It doesn't know anything about Kirby, so any sprite could use one.
    public class AnimationManager
    {
        public Animation CurrentAnimation { get; private set; }

        private int frameIndex;     // which frame of the current animation we're on
        private float frameTimer;   // how long we've been on that frame

        // The texture that should be on screen right now
        public Texture2D CurrentFrame
        {
            get { return CurrentAnimation.Frames[frameIndex]; }
        }

        // True when a non-looping animation has reached its last frame
        // (handy if you add the Landing state later)
        public bool IsFinished
        {
            get { return !CurrentAnimation.Loops && frameIndex == CurrentAnimation.Frames.Length - 1; }
        }

        // Start an animation from its first frame
        public void Play(Animation animation)
        {
            CurrentAnimation = animation;
            frameIndex = 0;
            frameTimer = 0;
        }

        public void Update(float time)
        {
            if (CurrentAnimation == null) { return; }

            frameTimer += time;
            if (frameTimer >= CurrentAnimation.FrameTime)   // time for the next frame
            {
                frameTimer -= CurrentAnimation.FrameTime;
                frameIndex++;

                if (frameIndex >= CurrentAnimation.Frames.Length)   // past the last frame?
                {
                    if (CurrentAnimation.Loops)
                        frameIndex = 0;                                  // start over
                    else
                        frameIndex = CurrentAnimation.Frames.Length - 1; // hold the last pose
                }
            }
        }
    }
}
```

## Kirby.cs (full file)

This is a full file, so it works whether or not you'd already pasted in my last version. The frame timer, `frameIndex` and `GetFrames` are gone. In their place, Kirby has four `Animation` objects and one `animator`.

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;

namespace OneButtonGame
{
    public enum KirbyState
    {
        Grounded,   // riding on the water
        Ascending,  // going up after a jump
        Falling,    // coming back down
        Hurt        // just got hit by a wave
    }

    public class Kirby : DrawableSprite
    {
        WaterFloor water;

        // Lives
        public int Lives { get; private set; }
        public int StartLives = 3;
        public float HurtTime = 1f;      // how long he stays in the Hurt state
        private float hurtTimer = 0;     // seconds left in the Hurt state

        // Physics
        public float Gravity = 6f;
        public float JumpStrength = 5f;
        public bool OnFloor { get; private set; }

        // Animation: Kirby decides WHAT to play, the animator handles HOW
        public KirbyState State { get; private set; }
        AnimationManager animator = new AnimationManager();
        Animation groundedAnimation;
        Animation ascendingAnimation;
        Animation fallingAnimation;
        Animation hurtAnimation;

        public Kirby(Game game, WaterFloor water) : base(game)
        {
            this.water = water;
            Scale = 2f;
        }

        protected override void LoadContent()
        {
            // new Animation(frames, seconds per frame, loops?)
            groundedAnimation = new Animation(new Texture2D[] {
                LoadFrame("ride0"), LoadFrame("ride1"), LoadFrame("ride2"), LoadFrame("ride1") }, 0.15f, true);
            ascendingAnimation = new Animation(new Texture2D[] {
                LoadFrame("rise0"), LoadFrame("rise1"), LoadFrame("rise2") }, 0.08f, false);
            fallingAnimation = new Animation(new Texture2D[] {
                LoadFrame("fall0"), LoadFrame("fall1"), LoadFrame("fall2") }, 0.08f, false);
            hurtAnimation = new Animation(new Texture2D[] {
                LoadFrame("hurt0"), LoadFrame("hurt1") }, 0.1f, true);

            SpriteTexture = groundedAnimation.Frames[0];   // needs a texture BEFORE base.LoadContent sets the Origin
            base.LoadContent();

            Reset();
        }

        private Texture2D LoadFrame(string name)
        {
            return Game.Content.Load<Texture2D>("Kirby/" + name);
        }

        public void Reset()
        {
            Lives = StartLives;
            Location = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2);
            Direction = Vector2.Zero;
            Speed = 100;
            OnFloor = false;
            hurtTimer = 0;
            ChangeState(KirbyState.Falling);   // he starts in the air
        }

        public void Hit()
        {
            Lives--;
            hurtTimer = HurtTime;
        }

        public void Jump()
        {
            if (OnFloor)
            {
                Direction.Y = -JumpStrength;
                OnFloor = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!OnFloor)
            {
                Direction.Y += Gravity * time;
            }
            Location += Direction * Speed * time;

            base.Update(gameTime);

            UpdateFloorCollision();

            if (hurtTimer > 0)
            {
                hurtTimer -= time;
            }

            UpdateState();                       // 1) Kirby decides which state he's in
            animator.Update(time);               // 2) the animator moves to the next frame
            SetTexture(animator.CurrentFrame);   // 3) show that frame
        }

        // ---------------- State ----------------

        // Look at what Kirby is doing and pick the matching state
        private void UpdateState()
        {
            KirbyState newState;

            if (hurtTimer > 0)
                newState = KirbyState.Hurt;        // being hurt overrides everything else
            else if (OnFloor)
                newState = KirbyState.Grounded;
            else if (Direction.Y < 0)
                newState = KirbyState.Ascending;   // negative Y = moving UP the screen
            else
                newState = KirbyState.Falling;

            if (newState != State)   // only restart the animation when the state actually CHANGES
            {
                ChangeState(newState);
            }
        }

        private void ChangeState(KirbyState newState)
        {
            State = newState;
            animator.Play(GetAnimation(newState));
        }

        // Which animation goes with which state
        private Animation GetAnimation(KirbyState state)
        {
            switch (state)
            {
                case KirbyState.Ascending: return ascendingAnimation;
                case KirbyState.Falling:   return fallingAnimation;
                case KirbyState.Hurt:      return hurtAnimation;
                default:                   return groundedAnimation;
            }
        }

        // Only swap when the frame actually changes.
        // Capital-S SpriteTexture: the frames are DIFFERENT SHAPES,
        // so the collision pixel data has to update along with the picture.
        private void SetTexture(Texture2D texture)
        {
            if (SpriteTexture != texture)
            {
                SpriteTexture = texture;
            }
        }

        // ---------------- Floor ----------------

        private void UpdateFloorCollision()
        {
            if (Direction.Y >= 0 && Intersects(water))
            {
                Rectangle overlap = Intersection(LocationRect, water.LocationRect);
                Location.Y -= overlap.Height - water.SinkDepth;

                Direction.Y = 0;
                OnFloor = true;
            }
        }
    }
}
```

The `if (newState != State)` check in `UpdateState` matters. `Play()` always rewinds to frame 0, so if Kirby called it every frame, the animation would be stuck on its first frame forever. Kirby only calls it when his state changes.

## Check that it works

The checklist is the same as before, plus one new thing to look for:

1. **Title screen:** he falls nose-down, then bobs once he lands. The bob should look **slower** than before (0.15 s per frame instead of 0.1).
2. **Jump:** the board tilts up, then tips down past the top of the arc, and he lands at the same height every time.
3. **Hit:** he blinks between the pink and red surprised faces for about 1 second, then goes back to the right pose.
4. **Replay** from a loss: he starts fresh in the Falling pose.

If you add the Landing state later, `animator.IsFinished` tells you when `land` has finished playing, so you know when to switch to Grounded.
