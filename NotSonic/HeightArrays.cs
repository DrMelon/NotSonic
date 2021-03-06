﻿using System;
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

        // Load Angle_Tiles.png
        public static Image angleTileImg = new Image(Assets.TILE_SHEET_ANGLES);

        public struct HeightInfo
        {
            public int[] floorHeight;
            public int[] rightHeight;
            public int[] ceilHeight;
            public int[] leftHeight;
            public Vector2 surfNorm;
        }


        // The Big Generated Array
        public static List<HeightInfo> HeightArraysList = new List<HeightInfo>();


        public static void GenerateHeightMap()
        {
            // Read all tiles.
            for(int i = 0; i < 755; i++)
            {
                HeightArraysList.Add(ReadTile(i));
            }

           
        }

        public static int[] ReadArrayBackwards(int[] inarray)
        {
            int[] newArr = new int[16];
            for(int i = 0; i < 16; i++)
            {
                newArr[i] = inarray[15 - i];
            }
            return newArr;
        }
        public static int[] ReadArrayInverted(int[] inarray)
        {
            int[] newArr = new int[16];
            for (int i = 0; i < 16; i++)
            {
                newArr[i] = 16 - inarray[i];
            }
            return newArr;
        }

        public static int[] FetchArrayHeight(int ID, int type)
        {
            if(type == 0)
            {
                return HeightArraysList.ElementAt(ID).floorHeight;
            }
            if (type == 1)
            {
                return HeightArraysList.ElementAt(ID).rightHeight;
            }
            if (type == 2)
            {
                return HeightArraysList.ElementAt(ID).ceilHeight;
            }
            if (type == 3)
            {
                return HeightArraysList.ElementAt(ID).leftHeight;
            }
            

            return null;
        }

        public static float GetAngleFromArrayHeight(int[] arrayheight, int type, bool moveRight)
        {




            float firstHeight = 0;
            float lastHeight = 0;
            float firstHeightPos = 0;
            float lastHeightPos = 0;

            for(int i = 0; i < 16; i++)
            {
                // go through array, find first nonempty first height
                if(arrayheight[i] > 0 && firstHeight == 0)
                {
                    firstHeight = arrayheight[i];
                    firstHeightPos = i;
                }
                //find last height
                if(arrayheight[i] > 0)
                {
                    lastHeight = arrayheight[i];
                    lastHeightPos = i;
                }
            }

            float slope = 0;
            float Angle = 0;

            // get slope of these two values
            if ((lastHeightPos - firstHeightPos) == 0)
            {
                //vertical? 
                Angle = 90;
            }
            else
            {
                
                slope = (lastHeight - firstHeight) / (lastHeightPos - firstHeightPos);
                float atanSlope = (float)Math.Atan(slope);
                if (atanSlope < 0 && moveRight)
                {
                    atanSlope += (float)Math.PI * 2.0f;
                }
                Angle = atanSlope * (180.0f / (float)Math.PI);

                // to nearest multiple of 5
                Angle = (float)Math.Round(Angle / 5.0f) * 5.0f;

                
                
                
            }

            if (moveRight)
            {

                if (type == 1)
                {

                    if (Angle > 180)
                    {
                        Angle = 360 - Angle;
                        Angle += 90;
                    }
                    else
                    {
                        Angle += 90;
                    }

                }
                if (type == 2)
                {

                    if (Angle > 270)
                    {
                        Angle = 360 - Angle;
                        Angle += 180;
                    }
                    else
                    {
                        Angle += 180;
                    }



                }
                if (type == 3)
                {
                    
                    if (Angle > 360)
                    {
                        Angle = 360 - Angle;
                        Angle += 270;
                    }
                    else
                    {
                        Angle += 270;
                    }


                    
                }

            }
            else
            {
                if(type == 0)
                {
                    if(Angle < 0)
                    {
                        Angle = 360 + Angle;
                    }
                }
                if(type == 1)
                {
                    Angle += 90;
                    if(Angle > 90)
                    {
                        Angle = 180 - Angle;
                    }
                }
                if(type == 2)
                {
                    Angle += 180;
                    if(Angle > 180)
                    {
                        Angle = 360 - Angle;
                    }
                }
                if(type == 3)
                {
                    Angle += 270;
                }
            }

            return Angle;
        }

        public static int[] GetFirstLastHeight(int ID, int type)
        {
            int[] info = new int[2];
            int[] heightarr = FetchArrayHeight(ID, type);
            int i, j;
            for (i = 0; i < 16; i++)
            {
                if (heightarr[i] != 0)
                {
                    break;
                }
            }
            for (j = 15; j >= 0; j--)
            {
                if (heightarr[j] != 0)
                {
                    break;
                }
            }
            info[0] = heightarr[i];
            info[1] = heightarr[j];

                return info;
        }

        public static HeightInfo ReadTile(int tileID)
        {
            HeightInfo newInfo = new HeightInfo();

            // Access this tile.
            int startX = tileID * 16;
            int startY = 0;
            while(startX >= angleTileImg.Width)
            {
                startX -= angleTileImg.Width;
                startY += 16;
            }

            // Now that we have startX and startY, we can read this whole tile in.
            int[,] tileData = new int[16,16];
            for (int x = startX; x < startX + 16; x++)
            {
                for(int y = startY; y < startY + 16; y++)
                {
                    tileData[x-startX,y-startY] = (angleTileImg.Texture.GetPixel(x, y).R > 0) ? 1 : 0;
                }
            }

            // Now we do the reads in each of the 4 directions.
            newInfo.floorHeight = new int[16];
            newInfo.ceilHeight = new int[16];
            newInfo.leftHeight = new int[16];
            newInfo.rightHeight = new int[16];

            // Floor Tile, Top to Bottom
            for (int x = 0; x < 16; x++)
            {
                int y;
                for(y = 0; y < 16; y++)
                {
                    if(tileData[x,y] == 1)
                    {
                        break;
                    }
                }
                newInfo.floorHeight[x] = 16 - y;
            }

            // Ceiling Tile, Bottom to Top
            for (int x = 0; x < 16; x++)
            {
                int y;
                for (y = 15; y >= 0; y--)
                {
                    if (tileData[x,y] == 1)
                    {
                        break;
                    }
                }
                newInfo.ceilHeight[x] = y + 1;
            }

            // Right Wall Tile, Left to Right
            for (int y = 0; y < 16; y++)
            {
                int x;
                for (x = 0; x < 16; x++)
                {
                    if (tileData[x,y] == 1)
                    {
                        break;
                    }
                }
                newInfo.rightHeight[y] = 16 - x;
            }

            // Left Wall  tile, Right to Left
            for (int y = 0; y < 16; y++)
            {
                int x;
                for (x = 15; x >= 0; x--)
                {
                    if (tileData[x,y] == 1)
                    {
                        break;
                    }
                }
                newInfo.leftHeight[y] = x + 1;
            }


            return newInfo;


        }
        
    }
}
