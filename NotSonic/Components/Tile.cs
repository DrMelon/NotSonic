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
        public Image spriteImage;

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




            // Tile Info Stuff
            myType = tileType;
            if (myType == TileType.TILE_EMPTY)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_EMPTY;


                spriteImage = new Image(16, 16); //blank
                Graphic = spriteImage;
            }

            if(myType == TileType.TILE_BASIC)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_BASIC;

                spriteImage = new Image(Assets.EXAMPLE_TILE);
                Graphic = spriteImage;
            }

            if (myType == TileType.TILE_SLOPE_45_UP)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_SLOPE_45_UP;

                ImageSet img = new ImageSet(Assets.TILE_SHEET, 16, 16);
                img.Frame = 26;
            
                spriteImage = new Image(Assets.EXAMPLE_TILE);
                Graphic = img;
            }

            if (myType == TileType.TILE_SLOPE_45_DOWN)
            {
                myTileInfo = SonicTileInfoDefaults.TILEINFO_SLOPE_45_DOWN;

                ImageSet img = new ImageSet(Assets.TILE_SHEET, 16, 16);
                img.Frame = 26;
                img.FlippedX = true;

                spriteImage = new Image(Assets.EXAMPLE_TILE);
                Graphic = img;
            }


            
        }
    }
}
