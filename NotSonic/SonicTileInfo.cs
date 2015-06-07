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

        public int ID;

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

            ID = type;
        

            if(type != 0)
            {
                // Angles are hardcoded, for now.
                switch(type)
                {
                    default:
                        Angle = 0;
                        break;
                    case 2:
                        Angle = 315;
                        break;
                    case 3:
                        Angle = 45;
                        break;
                    case 148:
                        Angle = 10;
                        break;
                    case 149:
                        Angle = 20;
                        break;
                    case 150:
                        Angle = 30;
                        break;
                    case 130:
                        Angle = 45;
                        break;
                    case 131:
                        Angle = 60;
                        break;
                    case 111:
                        Angle = 70;
                        break;
                    case 91:
                        Angle = 80;
                        break;

                }


                if(flipx && !flipy) // Left Wall
                {
                    Angle = 360 - Angle;
                }
                if (!flipx && flipy) // Right Ceil
                {
                    Angle = 180 - Angle;
                }
                if(flipx && flipy) // Left Ceil
                {
                    Angle = 180 + Angle;
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
