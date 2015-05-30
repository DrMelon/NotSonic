using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [NotSonic] Project.
// Date: 2015/05/30
//----------------
// Purpose: This represents a solid tile.
// Tiles have 2 height arrays, and an angle.
// They are always 16x16.

namespace NotSonic.Components
{
    class Tile : Entity
    {
        public int[] flatheightArray = new int[16];
        public int[] wallheightArray = new int[16];

        // Top left corner of tile
        public int XPos;
        public int YPos;

        // Tile angle
        public float Angle;

        // Otter image etc
        public Image spriteImage;

        public Tile(float x, float y, float ang, int[] fha)
        {
            X = x;
            Y = y;
            Angle = ang;
            flatheightArray = fha;
            

            spriteImage = new Image(Assets.EXAMPLE_TILE);
            XPos = (int)X;
            YPos = (int)Y;

            

            Graphic = spriteImage;
            
        }
    }
}
