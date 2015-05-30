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
// Purpose: This is the movement component for Sonic-like movement. This includes the various speeds (like x, y, ground speed/momentum etc) 
// and sensors.

namespace NotSonic.Components
{
    class SonicMovement : Component
    {
        // Sonic has multiple movement types.
        enum MoveType
        {
            GROUND,
            AIR
        }

        // Sonic also has multiple ground directions (floor mode, wall modes, etc)
        enum FloorMode
        {
            FLOOR, 
            RIGHTWALL, 
            CEILING, 
            LEFTWALL
        }

        // We track these
        public MoveType CurrentMoveType = MoveType.GROUND;
        public FloorMode CurrentFloorMode = FloorMode.FLOOR;

        // This is a reference to the current list of tiles.
        public List<Tile> TileList;
        
        
        // These are the many speed variables used in the Sonic 1, 2, 3&K engines.
        public float GroundSpeed = 0.0f; // GroundSpeed tracks momentum - it's used to calculate how fast sonic is going to move along the ground.
        public float XSpeed = 0.0f;
        public float YSpeed = 0.0f;
        public float Acceleration = 0.0f;
        public float Deceleration = 0.0f;
        public float Friction = 0.0f;
        public float JumpVelocity = 0.0f;
        public float Gravity = 0.0f;
        public float Angle = 0.0f;
        public float SlopeFactor = 0.0f;

        // Position in world
        public float XPos = 0.0f;
        public float YPos = 0.0f;

        // Which way we're facing.
        public bool FacingRight = true;

        // Are we rolling?
        public bool Rolling = false;

        #region Public Methods

        /// <summary>
        /// Updates the Movement.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Check sensors for solid ground:


            // Update Slope Factor
            if (CurrentMoveType != MoveType.AIR)
            {
                // If sonic is running, the slope factor is 0.125.
                SlopeFactor = 0.125f;

                if (Rolling)
                {
                    // If sonic is rolling, the slope factor is altered further depending on if he's going uphill or downhill.
                    // This is easy to check; if he's rolling uphill, the sign of GroundSpeed is not equal to the sign of Sin(Angle).
                    if (GroundSpeed < 0 && Math.Sin(Angle) >= 0)
                    {
                        // Uphill, it's 0.078125
                        SlopeFactor = 0.078125f;
                    }
                    else
                    {
                        // Downhill, it's 0.3125.
                        SlopeFactor = 0.3125f;
                    }
                }
            }
            // Slope factor is added to Ground Speed. This slows sonic when moving uphill, and speeds him up when moving downhill.
            GroundSpeed += SlopeFactor * (float)Math.Sin(Angle);
            
            // Use ground speed to calculate the X and Y Speeds.
            XSpeed = GroundSpeed * (float)Math.Cos(Angle);
            YSpeed = GroundSpeed * -(float)Math.Sin(Angle);

            XPos += XSpeed;
            YPos += YSpeed;
        }

        public void Jump()
        {
            // When sonic jumps, we need to make sure we jump perpendicular to the angle of travel.
            XSpeed -= JumpVelocity * (float)Math.Sin(Angle);
            YSpeed -= JumpVelocity * (float)Math.Cos(Angle);
        }

        // Sensor checks. [MESSY]

