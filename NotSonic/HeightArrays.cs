using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;



namespace NotSonic
{
    class HeightArrays
    {
        public static int[] HEIGHT_ARRAY_EMPTY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] HEIGHT_ARRAY_FULL = { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };

        // FLOOR-RIGHTWALL SLOPES
        public static int[] HEIGHT_SLP_45_UP = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public static int[] HEIGHT_LOOP_UP_10 =  { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3 };
        public static int[] HEIGHT_LOOP_UP_20 =  { 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };
        public static int[] HEIGHT_LOOP_UP_30 = { 10, 10, 11, 12, 12, 13, 14, 14, 15, 16, 16, 16, 16, 16, 16, 16 };
        public static int[] HEIGHT_LOOP_UP_45 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6 };
        public static int[] HEIGHT_LOOP_UP_60 = { 7, 8, 9, 11, 13, 14, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
        public static int[] HEIGHT_LOOP_UP_70 = { 0, 0, 0, 0, 0, 0, 0, 2, 4, 6, 8, 11, 14, 16, 16, 16 };
        public static int[] HEIGHT_LOOP_UP_85 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 8, 16 };

        // LEFTWALL-FLOOR SLOPES
        public static int[] HEIGHT_SLP_45_DOWN = { 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };

        public static int[] HEIGHT_LOOP_DOWN_10 = { 16, 8, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] HEIGHT_LOOP_DOWN_20 = { 16, 16, 16, 14, 11, 8, 6, 4, 2, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] HEIGHT_LOOP_DOWN_30 = { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 14, 13, 11, 9, 8, 7 };
        public static int[] HEIGHT_LOOP_DOWN_45 = { 6, 5, 4, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] HEIGHT_LOOP_DOWN_60 = { 16, 16, 16, 16, 16, 16, 16, 15, 14, 14, 13, 12, 12, 11, 10, 10 };
        public static int[] HEIGHT_LOOP_DOWN_70 = { 9, 9, 8, 8, 7, 7, 6, 6, 5, 5, 5, 4, 4, 4, 3, 3 };
        public static int[] HEIGHT_LOOP_DOWN_85 = { 3, 3, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1 };


        // The Big Generated Array
        public static List<int[]> HeightArraysList = new List<int[]>();


        public static void GenerateHeightMap()
        {
//             // Load Angle_Tiles.png
//             Image angleTileImg = new Image(Assets.TILE_SHEET_ANGLES);
//             Texture angleTileTex = angleTileImg.Texture;
//             
// 
//             // Loop 16x16, increment tile line counter
//             int tileLine = 0;
//             for (tileLine = 0; tileLine < (angleTileImg.Height / 16); tileLine++ )
//             {
//                 int[] heightArray = new int[angleTileImg.Width];
//                 for (int x = 0; x < angleTileImg.Width; x += 16)
//                 {
//                     // Read pixel values into height array for this tile
// 
//                     int y = 0;
//                     for (y = 0; y < 16; y++)
//                     {
//                         Color returnedCol = angleTileTex.GetPixel(x, angleTileImg.Height - (y + (tileLine * 16)));
//                         if (returnedCol.R > 0)
//                         {
//                             break;
//                         }
// 
//                     }
//                     heightArray[x] = y;
//                 }
// 
//                 // Store the height values by block in the heightarrayslist
// 
//                 for (int i = 0; i < angleTileImg.Width; i++)
//                 {
//                     int[] heightOfTile = new int[16];
//                     for (int j = 0; j < 16; j++)
//                     {
//                         heightOfTile[j] = heightArray[i];
//                     }
//                     HeightArraysList.Add(heightOfTile);
// 
//                 }


            }
            
        }
    }
}
