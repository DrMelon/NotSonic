﻿using System;
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
            spriteSheet.Play("idle");
            Graphic = spriteSheet;

            // Create movement
            myMovement = new Components.SonicMovement();
            AddComponent(myMovement);

            // Add to pausable objects group
            Group = Global.GROUP_ACTIVEOBJECTS;

        }

        public override void Update()
        {
            base.Update();
        }

    }
}
