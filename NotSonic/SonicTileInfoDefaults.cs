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
        public static SonicTileInfo TILEINFO_SLOPE_45_DOWN = new SonicTileInfo(HeightArrays.HEIGHT_SLP_45_DOWN, HeightArrays.HEIGHT_ARRAY_FULL, -45.0f);
    }
}
