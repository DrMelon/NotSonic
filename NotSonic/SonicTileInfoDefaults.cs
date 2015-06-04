using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace NotSonic
{
    class SonicTileInfoDefaults
    {
        public static SonicTileInfo TILEINFO_BASIC = new SonicTileInfo(HeightArrays.HEIGHT_ARRAY_FULL, HeightArrays.HEIGHT_ARRAY_FULL, 0.0f);
        public static SonicTileInfo TILEINFO_EMPTY = new SonicTileInfo(HeightArrays.HEIGHT_ARRAY_EMPTY, HeightArrays.HEIGHT_ARRAY_EMPTY, 0.0f);
        public static SonicTileInfo TILEINFO_SLOPE_45_UP = new SonicTileInfo(HeightArrays.HEIGHT_SLP_45_UP, HeightArrays.HEIGHT_ARRAY_FULL, 45.0f);
        public static SonicTileInfo TILEINFO_SLOPE_45_DOWN = new SonicTileInfo(HeightArrays.HEIGHT_SLP_45_UP, HeightArrays.HEIGHT_ARRAY_FULL, 360 - 45.0f);

        // Loop Up
        public static SonicTileInfo TILEINFO_LOOP_UP_10 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_10, HeightArrays.HEIGHT_LOOP_UP_85, 10);
        public static SonicTileInfo TILEINFO_LOOP_UP_20 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_20, HeightArrays.HEIGHT_LOOP_UP_70, 20);
        public static SonicTileInfo TILEINFO_LOOP_UP_30 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_30, HeightArrays.HEIGHT_LOOP_UP_60, 30);
        public static SonicTileInfo TILEINFO_LOOP_UP_45 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_45, HeightArrays.HEIGHT_LOOP_UP_45, 45);
        public static SonicTileInfo TILEINFO_LOOP_UP_60 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_60, HeightArrays.HEIGHT_LOOP_UP_30, 60);
        public static SonicTileInfo TILEINFO_LOOP_UP_70 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_70, HeightArrays.HEIGHT_LOOP_UP_20, 70);
        public static SonicTileInfo TILEINFO_LOOP_UP_85 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_85, HeightArrays.HEIGHT_LOOP_UP_10, 85);

        // LOOP CORNER ROTATIONS NEED TO BE CALCULATED, WHICH CAN BE DONE IN TILE.CS.

        void SetFlippedVersions()
        {
            // 45-Degree Slope Down
            TILEINFO_SLOPE_45_DOWN.flatheightArray = (int[])TILEINFO_SLOPE_45_UP.flatheightArray.Reverse();

            // Loop Bottom Right Corner - The Wall-Mode Sizes are flipped & reversed.
            TILEINFO_LOOP_UP_10.wallheightArray = (int[])TILEINFO_LOOP_UP_10.wallheightArray.Reverse();
            TILEINFO_LOOP_UP_20.wallheightArray = (int[])TILEINFO_LOOP_UP_20.wallheightArray.Reverse();
            TILEINFO_LOOP_UP_30.wallheightArray = (int[])TILEINFO_LOOP_UP_30.wallheightArray.Reverse();
            TILEINFO_LOOP_UP_45.wallheightArray = (int[])TILEINFO_LOOP_UP_45.wallheightArray.Reverse();
            TILEINFO_LOOP_UP_60.wallheightArray = (int[])TILEINFO_LOOP_UP_60.wallheightArray.Reverse();
            TILEINFO_LOOP_UP_70.wallheightArray = (int[])TILEINFO_LOOP_UP_70.wallheightArray.Reverse();
            TILEINFO_LOOP_UP_85.wallheightArray = (int[])TILEINFO_LOOP_UP_85.wallheightArray.Reverse();

        }

    }
}
