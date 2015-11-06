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

        // On/off
        public bool On = true;

        public SonicTileInfo(int[] fha, int[] wha, float Ang)
        {
            flatheightArray = fha;
            wallheightArray = wha;
            Angle = Ang;
        }

        public void Toggle()
        {
            On = !On;
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
                    // Wall Ceil tiles
                    case 21:
                        Angle = 90;
                        break;
                    case 41:
                        Angle = 180;
                        break;
                    case 61:
                        Angle = 270;
                        break;


                    // Loop Floor->Right Wall
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
                    // Loop Left Wall-> Floor
                    case 147:
                        Angle = 350;
                        break;
                    case 146:
                        Angle = 340;
                        break;
                    case 145:
                        Angle = 330;
                        break;
                    case 125:
                        Angle = 315;
                        break;
                    case 124:
                        Angle = 300;
                        break;
                    case 104:
                        Angle = 290;
                        break;
                    case 84:
                        Angle = 280;
                        break;
                    // Loop Ceiling-> Left Wall
                    case 64:
                        Angle = 260;
                        break;
                    case 44:
                        Angle = 250;
                        break;
                    case 24:
                        Angle = 240;
                        break;
                    case 25:
                        Angle = 225;
                        break;
                    case 5:
                        Angle = 210;
                        break;
                    case 6:
                        Angle = 200;
                        break;
                    case 7:
                        Angle = 190;
                        break;
                    // Loop Rightwall-> Ceiling
                    case 8:
                        Angle = 170;
                        break;
                    case 9:
                        Angle = 160;
                        break;
                    case 10:
                        Angle = 150;
                        break;
                    case 30:
                        Angle = 135;
                        break;
                    case 31:
                        Angle = 120;
                        break;
                    case 51:
                        Angle = 110;
                        break;
                    case 71:
                        Angle = 100;
                        break;

                    // Outer Curve -> Clockwise 1st Quad
                    case 15:
                        Angle = 10;
                        break;
                    case 14:
                        Angle = 20;
                        break;
                    case 13:
                        Angle = 30;
                        break;
                    case 33:
                        Angle = 45;
                        break;
                    case 32:
                        Angle = 60;
                        break;
                    case 52:
                        Angle = 70;
                        break;
                    case 72:
                        Angle = 80;
                        break;

                    // Outer Curve -> Clockwise 2nd Quad
                    case 92:
                        Angle = 100;
                        break;
                    case 112:
                        Angle = 110;
                        break;
                    case 132:
                        Angle = 120;
                        break;
                    case 133:
                        Angle = 135;
                        break;
                    case 153:
                        Angle = 150;
                        break;
                    case 154:
                        Angle = 160;
                        break;
                    case 155:
                        Angle = 170;
                        break;

                    // Outer Curve -> Clockwise 3rd Quad
                    case 156:
                        Angle = 190;
                        break;
                    case 157:
                        Angle = 200;
                        break;
                    case 158:
                        Angle = 210;
                        break;
                    case 138:
                        Angle = 225;
                        break;
                    case 139:
                        Angle = 240;
                        break;
                    case 119:
                        Angle = 250;
                        break;
                    case 99:
                        Angle = 260;
                        break;

                    // Outer Curve -> Clockwise 4th Quad
                    case 79:
                        Angle = 280;
                        break;
                    case 59:
                        Angle = 290;
                        break;
                    case 39:
                        Angle = 300;
                        break;
                    case 38:
                        Angle = 315;
                        break;
                    case 18:
                        Angle = 330;
                        break;
                    case 17:
                        Angle = 340;
                        break;
                    case 16:
                        Angle = 350;
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