        public void CheckWallSensor()
        {
            // Check for tiles that are at the sides of sonic, relative to Y+4.
            float LineY = YPos + 4;

            // Left and Right edges are at +-10 on the X axis.
            float LineXLeft = XPos - 10;
            float LineXRight = XPos + 10;

            // Cycle through all tiles in level (need a way to filter only close ones... maybe only pass close ones in?)
            foreach(Tile tile in TileList)
            {
                // Our line goes from -10, 4 to 10, 4. If anything intersects this, it's a collision to sonic's sides.
                // It's easiest to check if something is OUTSIDE a collision...
                if(tile.XPos + 16.0f < LineXLeft || tile.XPos > LineXRight)
                {
                    //nada
                }
                else
                {
                    // Make sure the Y position is in range.
                    if(tile.YPos + 16.0f < LineY || tile.YPos > LineY)
                    {
                        //nada
                    }
                    else
                    {
                        // Collision, cap'n!
                        // Now, if the collision is on the left of sonic...
                        if(tile.XPos < XPos)
                        {
                            // Pop sonic to the right by the requisite amount.
                            XPos += (XPos - (tile.XPos + 16.0f)) + 1;
                        }
                        else
                        {
                            // Pop sonic to the left
                            XPos -= (tile.XPos - XPos) - 1;
                        }

                        // Set ground speed to 0
                        GroundSpeed = 0;

                        break;
                    }
                }
            }
        }

        public void CheckGroundSensors()
        {
            // Sensor A: Positioned at -9, 0 to -9, 20.
            float SensorAX = XPos - 9;
            float SensorATop = YPos + 0;
            float SensorABottom = YPos + 20;

            // Sensor B: Positioned at 9, 0 to 9, 20.
            float SensorBX = XPos + 9;
            float SensorBTop = YPos + 0;
            float SensorBBottom = YPos + 20;

            // The tiles that will be located.
            Tile sensorATile = null;
            Tile sensorBTile = null;


            // Check Sensor A.
            foreach (Tile tile in TileList)
            {
                // It's easiest to check if something is OUTSIDE a collision...
                if (tile.YPos + 16.0f < SensorATop || tile.YPos > SensorABottom)
                {
                    //nada
                }
                else
                {
                    // Make sure the Y position is in range.
                    if (tile.XPos + 16.0f < SensorAX || tile.XPos > SensorAX)
                    {
                        //nada
                    }
                    else
                    {
                        // Collision, cap'n!
                        
                        // Select this tile. 
                        sensorATile = tile;

                        break;
                    }
                }
            }

            // Check Sensor B.
            foreach (Tile tile in TileList)
            {
                // It's easiest to check if something is OUTSIDE a collision...
                if (tile.YPos + 16.0f < SensorBTop || tile.YPos > SensorBBottom)
                {
                    //nada
                }
                else
                {
                    // Make sure the Y position is in range.
                    if (tile.XPos + 16.0f < SensorBX || tile.XPos > SensorBX)
                    {
                        //nada
                    }
                    else
                    {
                        // Collision, cap'n!

                        // Select this tile. 
                        sensorBTile = tile;

                        break;
                    }
                }
            }


            // Now that we've checked for the sensor tiles, let's have a look...
            if(sensorATile == null && sensorBTile == null)
            {
                // We didn't collide with anything. WE'RE FALLING AAARGH
                CurrentMoveType = MoveType.AIR;
                return;
            }
            else
            {
                // At least one was encountered, we must be on the ground.
            }

            // Store collision info
            int heightOfA = 0;
            int heightOfB = 0;
            float angleOfA = 0.0f;
            float angleOfB = 0.0f;

            if(sensorATile != null)
            {
                // Capture sensor A's result.
                int heightMapArrayIndex = (int)SensorAX - sensorATile.XPos;
                heightOfA = sensorATile.flatheightArray[heightMapArrayIndex];
                angleOfA = sensorATile.Angle;

            }
            if(sensorBTile != null)
            {
                // Capture sensor B's result.
                int heightMapArrayIndex = (int)SensorBX - sensorBTile.XPos;
                heightOfB = sensorBTile.flatheightArray[heightMapArrayIndex];
                angleOfB = sensorBTile.Angle;
            }

            if(heightOfA >= heightOfB && sensorATile != null)
            {
                YPos = sensorATile.YPos - heightOfA - 20;
                Angle = angleOfA;
            }
            else if(heightOfB > heightOfA && sensorBTile != null)
            {
                YPos = sensorBTile.YPos - heightOfB - 20;
                Angle = angleOfB;
            }

            return;
            
        }
       
        #endregion

    }
}
