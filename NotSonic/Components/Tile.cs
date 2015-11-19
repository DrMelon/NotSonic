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

        public bool FlipX;
        public bool FlipY;

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

            FlipX = flipX;
            FlipY = flipY;

            tileImage = new ImageSet(Assets.TILE_SHEET, 16, 16);
            Graphic = tileImage;
            myType = tileType;
            myTileInfo = new SonicTileInfo(myType, flipX, flipY);
            
        }
    }
}
