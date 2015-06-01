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
        public static SonicTileInfo TILEINFO_SLOPE_45_DOWN = new SonicTileInfo(HeightArrays.HEIGHT_SLP_45_DOWN, HeightArrays.HEIGHT_ARRAY_FULL, 360 - 45.0f);

        // Loop Up
        public static SonicTileInfo TILEINFO_LOOP_UP_10 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_10, HeightArrays.HEIGHT_LOOP_DOWN_10, 10);
        public static SonicTileInfo TILEINFO_LOOP_UP_20 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_20, HeightArrays.HEIGHT_LOOP_DOWN_20, 20);
        public static SonicTileInfo TILEINFO_LOOP_UP_30 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_30, HeightArrays.HEIGHT_LOOP_DOWN_30, 30);
        public static SonicTileInfo TILEINFO_LOOP_UP_45 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_45, HeightArrays.HEIGHT_LOOP_DOWN_45, 45);
        public static SonicTileInfo TILEINFO_LOOP_UP_60 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_60, HeightArrays.HEIGHT_LOOP_DOWN_60, 60);
        public static SonicTileInfo TILEINFO_LOOP_UP_70 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_70, HeightArrays.HEIGHT_LOOP_DOWN_70, 70);
        public static SonicTileInfo TILEINFO_LOOP_UP_85 = new SonicTileInfo(HeightArrays.HEIGHT_LOOP_UP_85, HeightArrays.HEIGHT_LOOP_DOWN_85, 85);

        // LOOP CORNER ROTATIONS NEED TO BE CALCULATED, WHICH CAN BE DONE IN TILE.CS.



    }
}
