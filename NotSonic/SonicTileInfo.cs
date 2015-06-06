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

        public SonicTileInfo(int type, bool flipx, bool flipy)
        {
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
             *  /
             */
            if(type != 0)
            {

            

                flatheightArray = HeightArrays.HeightArraysList.ElementAt(type);
                wallheightArray = HeightArrays.ReadArrayBackwards(HeightArrays.HeightArraysList.ElementAt(type));
                switch(type)
                {
                    default:
                        Angle = 0;
                        break;
                    case 2:
                        Angle = 45;
                        break;
                    case 3:
                        Angle = 315;
                        break;
                    case 5:
                        Angle = 85;
                        break;
                    case 25:
                        Angle = 70;
                        break;
                    case 44:
                        Angle = 45;
                        break;
                    case 45:
                        Angle = 60;
                        break;
                    case 62:
                        Angle = 10;
                        break;
                    case 63:
                        Angle = 20;
                        break;
                    case 64:
                        Angle = 30;
                        break;
                }

                if(flipx && !flipy)
                {
                    Angle = 360 - Angle;
                    flatheightArray = HeightArrays.ReadArrayBackwards(flatheightArray);
                    wallheightArray = HeightArrays.ReadArrayBackwards(wallheightArray);
                }
                if (!flipx && flipy)
                {
                    Angle = 180 - Angle;
                    flatheightArray = HeightArrays.ReadArrayInverted(flatheightArray);
                    wallheightArray = HeightArrays.ReadArrayInverted(wallheightArray);
                }
                if(flipx && flipy)
                {
                    Angle = 180 + Angle;
                    flatheightArray = HeightArrays.ReadArrayInverted(HeightArrays.ReadArrayBackwards(flatheightArray));
                    wallheightArray = HeightArrays.ReadArrayInverted(HeightArrays.ReadArrayBackwards(wallheightArray));
                }

            }
            else
            {
                flatheightArray = HeightArrays.HEIGHT_ARRAY_EMPTY;
                wallheightArray = HeightArrays.HEIGHT_ARRAY_EMPTY;
                Angle = 0;
            }


        }
    }
}
