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

        public float SmoothAngle;
        
        public SonicPlayer(List<NotSonic.Components.Tile> tl, float x = 0, float y = 0)
        {
            // Set Spawn Loc
            X = x;
            Y = y;
            tileList = tl;


            // Create sprites
            spriteSheet = new Spritemap<string>(Assets.SONIC_SHEET, 40, 40);
            spriteSheet.Add("idle", new int[] { 0 }, new float[] { 6f });
            spriteSheet.Add("walk", new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }, new float[] { 6f });
            spriteSheet.Add("run", new int[] { 9, 10, 11, 12}, new float[] { 6f });
            spriteSheet.Add("roll", new int[] { 13, 14, 13, 15, 13, 16, 13, 17 }, new float[] { 12f });
            spriteSheet.Add("spindash", new int[] { 18 }, new float[] { 6f });
            spriteSheet.Play("idle");
            Graphic = spriteSheet;
            this.Layer = 18;
            Graphic.CenterOrigin();

            // Create movement
            myMovement = new Components.SonicMovement();
            myMovement.TileList = tileList;
            AddComponent(myMovement);

            // Add to pausable objects group
            Group = Global.GROUP_ACTIVEOBJECTS;

        }

        public override void UpdateLast()
        {
            UpdateAnimations();
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

            if(myMovement.Rolling)
            {
                spriteSheet.Play("roll");
                for (int i = 0; i < 8; i++)
                {
                    spriteSheet.Anims["roll"].FrameDelays[i] = Math.Max(5 - Math.Abs(myMovement.GroundSpeed), 1);

                }
                    
                Graphic.Angle = 0;
                //Graphic.OriginY = 15;

            }
            else if (Math.Abs(myMovement.GroundSpeed) < 0.01)
            {
                spriteSheet.Play("idle");
                
            }
            else if (Math.Abs(myMovement.GroundSpeed) < myMovement.TopSpeed)
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

            if(myMovement.CurrentSpindashStrength > 0.0f)
            {
                spriteSheet.Play("spindash");
            }
        }

    }
}
