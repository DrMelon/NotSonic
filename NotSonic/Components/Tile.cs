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
            TILE_SLOPE_45_UP,
            TILE_LOOP_UP_10,
            TILE_LOOP_UP_20,
            TILE_LOOP_UP_30,
            TILE_LOOP_UP_45,
            TILE_LOOP_UP_60,
            TILE_LOOP_UP_70,
            TILE_LOOP_UP_85,
            TILE_SLOPE_45_DOWN
        }

        public int myType;

        public Tile(float x, float y, int tileType)
        {
            // Otter stuff
            X = x;
            Y = y;

            tileImage = new ImageSet(Assets.TILE_SHEET, 16, 16);
            Graphic = tileImage;
            myTileInfo = new SonicTileInfo(HeightArrays.HEIGHT_ARRAY_EMPTY, HeightArrays.HEIGHT_ARRAY_EMPTY, 0.0f);


            // Tile Info Stuff
            myType = tileType;
            if (myType == (int)TileType.TILE_EMPTY)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_EMPTY;
                Image spriteImage = new Image(16, 16); //blank
                Graphic = spriteImage;
            }

            if(myType == (int)TileType.TILE_BASIC)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_BASIC;
            }

            // Tiletype is cross-linked to the height_tileset image.




            
        }
    }
}
