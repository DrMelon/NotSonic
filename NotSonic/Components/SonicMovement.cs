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
        public enum MoveType
        {
            GROUND,
            AIR
        }

        // Sonic also has multiple ground directions (floor mode, wall modes, etc)
        public enum FloorMode
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
        public float Acceleration = 0.046875f;
        public float Deceleration = 0.5f;
        public float AirAccel = 0.09375f;
        public float Friction = 0.046875f;
        public float TopSpeed = 6.0f;
        public float JumpVelocity = 6.5f;
        public float Gravity = 0.21875f;
        public float Angle = 0.0f;
        public float SlopeFactor = 0.0f;
        public float CurrentHeight = 20.0f;

        // Spindashing
        public float CurrentSpindashStrength = 0.0f;
        public float MaxSpindashStrength = 8.0f;

        // Position in world
        public float XPos = 0.0f;
        public float YPos = 0.0f;

        // Which way we're facing.
        public bool FacingRight = true;
        public bool FacingUp = true;

        // Are we rolling?
        public bool Rolling = false;

        // Mid-jump?
        public bool Jumping = false;


        // Sensors
        public Sensor wallSensor;
        public Sensor groundSensorA;
        public Sensor groundSensorB;

        #region Public Methods


        public override void Added()
        {
            XPos = Entity.X;
            YPos = Entity.Y;

            wallSensor = new Sensor(0,0,0,false);
            groundSensorA = new Sensor(0,0,0,true);
            groundSensorB = new Sensor(0,0,0,true);

            base.Added();
        }

        public float HexAngleToDec(int hexangle)
        {
            return (hexangle) * 1.40625f;
        }


        /// <summary>
        /// Updates the Movement.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Check left/right flip
            if (XSpeed > 0)
            {
                FacingRight = true;
            }
            if (XSpeed < 0)
            {
                FacingRight = false;
            }

            // Handle input
            HandleInput();

            // Check sensors for solid ground:
            CheckGroundSensors();
            CheckWallSensor();

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

                // Check heights
                if(Rolling)
                {
                    CurrentHeight = 15;
                }
                else
                {
                    CurrentHeight = 20;
                }


                // Slope factor is added to Ground Speed. This slows sonic when moving uphill, and speeds him up when moving downhill.
                GroundSpeed += SlopeFactor * -(float)Math.Sin(Angle);

                // Use ground speed to calculate the X and Y Speeds.
                XSpeed = GroundSpeed * (float)Math.Cos(Angle);
                YSpeed = GroundSpeed * -(float)Math.Sin(Angle);
            }

            // Slow down spindash amt
            CurrentSpindashStrength = CurrentSpindashStrength - (float)(Math.Floor(CurrentSpindashStrength / 0.125) / 256);
            if(CurrentSpindashStrength < 0.0f)
            {
                CurrentSpindashStrength = 0.0f;
            }

            XPos += XSpeed;
            YPos += YSpeed;

            // Apply to parent ent
            Entity.X = XPos;
            Entity.Y = YPos;









        }

        public void Jump()
        {
            // When sonic jumps, we need to make sure we jump perpendicular to the angle of travel.
            XSpeed -= JumpVelocity * (float)Math.Sin(Angle);
            YSpeed -= JumpVelocity * (float)Math.Cos(Angle);
            Jumping = true;
            CurrentMoveType = MoveType.AIR;

        }

        // Sensor checks. [MESSY]

        public void CheckWallSensor()
        {
            // Check for tiles that are at the sides of sonic, relative to Y+4.
            wallSensor.APos = YPos - 4;

            // Left and Right edges are at +-10 on the X axis.
            wallSensor.BPos1 = XPos - 10;
            wallSensor.BPos2 = XPos + 10;

            Sensor.CollisionInfo colInfo = wallSensor.Sense(TileList);
            if (!colInfo.thisIsNull)
            {
                // Collision, cap'n!
                // Now, if the collision is on the left of sonic...
                if (colInfo.tileHit.X < XPos)
                {
                    // Pop sonic to the right by the requisite amount.
                    XPos += (XPos - (colInfo.tileHit.X + 16.0f)) + 1 - 10.0f;
                }
                else
                {
                    // Pop sonic to the left
                    XPos -= (colInfo.tileHit.X - XPos) - 1;
                }

                // Set ground speed to 0
                GroundSpeed = 0;
            }

        }

        public void CheckGroundSensors()
        {
            // Check Mode.
            if(CurrentFloorMode == FloorMode.FLOOR)
            {
                // Sensor A: Positioned at -9, 0 to -9, 20.
                groundSensorA.APos = XPos - 9;
                groundSensorA.BPos1 = YPos + 0;
                groundSensorA.BPos2 = YPos + 0 + 20;
                groundSensorA.verticalSensor = true;

                // Sensor B: Positioned at 9, 0 to 9, 20.
                groundSensorB.APos = XPos + 9;
                groundSensorB.BPos1 = YPos + 0;
                groundSensorB.BPos2 = YPos + 0 + 20;
                groundSensorB.verticalSensor = true;
            }
            if (CurrentFloorMode == FloorMode.CEILING)
            {
                groundSensorA.APos = XPos + 9;
                groundSensorA.BPos1 = YPos + 0;
                groundSensorA.BPos2 = YPos + 0 - 20;
                groundSensorA.verticalSensor = true;

                groundSensorB.APos = XPos - 9;
                groundSensorB.BPos1 = YPos + 0;
                groundSensorB.BPos2 = YPos + 0 - 20;
                groundSensorB.verticalSensor = true;
            }
            if (CurrentFloorMode == FloorMode.RIGHTWALL)
            {
                // Sensor A: Positioned at -9, 0 to -9, 20.
                groundSensorA.APos = YPos + 9;
                groundSensorA.BPos1 = XPos + 0;
                groundSensorA.BPos2 = XPos + 0 + 20;
                groundSensorA.verticalSensor = false;

                // Sensor B: Positioned at 9, 0 to 9, 20.
                groundSensorB.APos = YPos - 9;
                groundSensorB.BPos1 = XPos + 0;
                groundSensorB.BPos2 = XPos + 0 + 20;
                groundSensorB.verticalSensor = false;
            }
            if (CurrentFloorMode == FloorMode.LEFTWALL)
            {
                // Sensor A: Positioned at -9, 0 to -9, 20.
                groundSensorA.APos = YPos - 9;
                groundSensorA.BPos1 = XPos + 0;
                groundSensorA.BPos2 = XPos + 0 - 20;
                groundSensorA.verticalSensor = false;

                // Sensor B: Positioned at 9, 0 to 9, 20.
                groundSensorB.APos = YPos + 9;
                groundSensorB.BPos1 = XPos + 0;
                groundSensorB.BPos2 = XPos + 0 - 20;
                groundSensorB.verticalSensor = false;
            }




            // The tiles that will be located.
            Tile sensorATile = null;
            Tile sensorBTile = null;


            // Sense those tiles!
            Sensor.CollisionInfo colInfoA = groundSensorA.Sense(TileList);
            Sensor.CollisionInfo colInfoB = groundSensorB.Sense(TileList);

            sensorATile = colInfoA.tileHit;
            sensorBTile = colInfoB.tileHit;



            // Now that we've checked for the sensor tiles, let's have a look...
            if(sensorATile == null && sensorBTile == null)
            {
                // We didn't collide with anything. WE'RE FALLING AAARGH
                CurrentMoveType = MoveType.AIR;
                return;
            }
            else
            {
                // At least one was encountered, we must be touching something.
                // Store collision info
                int heightOfA = 0;
                int heightOfB = 0;
                int fullheightOfA = 0;
                int fullheightOfB = 0;
                float angleOfA = 0.0f;
                float angleOfB = 0.0f;

                /// THIS ONLY WORKS FOR FLOORMODE CURRENTLY!!

                if (sensorATile != null)
                {
                    // Capture sensor A's result.
                    int heightMapArrayIndex = (int)groundSensorA.APos - (int)sensorATile.X;
                    heightMapArrayIndex = Math.Min(heightMapArrayIndex, 15);
                    heightOfA = sensorATile.myTileInfo.flatheightArray[heightMapArrayIndex];
                    fullheightOfA = heightOfA + (1600 - (int)sensorATile.Y);
                    angleOfA = sensorATile.myTileInfo.Angle;

                    // If the tile is empty of collision, don't collide with it. Duh!
                    if(heightOfA == 0)
                    {
                        sensorATile = null;
                    }

                }
                if (sensorBTile != null)
                {
                    // Capture sensor B's result.
                    int heightMapArrayIndex = (int)groundSensorB.APos - (int)sensorBTile.X;
                    heightMapArrayIndex = Math.Min(heightMapArrayIndex, 15);
                    heightOfB = sensorBTile.myTileInfo.flatheightArray[heightMapArrayIndex];
                    fullheightOfB = heightOfB + (1600 - (int)sensorBTile.Y);
                    angleOfB = sensorBTile.myTileInfo.Angle;

                    // If the tile is empty of collision, don't collide with it. Duh!
                    if (heightOfB == 0)
                    {
                        sensorBTile = null;
                    }
                }


                if (CurrentMoveType == MoveType.GROUND || YSpeed >= 0)
                {
                    if (fullheightOfA >= fullheightOfB && sensorATile != null)
                    {
                        YPos = sensorATile.Y + 16 - heightOfA - 20;
                        Angle = angleOfA;

                    }
                    else if(sensorBTile != null)
                    {
                        YPos = sensorBTile.Y + 16 - heightOfB - 20;
                        Angle = angleOfB;
                    }
                }


                // If we were in the air, reset the groundspeed.
                if(CurrentMoveType == MoveType.AIR && YSpeed >= 0)
                {
                    CurrentMoveType = MoveType.GROUND;
                    Rolling = false;


                    // Account for sloping...
                    if (Math.Abs(XSpeed) > YSpeed || Math.Abs(Angle) < 10f)
                    {
                        GroundSpeed = XSpeed;
                    }
                    else
                    {
                        GroundSpeed = YSpeed * (float)Math.Sin(-Angle);
                    }
                    
                    
                }
            }


            return;
            
        }

        public void HandleInput()
        {
            // Get ref to controller
            NotSonic.System.SegaController theController = Global.playerSession.GetController<NotSonic.System.SegaController>();

            /// Check d-pad.
            /// GROUND:
            if (CurrentMoveType == MoveType.GROUND)
            {
                /// RUNNING & Not Spindashing
                if (!(CurrentSpindashStrength > 0.0f))
                {
                    if (!Rolling)
                    {
                        if (theController.Left.Down)
                        {
                            FacingRight = false;
                            if (GroundSpeed > 0) //Heading right, now going left
                            {
                                GroundSpeed -= Deceleration;
                            }
                            else if (GroundSpeed > -TopSpeed)
                            {
                                // Zoom!
                                GroundSpeed -= Acceleration;
                            }
                            else
                            {
                                GroundSpeed = -TopSpeed;
                            }

                        }
                        else if (theController.Right.Down)
                        {
                            FacingRight = true;
                            if (GroundSpeed < 0)
                            {
                                GroundSpeed += Deceleration;
                            }
                            else if (GroundSpeed < TopSpeed)
                            {
                                GroundSpeed += Acceleration;
                            }
                            else
                            {
                                GroundSpeed = TopSpeed;
                            }
                        }
                        else
                        {
                            // FRICTION
                            if (Math.Abs(GroundSpeed) < Friction)
                            {
                                GroundSpeed = 0;
                            }
                            else
                            {
                                GroundSpeed -= Friction * Math.Sign(GroundSpeed);
                            }

                        }
                    }
                    else //ROLLING
                    {
                        // Can only DECELERATE while rolling.
                        if (theController.Left.Down)
                        {
                            FacingRight = false;
                            if (GroundSpeed > 0) //Heading right, now going left
                            {
                                GroundSpeed -= 0.125f;
                            }

                        }
                        else if (theController.Right.Down)
                        {
                            FacingRight = true;
                            if (GroundSpeed < 0)
                            {
                                GroundSpeed += 0.125f;
                            }
                        }


                        // FRICTION is always active during rolling.
                        if (Math.Abs(GroundSpeed) < Friction / 2)
                        {
                            GroundSpeed = 0;
                            Rolling = false;
                        }
                        else
                        {
                            GroundSpeed -= (Friction / 2) * Math.Sign(GroundSpeed);
                        }
                    }
                }


                // Check for jump.
                if(theController.A.Pressed || theController.B.Pressed || theController.C.Pressed)
                {
                    if (theController.Down.Down && !Rolling && Math.Abs(GroundSpeed) < 1.03125)
                    {
                        // Add to spindash counter
                        CurrentSpindashStrength += 2.0f;
                        if(CurrentSpindashStrength > MaxSpindashStrength)
                        {
                            CurrentSpindashStrength = MaxSpindashStrength;
                           
                        }

                    }
                    else
                    {
                        Jump();
                        // Voluntary jumps = rolling state...
                        Rolling = true;
                    }
                }

                // Roll up
                if(theController.Down.Down && !Rolling && Math.Abs(GroundSpeed) > 1.03125)
                {
                    Rolling = true;
                }

                // Spindash release
                if (theController.Down.Released && CurrentSpindashStrength > 0.0f)
                {
                    if (FacingRight)
                    {
                        GroundSpeed += 8 + (float)(Math.Floor(CurrentSpindashStrength) / 2);
                        Rolling = true;
                        CurrentSpindashStrength = 0.0f;
                    }
                    else
                    {
                        GroundSpeed -= 8 + (float)(Math.Floor(CurrentSpindashStrength) / 2);
                        Rolling = true;
                        CurrentSpindashStrength = 0.0f;
                    }
                }

            }
            else
            {
                // AIR MODE
                if(theController.Left.Down)
                {
                    XSpeed -= AirAccel;
                }
                else if(theController.Right.Down)
                {
                    XSpeed += AirAccel;
                }

                // Air Drag
                if (YSpeed < 0 && YSpeed > -4.0f)
                {
                    if (Math.Abs(XSpeed) >= 0.125f)
                    {
                        XSpeed *= 0.96875f;
                    }
                }


                // Gravity is applied to Y-velocity
                YSpeed += Gravity;
                if(YSpeed > 16.0f)
                {
                    // SonicCD fall speed limit
                    //YSpeed = 16.0f;
                }

                // Cut jump short if no jump buttons held
                if(Jumping && !theController.A.Down && !theController.B.Down && !theController.C.Down)
                {
                    Jumping = false;
                    if(YSpeed < -4.0f)
                    {
                        YSpeed = -4.0f;
                    }
                }


            }
        }
       
        #endregion

    }
}
