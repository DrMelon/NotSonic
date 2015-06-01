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
            myTileInfo = new SonicTileInfo(HeightArrays.HEIGHT_ARRAY_EMPTY, HeightArrays.HEIGHT_ARRAY_EMPTY, 0.0f);


            // Tile Info Stuff

            // Tiletype is cross-linked to the height_tileset image.

            /*
             *  Tile Type Defs
             *  0 = Empty
             *  1 = Basic
             *  2 = Slope up 45
             *  3 = Slope down 45
             *  ======================== LOOP UP
             *  5 = Loop Up 85
             *  25 = Loop Up 70
             *  44 = Loop Up 45
             *  45 = Loop Up 60
             *  62 = Loop Up 10
             *  63 = Loop Up 20
             *  64 = Loop Up 30
             * ======================== LOOP DOWN
             *  82 = Loop Down 10
             *  102 = Loop Down 20
             *  122 = Loop Down 30
             *  123 = Loop Down 45
             *  143 = Loop Down 60
             *  144 = Loop Down 70
             *  145 = Loop Down 85
             * 
             * 
             * 
             * 
             * ======================== LOOP UP Y FLIP
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * ======================= LOOP DOWN Y FLIP
             * 
             */
            myType = tileType;
            if (myType <= (int)TileType.TILE_EMPTY)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_EMPTY;
                Image spriteImage = new Image(16, 16); //blank
                Graphic = spriteImage;
            }

            else if(myType == (int)TileType.TILE_BASIC)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_BASIC;
            }

            else if(myType == 2)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_SLOPE_45_UP;
            }

            else if (myType == 3)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_SLOPE_45_DOWN;
            }

    // LOOP UP
            else if (myType == 62)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_10;
            }
            else if (myType == 63)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_20;
            }
            else if (myType == 64)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_30;
            }
            else if (myType == 44)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_45;
            }
            else if (myType == 45)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_60;
            }
            else if (myType == 25)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_70;
            }
            else if (myType == 5)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_85;
            }

    // LOOP DOWN
            else if (myType == 82)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_DOWN_10;
            }
            else if (myType == 102)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_DOWN_20;
            }
            else if (myType == 122)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_DOWN_30;
            }
            else if (myType == 123)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_DOWN_45;
            }
            else if (myType == 143)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_DOWN_60;
            }
            else if (myType == 144)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_DOWN_70;
            }
            else if (myType == 145)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_DOWN_85;
            }
            else
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_EMPTY;
               // Image spriteImage = new Image(16, 16); //blank
               // Graphic = spriteImage;
            }



            
        }
    }
}
