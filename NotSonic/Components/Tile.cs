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

            // Loop Up
            if (myType == TileType.TILE_LOOP_UP_10)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_10;
                tileImage.Frame = 333;
            }
            if (myType == TileType.TILE_LOOP_UP_20)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_20;
                tileImage.Frame = 334;
            }
            if (myType == TileType.TILE_LOOP_UP_30)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_30;
                tileImage.Frame = 335;
            }
            if (myType == TileType.TILE_LOOP_UP_45)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_45;
                tileImage.Frame = 315;
            }
            if (myType == TileType.TILE_LOOP_UP_60)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_60;
                tileImage.Frame = 316;
            }
            if (myType == TileType.TILE_LOOP_UP_70)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_70;
                tileImage.Frame = 296;
            }
            if (myType == TileType.TILE_LOOP_UP_85)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_LOOP_UP_85;
                tileImage.Frame = 276;
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
