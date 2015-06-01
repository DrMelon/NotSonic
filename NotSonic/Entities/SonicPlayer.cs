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
        
        public SonicPlayer(List<NotSonic.Components.Tile> tl, float x = 0, float y = 0)
        {
            // Set Spawn Loc
            X = x;
            Y = y;
            tileList = tl;


            // Create sprites
            spriteSheet = new Spritemap<string>(Assets.SONIC_SHEET, 32, 40);
            spriteSheet.Add("idle", new int[] { 0 }, new float[] { 6f });
            spriteSheet.Add("roll", new int[] { 1 }, new float[] { 6f });
            spriteSheet.Add("spindash", new int[] { 2 }, new float[] { 6f });
            spriteSheet.Play("idle");
            Graphic = spriteSheet;
            Graphic.CenterOrigin();

            // Create movement
            myMovement = new Components.SonicMovement();
            myMovement.TileList = tileList;
            AddComponent(myMovement);

            // Add to pausable objects group
            Group = Global.GROUP_ACTIVEOBJECTS;

        }

        public override void Update()
        {
            UpdateAnimations();
            base.Update();
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
                //Graphic.Angle = 0;
                Graphic.OriginY = 15;

            }
            else
            {
                spriteSheet.Play("idle");
            }

            if(myMovement.CurrentSpindashStrength > 0.0f)
            {
                spriteSheet.Play("spindash");
            }
        }

    }
}
