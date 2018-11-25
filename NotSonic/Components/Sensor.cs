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
// Purpose: This is used to "sense" collisions with the ground and objects by the player characters.
// It's basically a simple line-cast, with room for additional info.


namespace NotSonic.Components
{
    class Sensor
    {
        public class CollisionInfo
        {
            // Whether this collision is valid or not
            public bool thisIsNull;
            // The tile we hit, if any
            public NotSonic.Components.Tile tileHit;
        }

        // Start position variables. Updated by whatever holds the instances.
        public float APos;
        public float BPos1;
        public float BPos2;

        public float LasthitX;
        public float LasthitY;

        public int senseMode;
        public bool moveRight;

        public int lastHeightHit;

        public int[] lastHeight;

        public bool keepCheck;
        
        // Sensor type; horizontal / vertical
        public bool verticalSensor;

        public Tilemap CollisionTilemap;

        public Vector2 tempNormal;

        public Sensor(float Ap, float Bp1, float Bp2, bool vert)
        {
            APos = Ap;
            BPos1 = Bp1;
            BPos2 = Bp2;
            verticalSensor = vert;
        }


        // This function checks for any tiles in the collision.
        // Need a way to locate nearby tiles in the tile list, rather than iterating. Iterating will do for now.
        public CollisionInfo Sense(List<Tile> tileList, int sM)
        {
            
            senseMode = sM;
            // Initialize new collision.
            CollisionInfo newCollision = new CollisionInfo();
            newCollision.tileHit = null;
            newCollision.thisIsNull = true;

            // Check for a tile depending on type.
            lastHeightHit = 0;

            // Vertical
            if(verticalSensor)
            {


                foreach (Tile tile in tileList)
                {
                    // It's easiest to check if something is OUTSIDE a collision...
                    if (tile.Y + 16.0f < BPos1 || tile.Y > BPos2)
                    {
                        //nada
                    }
                    else
                    {
                        // Make sure the Y position is in range.
                        if (tile.X + 16.0f < APos || tile.X > APos)
                        {
                            //nada
                        }
                        else
                        {
                            // Collision, cap'n!

                            if (tile.myTileInfo.On == false || tile.myTileInfo.flatheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY || tile.myTileInfo.wallheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                            {
                                continue;
                            }

                            // Select this tile, if it's closer to us than the last.

                            newCollision.tileHit = tile;
                            newCollision.thisIsNull = false;




                            lastHeight = HeightArrays.FetchArrayHeight(tile.myType, senseMode);
                            newCollision.tileHit.myTileInfo.Angle = HeightArrays.GetAngleFromArrayHeight(lastHeight, senseMode, moveRight);
                            
                            lastHeightHit = lastHeight[Math.Min(Math.Max((int)APos - (int)tile.X, 0),15)];

                            if (CollisionTilemap != null)
                            {
                                int i;
                                for (i = (int)BPos2; i >= (int)BPos1; i--)
                                {
                                    if (TilemapExt.ReadTilemapPixel(CollisionTilemap, (int)APos, i).R > 0.5f)
                                    {
                                        // hit! 
                                        lastHeightHit = 16 - ((int)BPos2 - i);//(i - (int)BPos1) / 2;
                                        newCollision.thisIsNull = false;


                                        if (CollisionTilemap != null)
                                        {
                                            tempNormal = TilemapExt.SurfaceNormal(CollisionTilemap, (int)APos, i);
                                        }
                                        break;
                                    }
                                }

                            }

                            if (CollisionTilemap != null)
                            {
                                tempNormal = TilemapExt.SurfaceNormal(CollisionTilemap, (int)APos, (int)BPos2 - (int)lastHeightHit);
                            }


                            LasthitX = tile.X;
                            LasthitY = tile.Y;

                            if (lastHeightHit == 0)
                            {
                               continue;
                            }

                            if (lastHeightHit == 16 && keepCheck)
                            {
                                continue; //keep going until blank?
                            }


                            
                            
                            break;
                        }
                    }
                }
            }
            // Horizontal
            else
            {


                foreach (Tile tile in tileList)
                {
                    // It's easiest to check if something is OUTSIDE a collision...
                    if (tile.X + 16.0f < BPos1 || tile.X > BPos2)
                    {
                        //nada
                    }
                    else
                    {
                        // Make sure the X position is in range.
                        if (tile.Y + 16.0f < APos || tile.Y > APos)
                        {
                            //nada
                        }
                        else
                        {
                            // Collision, cap'n!

                            if (tile.myTileInfo.On == false || tile.myTileInfo.flatheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                            {
                                continue;
                            }

                            // Select this tile, if it's closer to us than the last.

                            newCollision.tileHit = tile;
                            newCollision.thisIsNull = false;

                            

                            lastHeight = HeightArrays.FetchArrayHeight(tile.myType, senseMode);
                            newCollision.tileHit.myTileInfo.Angle = HeightArrays.GetAngleFromArrayHeight(lastHeight, senseMode, moveRight);
                            lastHeightHit = lastHeight[Math.Min((int)APos - (int)tile.Y, 15)];
                            

                            LasthitX = tile.X;
                            LasthitY = tile.Y;

                            if (lastHeightHit == 0)
                            {
                               continue;
                            }

                            if (lastHeightHit == 16 && keepCheck)
                            {
                                continue; //keep going until blank?
                            }



                            break;
                        }
                    }
                }
            }
            


            return newCollision;
        }

        public void DrawSelf(Color col)
        { 
            if (verticalSensor)
            {
                Otter.Draw.Line(APos, BPos1, APos, BPos2, col);
            }
            else
            {
                Otter.Draw.Line(BPos1, APos, BPos2, APos, col);
            }

            Otter.Draw.Rectangle(LasthitX, LasthitY, 16, 16, null, col, 1);

            // Height info
            for(int i = 0; i < 16; i++)
            {
                if (lastHeight != null)
                {
                    Otter.Draw.Line(LasthitX + i + 1, LasthitY + 16, LasthitX + i + 1, LasthitY + 16 - lastHeight[i], col);
                }
            }
            

            if(verticalSensor)
            {
                Otter.Draw.Line(APos, BPos1, APos, BPos2, col);
                Otter.Draw.Line(APos, BPos1, APos + tempNormal.X * 100, BPos1 + tempNormal.Y * 100, Color.Cyan);
                Otter.Draw.Rectangle(APos - 3, BPos2 - lastHeightHit - 3, 6, 6, new Color(0, 0, 0, 0), Color.Cyan, 1);
            }

        }
    }
}
