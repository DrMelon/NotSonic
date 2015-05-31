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
            TILE_SLOPE_45_DOWN
        }

        public TileType myType;

        public Tile(float x, float y, TileType tileType)
        {
            // Otter stuff
            X = x;
            Y = y;

            tileImage = new ImageSet(Assets.TILE_SHEET, 16, 16);
            Graphic = tileImage;


            // Tile Info Stuff
            myType = tileType;
            if (myType == TileType.TILE_EMPTY)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_EMPTY;


                Image spriteImage = new Image(16, 16); //blank
                Graphic = spriteImage;
            }

            if(myType == TileType.TILE_BASIC)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_BASIC;
                tileImage.Frame = 41;
            }

            if (myType == TileType.TILE_SLOPE_45_UP)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_SLOPE_45_UP;
                tileImage.Frame = 26;
            }

            if (myType == TileType.TILE_SLOPE_45_DOWN)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_SLOPE_45_DOWN;
                tileImage.Frame = 26;
                tileImage.FlippedX = true;        
                
            }


            
        }
    }
}
