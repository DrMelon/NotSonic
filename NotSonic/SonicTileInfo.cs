using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotSonic
{
    class SonicTileInfo
    {
        public int[] flatheightArray = new int[16];
        public int[] wallheightArray = new int[16];

        // Tile angle
        public float Angle;

        public SonicTileInfo(int[] fha, int[] wha, float Ang)
        {
            flatheightArray = fha;
            wallheightArray = wha;
            Angle = Ang;
        }
    }
}
