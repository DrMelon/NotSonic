using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace NotSonic.Entities
{
    class SonicPlayer : Entity
    {
        public Spritemap<string> spriteSheet;
        public NotSonic.Components.SonicMovement myMovement;
        public List<NotSonic.Components.Tile> tileList;
        public NotSonic.Components.ParticleSystem bubbleParticles;
        public NotSonic.Components.ParticleSystem brakeSmokeParticles;
        public NotSonic.Components.ParticleSystem speedTrailParticles;
        public NotSonic.System.SegaController myController;

        public float SmoothAngle;
        public bool flipReady = false;
        public bool GrossMode = false; //gross animation rotation mode

        // Chain destruction stuff
        public float comboTime;
        public int comboAmt;

        Sound comboResetSound = new Sound(Assets.SND_WARP);
        
        
        public SonicPlayer(NotSonic.System.SegaController ctrl, List<NotSonic.Components.Tile> tl, float x = 0, float y = 0)
        {
            // Set Spawn Loc
            X = x;
            Y = y;
            float CollisionWidth = 40;
            float CollisionHeight = 40;
            this.Collider = new BoxCollider((int)CollisionWidth, (int)CollisionHeight, new int[] { 0 });
            this.Collider.CenterOrigin();
            tileList = tl;
            myController = ctrl;

            // Create sprites
            spriteSheet = new Spritemap<string>(Assets.SONIC_SHEET, 40, 40);
            spriteSheet.Add("idle", new int[] { 0 }, new float[] { 6f });
            spriteSheet.Add("walk", new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }, new float[] { 6f });
            spriteSheet.Add("run", new int[] { 9, 10, 11, 12}, new float[] { 6f });
            spriteSheet.Add("roll", new int[] { 13, 14, 13, 15, 13, 16, 13, 17 }, new float[] { 12f });
            spriteSheet.Add("spindash", new int[] { 18, 19, 20, 21, 22, 23 }, new float[] { 6f });
            spriteSheet.Add("brake", new int[] { 24, 25, 26, 27 }, new float[] { 6f });
            spriteSheet.Add("freezers", new int[] { 28, 29, 30, 31, 32 }, new float[] { 600f });
            spriteSheet.Play("idle");
            Graphic = spriteSheet;
            this.Layer = 18;
            Graphic.CenterOrigin();

            // Create movement
            myMovement = new Components.SonicMovement();
            myMovement.TileList = tileList;
            myMovement.theController = myController;
            AddComponent(myMovement);

            // Add to pausable objects group
            Group = Global.GROUP_ACTIVEOBJECTS;


            

        }

        public override void Added()
        {
            base.Added();
            // Create particle systems
            speedTrailParticles = new Components.ParticleSystem(0, 0);
            speedTrailParticles.Initialize(0, 0, 0, 0, 1, 10, Assets.SONIC_SHEET, 40, 40, 1, true, 1, 1);
            speedTrailParticles.beginColour.A = 0.25f;
            speedTrailParticles.endColour.A = 0.1f;
            
            this.Scene.Add(speedTrailParticles);
            speedTrailParticles.Start();
            speedTrailParticles.Group = Global.GROUP_ACTIVEOBJECTS;
            speedTrailParticles.Visible = false;

        }

        private void UpdateSpeedTrails()
        {
            if((Math.Max(Math.Abs(myMovement.YSpeed), Math.Abs(myMovement.XSpeed)) >= 6))
            {
                speedTrailParticles.Visible = true;
                speedTrailParticles.Start();
            }
            else
            {
                speedTrailParticles.Visible = false;
                speedTrailParticles.Stop();
            }
            
            speedTrailParticles.X = X;
            speedTrailParticles.Y = Y;
            speedTrailParticles.Update();
            foreach (Particle p in speedTrailParticles.activeLocalParticles)
            {
                p.Group = Global.GROUP_ACTIVEOBJECTS;
                p.Angle = spriteSheet.Angle;
                p.FlipX = !myMovement.FacingRight;
                p.FrameOffset = spriteSheet.CurrentFrame;
                p.Visible = speedTrailParticles.Visible;
                p.Layer = this.Layer + 1;
            }
        }

        private void UpdateParticleSystems()
        {
            // Speed Trails
            UpdateSpeedTrails();


        }

        public override void Update()
        {
            base.Update();
            // Update combo timings
            if(comboTime > 0 && !myMovement.Rolling)
            {
                comboTime -= 5;
            }
            if(comboTime <= 0 && comboAmt > 0)
            {
                // Combo reset.
                // Pick combo reset noise.
                if(comboAmt > 2 && comboAmt < 5)
                {
                    comboResetSound = new Sound(Assets.SND_VO_EXCELLENT);
                    comboResetSound.Play();
                }
                if (comboAmt >= 5 && comboAmt < 10)
                {
                    comboResetSound = new Sound(Assets.SND_VO_IMPRESSIVE);
                    comboResetSound.Play();
                }
                if (comboAmt >= 10)
                {
                    comboResetSound = new Sound(Assets.SND_VO_BLUESTREAK);
                    comboResetSound.Play();
                }
                
                comboTime = 0;
                comboAmt = 0;

            }


        }

        public override void UpdateLast()
        {
            UpdateAnimations();
            UpdateParticleSystems();


            base.UpdateLast();
        }

        public void UpdateAnimations()
        {
            Graphic.CenterOrigin();
            
            spriteSheet.FlippedX = !myMovement.FacingRight;
            spriteSheet.FlippedY = !myMovement.FacingUp;

            //myMovement.CurrentFloorMode = NotSonic.Components.SonicMovement.FloorMode.LEFTWALL;

            // Rotations
            if (myMovement.CurrentFloorMode == NotSonic.Components.SonicMovement.FloorMode.FLOOR)
            {
                Graphic.Angle = 0;   
            }
            if(myMovement.CurrentFloorMode == NotSonic.Components.SonicMovement.FloorMode.RIGHTWALL)
            {
                Graphic.Angle = 90;
            }
            if (myMovement.CurrentFloorMode == NotSonic.Components.SonicMovement.FloorMode.CEILING)
            {
                Graphic.Angle = 180;
            }
            if (myMovement.CurrentFloorMode == NotSonic.Components.SonicMovement.FloorMode.LEFTWALL)
            {
                Graphic.Angle = 270;
            }

            if(GrossMode)
            {
                Graphic.Angle = myMovement.Angle;
            }

            if (myMovement.Rolling)
            {
                spriteSheet.Play("roll");
                for (int i = 0; i < 8; i++)
                {
                    spriteSheet.Anims["roll"].FrameDelays[i] = Math.Max(5 - Math.Abs(myMovement.GroundSpeed), 1);

                }

                Graphic.Angle = 0;
                //Graphic.OriginY = 15;

            }
            // Spindashing?
            else if (myMovement.CurrentSpindashStrength > 0.0f)
            {
                spriteSheet.Play("spindash");
                for (int i = 0; i < 6; i++)
                {
                    spriteSheet.Anims["spindash"].FrameDelays[i] = Math.Max(4 - Math.Abs(myMovement.CurrentSpindashStrength), 1);
                }
            }

            // Screeching to a halt
            else if (myMovement.Braking)
            {
                spriteSheet.Play("brake");
                if (flipReady)
                {
                    myMovement.Braking = false;
                    flipReady = false;
                }
                if (spriteSheet.Anims["brake"].CurrentFrameIndex == spriteSheet.Anims["brake"].FrameCount - 1)
                {
                    flipReady = true;
                }


            }
            else if (Math.Abs(myMovement.GroundSpeed) < 0.01 && Math.Abs(myMovement.YSpeed) < 0.3)
            {
                spriteSheet.Play("idle");
                
            }
            else if (Math.Abs(myMovement.GroundSpeed) < 6)
            {

                //walking 
                spriteSheet.Play("walk");
                for (int i = 0; i < 8; i++)
                {
                    spriteSheet.Anims["walk"].FrameDelays[i] = Math.Max(8 - Math.Abs(myMovement.GroundSpeed), 1);

                }
            
            }
            else
            {
                // Running
                spriteSheet.Play("run");
                for (int i = 0; i < 4; i++)
                {
                    spriteSheet.Anims["run"].FrameDelays[i] = Math.Max(8 - Math.Abs(myMovement.GroundSpeed), 1);

                }
            }


        }

    }
}
