using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        public struct CollisionInfo
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

        public bool keepCheck;
        
        // Sensor type; horizontal / vertical
        public bool verticalSensor;

        public Sensor(float Ap, float Bp1, float Bp2, bool vert)
        {
            APos = Ap;
            BPos1 = Bp1;
            BPos2 = Bp2;
            verticalSensor = vert;
        }


        // This function checks for any tiles in the collision.
        // Need a way to locate nearby tiles in the tile list, rather than iterating. Iterating will do for now.
        public CollisionInfo Sense(List<Tile> tileList)
        {

            // Initialize new collision.
            CollisionInfo newCollision = new CollisionInfo();
            newCollision.tileHit = null;
            newCollision.thisIsNull = true;

            // Check for a tile depending on type.

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

                            if (tile.myTileInfo.flatheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                            {
                                continue;
                            }

                            // Select this tile, if it's closer to us than the last.

                            newCollision.tileHit = tile;
                            newCollision.thisIsNull = false;




                            LasthitX = tile.X;
                            LasthitY = tile.Y;

                            if (tile.myTileInfo.wallheightArray == HeightArrays.HEIGHT_ARRAY_FULL && keepCheck)
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

                            if (tile.myTileInfo.flatheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                            {
                                continue;
                            }

                            // Select this tile, if it's closer to us than the last.

                            newCollision.tileHit = tile;
                            newCollision.thisIsNull = false;
                            

                            

                            LasthitX = tile.X;
                            LasthitY = tile.Y;

                            if (tile.myTileInfo.wallheightArray == HeightArrays.HEIGHT_ARRAY_FULL && keepCheck)
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
    }
}
