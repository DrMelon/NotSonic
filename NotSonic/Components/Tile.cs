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
        // Otter image etc
        public ImageSet tileImage;

        public SonicTileInfo myTileInfo;

        public enum TileType
        {
            TILE_EMPTY,
            TILE_BASIC,
        }

        public int myType;

        public Tile(float x, float y, int tileType, bool flipX, bool flipY)
        {
            // Otter stuff
            X = x;
            Y = y;

            tileImage = new ImageSet(Assets.TILE_SHEET, 16, 16);
            Graphic = tileImage;
            myType = tileType;
            myTileInfo = new SonicTileInfo(myType, flipX, flipY);



/*



            if (myType <= (int)TileType.TILE_EMPTY)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_EMPTY;
            }

            else if(myType == (int)TileType.TILE_BASIC)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_BASIC;
                // TEST: load solidity from heightmap
                myTileInfo.flatheightArray = HeightArrays.HeightArraysList.ElementAt(1);
            }

            else if(myType == 2)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_SLOPE_45_UP;
            }

            else if (myType == 3)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_SLOPE_45_DOWN;
            }

    // LOOP SEGMENTS. Check for flipping here, re-set angles?
            else if (myType == 62)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_10;
                if(flipX && !flipY) // Up To Leftwall
                {
                    float newAngle = 360 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_85;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_85;
                    myTileInfo.Angle = newAngle;
                }
                if(flipY && !flipX) // Rightwall to Ceiling
                {
                    float newAngle = 180 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_UP_10;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_DOWN_10;
                    myTileInfo.Angle = newAngle;
                }
                if(flipX && flipY) // Leftwall to Ceiling
                {
                    float newAngle = 180 + myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_85;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_85;
                    myTileInfo.Angle = newAngle;
                }
            }
            else if (myType == 63)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_20;
                if (flipX && !flipY) // Up To Leftwall
                {
                    float newAngle = 360 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_70;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_70;
                    myTileInfo.Angle = newAngle;
                }
                if (flipY && !flipX)  // Rightwall to Ceiling
                {
                    float newAngle = 180 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_UP_20;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_DOWN_20;
                    myTileInfo.Angle = newAngle;
                }
                if (flipX && flipY) // Leftwall to Ceiling
                {
                    float newAngle = 180 + myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_70;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_70;
                    myTileInfo.Angle = newAngle;
                }
            }
            else if (myType == 64)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_30;
                if (flipX && !flipY) // Up To Leftwall
                {
                    float newAngle = 360 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_60;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_60;
                    myTileInfo.Angle = newAngle;
                }
                if (flipY && !flipX)  // Rightwall to Ceiling
                {
                    float newAngle = 180 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_UP_30;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_DOWN_30;
                    myTileInfo.Angle = newAngle;
                }
                if (flipX && flipY) // Leftwall to Ceiling
                {
                    float newAngle = 180 + myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_60;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_60;
                    myTileInfo.Angle = newAngle;
                }
            }
            else if (myType == 44)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_45;
                if (flipX && !flipY) // Up To Leftwall
                {
                    float newAngle = 360 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_45;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_45;
                    myTileInfo.Angle = newAngle;
                }
                if (flipY && !flipX)  // Rightwall to Ceiling
                {
                    float newAngle = 180 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_UP_45;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_DOWN_45;
                    myTileInfo.Angle = newAngle;
                }
                if (flipX && flipY) // Leftwall to Ceiling
                {
                    float newAngle = 180 + myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_45;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_45;
                    myTileInfo.Angle = newAngle;
                }
            }
            else if (myType == 45)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_60;
                if (flipX && !flipY) // Up To Leftwall
                {
                    float newAngle = 360 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_30;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_30;
                    myTileInfo.Angle = newAngle;
                }
                if (flipY && !flipX)  // Rightwall to Ceiling
                {
                    float newAngle = 180 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_UP_60;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_DOWN_60;
                    myTileInfo.Angle = newAngle;
                }
                if (flipX && flipY) // Leftwall to Ceiling
                {
                    float newAngle = 180 + myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_30;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_30;
                    myTileInfo.Angle = newAngle;
                }
            }
            else if (myType == 25)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_70;
                if (flipX && !flipY) // Up To Leftwall
                {
                    float newAngle = 360 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_20;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_20;
                    myTileInfo.Angle = newAngle;
                }
                if (flipY && !flipX)  // Rightwall to Ceiling
                {
                    float newAngle = 180 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_UP_70;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_DOWN_70;
                    myTileInfo.Angle = newAngle;
                }
                if (flipX && flipY) // Leftwall to Ceiling
                {
                    float newAngle = 180 + myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_20;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_20;
                    myTileInfo.Angle = newAngle;
                }
            }
            else if (myType == 5)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_85;
                if (flipX && !flipY) // Up To Leftwall
                {
                    float newAngle = 360 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_10;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_10;
                    myTileInfo.Angle = newAngle;
                }
                if (flipY && !flipX)  // Rightwall to Ceiling
                {
                    float newAngle = 180 - myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_UP_85;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_DOWN_85;
                    myTileInfo.Angle = newAngle;
                }
                if (flipX && flipY) // Leftwall to Ceiling
                {
                    float newAngle = 180 + myTileInfo.Angle;
                    myTileInfo = new SonicTileInfo(null, null, 0.0f);
                    myTileInfo.flatheightArray = HeightArrays.HEIGHT_LOOP_DOWN_10;
                    myTileInfo.wallheightArray = HeightArrays.HEIGHT_LOOP_UP_10;
                    myTileInfo.Angle = newAngle;
                }
            }

            else
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_EMPTY;
               // Image spriteImage = new Image(16, 16); //blank
               // Graphic = spriteImage;
            }


*/
            
        }
    }
}
