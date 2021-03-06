﻿using System;
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

        // Current controller
        public NotSonic.System.SegaController theController;

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
        public float HLock = 0.0f;
        public float DashSpeed = 8.0f;

        // Spindashing
        public float CurrentSpindashStrength = 0.0f;
        public float MaxSpindashStrength = 12.0f;

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

        // Brake-turning?
        public bool Braking = false;

        // Bumped a wall this frame?
        public bool Bumped = false;

        // Underwater?
        public bool Underwater = false;

        // Can megamix airdash?
        public bool CanAirdash = true;

        // Should we ignore the floor? 
        public bool IgnoreFloor = false;

        // Debug view on?
        public bool DebugView = false;
        public List<KeyValuePair<Vector2, Vector2>> DebugPopLineBuffer;

        // Sensors
        public Sensor wallSensor;
        public Sensor groundSensorA;
        public Sensor groundSensorB;
        public Sensor ceilingSensorC;
        public Sensor ceilingSensorD;

        // Sounds
        public Sound jumpSound = new Sound(Assets.SND_JUMP);
        public Sound rollingSound = new Sound(Assets.SND_ROLL);
        public Sound revSound = new Sound(Assets.SND_REV);
        public Sound dashGoSound = new Sound(Assets.SND_DASHGO);
        public Sound brakeSound = new Sound(Assets.SND_BRAKE);

        // Physics Pop-Out Stuff
        public float TotalPopX;
        public float TotalPopY;
      

        // Net stuff
        public float DXPos = 0;
        public float DYPos = 0;
        public float DXSpeed = 0;
        public float DYSpeed = 0;
        public float DGroundSpeed = 0;
        public float DAngle = 0;

        #region Public Methods


        public override void Added()
        {
            XPos = Entity.X;
            YPos = Entity.Y;

            wallSensor = new Sensor(0,0,0,false);
            groundSensorA = new Sensor(0,0,0,true);
            groundSensorB = new Sensor(0,0,0,true);
            ceilingSensorC = new Sensor(0, 0, 0, true);
            ceilingSensorD = new Sensor(0, 0, 0, true);

            DebugPopLineBuffer = new List<KeyValuePair<Vector2, Vector2>>();
            
            base.Added();
        }

        public float HexAngleToDec(int hexangle)
        {
            return (hexangle) * 1.40625f;
        }

        public void CheckUnderwater()
        {
            if(Underwater)
            {
                  Acceleration = 0.046875f / 2.0f;
                  Deceleration = 0.5f / 2.0f;
                  AirAccel = 0.09375f / 2.0f;
                  Friction = 0.046875f / 2.0f;
                  TopSpeed = 6.0f / 2.0f;
                  JumpVelocity = 3.5f;
                  Gravity = 0.0625f;
                  DashSpeed = 4.0f;
            }
            else
            {
                  Acceleration = 0.046875f;
                  Deceleration = 0.5f;
                  AirAccel = 0.09375f;
                  Friction = 0.046875f;
                  TopSpeed = 6.0f;
                  JumpVelocity = 6.5f;
                  Gravity = 0.21875f;
                  DashSpeed = 8.0f;
            }
        }

        public void ExitWater()
        {
            Underwater = false;
            YSpeed *= 2.0f;
        }

        public void EnterWater()
        {
            Underwater = true;
            XSpeed *= 0.5f;
            YSpeed *= 0.25f;       
        }


        private void UpdateSlopeFactor()
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

        private void UpdateObjectHeight()
        {
            if (Rolling)
            {
                CurrentHeight = 15;
            }
            else
            {
                CurrentHeight = 20;
            }
        }

        private void CalculateSpeedFromGroundSpeed()
        {
            // Swap these for wallmode?
            YSpeed = GroundSpeed * -(float)Math.Sin(Angle * Math.PI / 180.0f);
            XSpeed = GroundSpeed * (float)Math.Cos(Angle * Math.PI / 180.0f);

            // Wallrunning stuff?
            if (Angle == 0)
            {
                if (CurrentFloorMode == FloorMode.RIGHTWALL)
                {

                    YSpeed = GroundSpeed * -(float)Math.Sin((90) * Math.PI / 180.0f);
                    XSpeed = GroundSpeed * (float)Math.Cos((90) * Math.PI / 180.0f);

                }

                if (CurrentFloorMode == FloorMode.LEFTWALL)
                {
                    YSpeed = GroundSpeed * -(float)Math.Sin((270) * Math.PI / 180.0f);
                    XSpeed = GroundSpeed * (float)Math.Cos((270) * Math.PI / 180.0f);
                }

                if (CurrentFloorMode == FloorMode.CEILING)
                {
                    YSpeed = GroundSpeed * -(float)Math.Sin((180) * Math.PI / 180.0f);
                    XSpeed = GroundSpeed * (float)Math.Cos((180) * Math.PI / 180.0f);
                    
                }
            }
        }

        public void CheckBraking()
        {
            if(Math.Abs(GroundSpeed) >= 4.5 && CurrentFloorMode == FloorMode.FLOOR && !Rolling && !Jumping && CurrentMoveType == MoveType.GROUND)
            {
                //Pushing away from current direction
                if ((theController.Left.Down && GroundSpeed > 0) || (theController.Right.Down && GroundSpeed < 0))
                {
                    if(Braking == false)
                    {
                        brakeSound.Play();
                    }
                    Braking = true;
                }
               
            }
            if(CurrentMoveType == MoveType.AIR)
            {
                Braking = false;
            }
        }

        public void ToggleDebugView()
        {
            DebugView = !DebugView;
        }


        private void AtrophySpindashStrength()
        {
            // Slow down spindash amt
            CurrentSpindashStrength = CurrentSpindashStrength - (float)(Math.Floor(CurrentSpindashStrength / 0.125) / 256);
            if (CurrentSpindashStrength < 0.0f)
            {
                CurrentSpindashStrength = 0.0f;
            }
        }

        private void ApplySpeedToPos()
        {

            XPos += XSpeed;
            YPos += YSpeed;
        }

        private void ChangeFloorMode()
        {
            // Clockwise
            if(GroundSpeed <= -0.5)
            {
                // Change which mode we're on
                if ((Angle > 315 || Angle <= 45) && CurrentFloorMode != FloorMode.FLOOR && Angle != 0)
                {
                    // Now in Floor Mode
                    CurrentFloorMode = FloorMode.FLOOR;
                }
                else if ((Angle > 45 && Angle <= 135) && CurrentFloorMode != FloorMode.RIGHTWALL)
                {
                    // Now in Floor Mode
                    CurrentFloorMode = FloorMode.RIGHTWALL;

                }
                else if ((Angle > 135 && Angle <= 225) && CurrentFloorMode != FloorMode.CEILING)
                {
                    // Now in Floor Mode
                    CurrentFloorMode = FloorMode.CEILING;

                }
                else if ((Angle > 225 && Angle <= 315) && CurrentFloorMode != FloorMode.LEFTWALL)
                {
                    // Now in Floor Mode
                    CurrentFloorMode = FloorMode.LEFTWALL;
                }
            }


            // Anticlockwise
            if(GroundSpeed >= 0.5)
            {
                // Change which mode we're on
                if ((Angle >= 315 || Angle < 45) && CurrentFloorMode != FloorMode.FLOOR && Angle != 0)
                {
                    // Now in Floor Mode
                    CurrentFloorMode = FloorMode.FLOOR;
                }
                else if ((Angle >= 45 && Angle < 135) && CurrentFloorMode != FloorMode.RIGHTWALL)
                {
                    // Now in Floor Mode
                    CurrentFloorMode = FloorMode.RIGHTWALL;

                }
                else if ((Angle >= 135 && Angle < 225) && CurrentFloorMode != FloorMode.CEILING)
                {
                    // Now in Floor Mode
                    CurrentFloorMode = FloorMode.CEILING;

                }
                else if ((Angle >= 225 && Angle < 315) && CurrentFloorMode != FloorMode.LEFTWALL)
                {
                    // Now in Floor Mode
                    CurrentFloorMode = FloorMode.LEFTWALL;
                }
            }

        }

        private void AtrohpyHLock()
        {
            if (HLock > 0.0f)
            {
                HLock -= 1.0f;
            }
        }

        private void ApplyPosToParent()
        {
            // Apply to parent ent
           // XPos = (float)(int)XPos;
          //  YPos = (float)(int)YPos;
            Entity.X = XPos;
            Entity.Y = YPos;
        }

        private void CheckWallModeSpeed()
        {
            // Check speed for wall mode.
            if (Math.Abs(GroundSpeed) < 2.5 && (Angle > 60 && Angle < 360-60 ))
            {
                // We slipped off!
                if (CurrentFloorMode == FloorMode.CEILING)
                {
                    GroundSpeed = XSpeed;
                    
                }

                if (CurrentFloorMode != FloorMode.FLOOR)
                {
                    CurrentFloorMode = FloorMode.FLOOR;
                    CurrentMoveType = MoveType.AIR;
                    Angle = 0;
                }
                
                // Lock controls, prevent further movement for half a second.
                HLock = 30.0f;
            }

            
        }

        private void FloorModeWhenFalling()
        {
            if (CurrentMoveType == MoveType.AIR)
            {
                CurrentFloorMode = FloorMode.FLOOR;
            }
        }

        private void CalculateGroundSpeed()
        {
            GroundSpeed += SlopeFactor * -(float)Math.Sin(Angle * Math.PI / 180.0f);            
        }

        /// <summary>
        /// Updates the Movement.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Check left/right flip
            FlipLeftRight();

            // Check Underwater Status
            CheckUnderwater();

            // Slope factor is added to Ground Speed. This slows sonic when moving uphill, and speeds him up when moving downhill.
            CalculateGroundSpeed();

            // Check for braking
            CheckBraking();

            // Handle input
            HandleInput();

            // Check heights
            UpdateObjectHeight();

            // Check and change floor mode
            if (!Bumped)
            {
                ChangeFloorMode();
            }
            

            // Falling is always considered to be right side up.
            FloorModeWhenFalling();

            // Reset pop vals
            TotalPopX = 0;
            TotalPopY = 0;

            // Check walls
            Bumped = false;
            CheckWallSensor();

            // Make sure to fall off if wall mode speed is too slow
            CheckWallModeSpeed();

            // Check sensors for solid ground:
            CheckGroundSensors();

            // Set pop
            XPos += TotalPopX;
            YPos += TotalPopY;

            
            if (CurrentMoveType == MoveType.AIR)
            {
                // Check ceiling sensors
                CheckCeilingSensors();
            }

            // Ground Stuff
            if (CurrentMoveType != MoveType.AIR)
            {
                // Update Slope Factor
                UpdateSlopeFactor();

                // Do Speed check
                CalculateSpeedFromGroundSpeed();
                
            }


            // Spindash tick
            AtrophySpindashStrength();
            // HLock tick
            AtrohpyHLock();

            
            
            // Apply speeds to pos
            ApplySpeedToPos();

            // Apply pos to parent
            ApplyPosToParent();
                        
        }

        private void FlipLeftRight()
        {
            if(Braking)
            {
                return;
            }
            if (CurrentMoveType == MoveType.AIR)
            {
                if (XSpeed > 0)
                {
                    FacingRight = true;
                }
                if (XSpeed < 0)
                {
                    FacingRight = false;
                }
            }
            else
            {
                if (GroundSpeed > 0)
                {
                    FacingRight = true;
                }
                if (GroundSpeed < 0)
                {
                    FacingRight = false;
                }
            }
        }

        public void Jump()
        {
            // When sonic jumps, we need to make sure we jump perpendicular to the angle of travel.
            XSpeed -= JumpVelocity * (float)Math.Sin(Angle * Math.PI / 180.0f);
            YSpeed -= JumpVelocity * (float)Math.Cos(Angle * Math.PI / 180.0f);
            
            jumpSound.Play();
            Jumping = true;
            CurrentMoveType = MoveType.AIR;
            CurrentFloorMode = FloorMode.FLOOR; // switch to falling

        }

        // Sensor checks. [MESSY]

        public void CheckWallSensor()
        {
            // debug
            //return;
            if (!(CurrentFloorMode == FloorMode.FLOOR || CurrentMoveType == MoveType.AIR))
            {
                return;
            }
            
            // Check for tiles that are at the sides of sonic, relative to Y+4.
            wallSensor.APos = YPos + 1;
            
            // Left and Right edges are at +-10 on the X axis.
            wallSensor.BPos1 = XPos - 10;
            wallSensor.BPos2 = XPos + 10;

            wallSensor.moveRight = GroundSpeed > 0;

            Sensor.CollisionInfo colInfo = wallSensor.Sense(TileList, 1);

            if (!colInfo.thisIsNull && (colInfo.tileHit.myType == 411 || colInfo.tileHit.myType == 412 || colInfo.tileHit.myType == 413 || colInfo.tileHit.myType == 414) )
            {
                // Collision, cap'n!
                // Now, if the collision is on the left of sonic...
                if (colInfo.tileHit.X < XPos)
                {
                    // Pop sonic to the right by the requisite amount.
                    TotalPopX -= (XPos - (colInfo.tileHit.X + 16.0f + 10.0f));

                    Bumped = true;
                    
                    if(theController.Right.Down)
                    {
                        // Running away!
                        return;
                    }
                }
                else
                {
                    // Pop sonic to the left
                    TotalPopX -= (XPos - (colInfo.tileHit.X - 10.0f));

                    Bumped = true;

                    if (theController.Left.Down)
                    {
                        // Running away!
                        return;
                    }
                }

                // Set ground speed to 0 if pushing at it
                XSpeed = 0;
                GroundSpeed = 0;
            }

            

        }

        private void RegainGround()
        {
            // Prevent ground regain if we just clipped into the wall
            if(Bumped && Math.Abs(TotalPopX) > 1)
            {
                TotalPopY = 0;
                return;
            }
            if (CurrentMoveType == MoveType.AIR && YSpeed >= 0)
            {
                CurrentMoveType = MoveType.GROUND;
                Rolling = false;
                CanAirdash = true;

                // Account for sloping...
                if (Math.Abs(XSpeed) > YSpeed || Math.Abs(Angle) < 10f || Math.Abs(Angle) > 350f)
                {
                    GroundSpeed = XSpeed;
                }
                else
                {
                    GroundSpeed = YSpeed * -(float)Math.Sin(Angle * Math.PI / 180.0f);
                }

            }
        }

        public void CheckGroundSensors()
        {
            if(IgnoreFloor)
            {
                return;
            }

            groundSensorA.moveRight = GroundSpeed > 0;
            groundSensorB.moveRight = GroundSpeed > 0;

            // Check Mode.
            if(CurrentFloorMode == FloorMode.FLOOR)
            {
                // Sensor A: Positioned at -9, 0 to -9, 20.
                groundSensorA.APos = XPos - 9;
                groundSensorA.BPos1 = YPos + 0;
                groundSensorA.BPos2 = YPos + 16 + CurrentHeight;
                groundSensorA.verticalSensor = true;
                groundSensorA.keepCheck = false;

                // Sensor B: Positioned at 9, 0 to 9, 20.
                groundSensorB.APos = XPos + 9;
                groundSensorB.BPos1 = YPos + 0;
                groundSensorB.BPos2 = YPos + 16 + CurrentHeight;
                groundSensorB.verticalSensor = true;
                groundSensorB.keepCheck = false;
            }
            if (CurrentFloorMode == FloorMode.CEILING)
            {
                groundSensorA.APos = XPos + 9;
                groundSensorA.BPos1 = YPos - 16 - CurrentHeight;
                groundSensorA.BPos2 = YPos;
                groundSensorA.verticalSensor = true;
                groundSensorA.keepCheck = true;

                groundSensorB.APos = XPos - 9;
                groundSensorB.BPos1 = YPos - 16 - CurrentHeight;
                groundSensorB.BPos2 = YPos;
                groundSensorB.verticalSensor = true;
                groundSensorB.keepCheck = true;
            }
            if (CurrentFloorMode == FloorMode.RIGHTWALL)
            {
                // Sensor A: Positioned at -9, 0 to -9, 20.
                groundSensorA.APos = YPos + 9;
                groundSensorA.BPos1 = XPos + 0;
                groundSensorA.BPos2 = XPos + 16 + CurrentHeight;
                groundSensorA.verticalSensor = false;
                groundSensorA.keepCheck = false;

                // Sensor B: Positioned at 9, 0 to 9, 20.
                groundSensorB.APos = YPos - 9;
                groundSensorB.BPos1 = XPos + 0;
                groundSensorB.BPos2 = XPos + 16 + CurrentHeight;
                groundSensorB.verticalSensor = false;
                groundSensorB.keepCheck = false;
            }
            if (CurrentFloorMode == FloorMode.LEFTWALL)
            {
                // Sensor A: Positioned at -9, 0 to -9, 20.
                groundSensorA.APos = YPos - 9;
                groundSensorA.BPos1 = XPos - 16 - CurrentHeight;
                groundSensorA.BPos2 = XPos + 0;
                groundSensorA.verticalSensor = false;
                groundSensorA.keepCheck = true;

                // Sensor B: Positioned at 9, 0 to 9, 20.
                groundSensorB.APos = YPos + 9;
                groundSensorB.BPos1 = XPos - 16 - CurrentHeight;
                groundSensorB.BPos2 = XPos + 0;
                groundSensorB.verticalSensor = false;
                groundSensorB.keepCheck = true;
            }

            // The tiles that will be located.
            Tile sensorATile = null;
            Tile sensorBTile = null;

            // Sense those tiles!
            int senseMode = 0;
            if(CurrentFloorMode == FloorMode.FLOOR)
            {
                senseMode = 0;
            }
            if(CurrentFloorMode == FloorMode.RIGHTWALL)
            {
                senseMode = 1;
            }
            if (CurrentFloorMode == FloorMode.CEILING)
            {
                senseMode = 2;
            }
            if (CurrentFloorMode == FloorMode.LEFTWALL)
            {
                senseMode = 3;
            }
            Sensor.CollisionInfo colInfoA = groundSensorA.Sense(TileList, senseMode);
            Sensor.CollisionInfo colInfoB = groundSensorB.Sense(TileList, senseMode);

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

                // FLOOR AND CEILING MODES:
                if(CurrentFloorMode == FloorMode.FLOOR || CurrentFloorMode == FloorMode.CEILING)
                {
                    if (sensorATile != null)
                    {
                        // Capture sensor A's result.
                        int heightMapArrayIndex = (int)groundSensorA.APos - (int)sensorATile.X;

                        heightMapArrayIndex = Math.Min(heightMapArrayIndex, 15);

                        heightOfA = groundSensorA.lastHeightHit;

                        fullheightOfA = heightOfA + (Global.maxlvlheight - (int)sensorATile.Y);

                        angleOfA = sensorATile.myTileInfo.Angle;

                        // If the tile is empty of collision, don't collide with it. Duh!
                        if (sensorATile.myTileInfo.flatheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                        {
                            sensorATile = null;
                        }

                    }
                    if (sensorBTile != null)
                    {
                        // Capture sensor B's result.
                        int heightMapArrayIndex = (int)groundSensorB.APos - (int)sensorBTile.X;

                        heightMapArrayIndex = Math.Min(heightMapArrayIndex, 15);

                        heightOfB = groundSensorB.lastHeightHit;



                        fullheightOfB = heightOfB + (Global.maxlvlheight - (int)sensorBTile.Y);

                        angleOfB = sensorBTile.myTileInfo.Angle;

                        // If the tile is empty of collision, don't collide with it. Duh!
                        if (sensorBTile.myTileInfo.flatheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                        {
                            sensorBTile = null;
                        }
                    }
                }

                // WALL MODES:
                if (CurrentFloorMode == FloorMode.RIGHTWALL || CurrentFloorMode == FloorMode.LEFTWALL)
                {
                    if (sensorATile != null)
                    {
                        // Capture sensor A's result.
                        int heightMapArrayIndex = (int)groundSensorA.APos - (int)sensorATile.Y;
                        
                        heightMapArrayIndex = Math.Min(heightMapArrayIndex, 15);

                        heightOfA = groundSensorA.lastHeightHit;


                        fullheightOfA = heightOfA + (Global.maxlvlwidth - (int)sensorATile.X);


                        angleOfA = sensorATile.myTileInfo.Angle;

                        // If the tile is empty of collision, don't collide with it. Duh!
                        if (sensorATile.myTileInfo.wallheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                        {
                            sensorATile = null;
                        }

                    }
                    if (sensorBTile != null)
                    {
                        // Capture sensor B's result.
                        int heightMapArrayIndex = (int)groundSensorB.APos - (int)sensorBTile.Y;

                        heightMapArrayIndex = Math.Min(heightMapArrayIndex, 15);

                        heightOfB = groundSensorB.lastHeightHit;

                        fullheightOfB = heightOfB + (Global.maxlvlwidth - (int)sensorBTile.X);

                        angleOfB = sensorBTile.myTileInfo.Angle;

                        // If the tile is empty of collision, don't collide with it. Duh!
                        if (sensorBTile.myTileInfo.wallheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                        {
                            sensorBTile = null;
                        }
                    }
                }

                if ((sensorATile == null && sensorBTile == null))
                {
                    // We didn't collide with anything. WE'RE FALLING AAARGH
                    CurrentMoveType = MoveType.AIR;
                    //CurrentFloorMode = FloorMode.FLOOR;
                    return;
                }

              float AngleLastFrame = Angle;

                // Pop from tiles walked on.
                    //Flip fullheights for left and ceiling modes
                    if(CurrentFloorMode == FloorMode.LEFTWALL || CurrentFloorMode == FloorMode.CEILING)
                    {
                        int tmp = fullheightOfA;
                        fullheightOfA = fullheightOfB;
                        fullheightOfB = tmp;

                    }
                    if (fullheightOfA >= fullheightOfB && sensorATile != null)
                    {
                        if(CurrentFloorMode == FloorMode.FLOOR || CurrentFloorMode == FloorMode.CEILING)
                        {
                            if ((CurrentMoveType == MoveType.AIR && YSpeed > 0) || CurrentMoveType == MoveType.GROUND)
                            {
                                if (CurrentMoveType == MoveType.AIR)
                                {
                                    // In air mode, we only stick to the ground if we are below the new pos
                                    if(YPos >= sensorATile.Y + 16 - heightOfA - CurrentHeight)
                                    {
                                        TotalPopY -= YPos - (sensorATile.Y + (16 - heightOfA) - 20);
                                        RegainGround();
                                    }


                                  
                                }
                                else
                                {
                                    TotalPopY -= YPos - (sensorATile.Y + 16 - heightOfA - CurrentHeight);
                                    RegainGround();
                                }
                                if(CurrentFloorMode == FloorMode.CEILING)
                                {
                                    TotalPopY += YPos - (sensorATile.Y + 16 - heightOfA - CurrentHeight);

                                    RegainGround();
                                }
                                
                            }
                            
                        }
                        else
                        {
                            TotalPopX -= XPos - ((sensorATile.X + 16) - (heightOfA + CurrentHeight));
                            if(CurrentFloorMode == FloorMode.LEFTWALL)
                            {
                                TotalPopX += XPos - ((sensorATile.X + 16) - (heightOfA + CurrentHeight));
                            }
                        
                        }
                        Angle = angleOfA;
                    
                    }
                    else if(sensorBTile != null)
                    {
                        if (CurrentFloorMode == FloorMode.FLOOR || CurrentFloorMode == FloorMode.CEILING)
                        {
                         if ((CurrentMoveType == MoveType.AIR && YSpeed > 0) || CurrentMoveType == MoveType.GROUND)
                            {
                                if (CurrentMoveType == MoveType.AIR)
                                {
                                    // In air mode, we only stick to the ground if we are below the new pos
                                    if (YPos >= sensorBTile.Y + 16 - heightOfB - CurrentHeight)
                                    {
                                        TotalPopY -= YPos - (sensorBTile.Y + 16 - heightOfB - 20);
                                        RegainGround();
                                    }
              
                                }
                                else
                                {
                                    TotalPopY -= YPos - (sensorBTile.Y + 16 - heightOfB - CurrentHeight);
                                    RegainGround();
                                }
                                if(CurrentFloorMode == FloorMode.CEILING)
                                {
                                    TotalPopY += YPos - (sensorBTile.Y + 16 - heightOfB - CurrentHeight); 
                            }
                            
                            }
                            
                        }
                        else
                        {
                        TotalPopX -= XPos - ((sensorBTile.X + 16) - (heightOfB + CurrentHeight));
                        if (CurrentFloorMode == FloorMode.LEFTWALL)
                            {
                                TotalPopX += XPos - ((sensorBTile.X + 16) - (heightOfB + CurrentHeight));
                            }
                           
                        }
                        Angle = angleOfB;
                    }



                   
                

            }


            return;
            
        }

        public void HandleInput()
        {
            // Get ref to controller
            //NotSonic.System.SegaController theController = Global.playerSession.GetController<NotSonic.System.SegaController>();

            /// Check d-pad.
            /// GROUND:
            if (CurrentMoveType == MoveType.GROUND)
            {
                /// RUNNING & Not Spindashing
                if (!(CurrentSpindashStrength > 0.0f && (theController.Left.Down || theController.Right.Down)))
                {
                    if (!Rolling)
                    {
                        if (theController.Left.Down)
                        {
                            if(!Braking)
                            {
                                FacingRight = false;
                            }
                            
                            if (HLock <= 0.0f)
                            {
                                if (GroundSpeed > 0) //Heading right, now going left
                                {
                                    GroundSpeed -= Deceleration;
                                }
                                else if (GroundSpeed > -TopSpeed)
                                {
                                    // Zoom!
                                    GroundSpeed -= Acceleration;
                                    if(GroundSpeed < -TopSpeed)
                                    {
                                        GroundSpeed = -TopSpeed;
                                    }
                                }
                            }

                        }
                        else if (theController.Right.Down)
                        {
                            if (!Braking)
                            {
                                FacingRight = true;
                            }
                            if (HLock <= 0.0f)
                            {
                                if (GroundSpeed < 0)
                                {
                                    GroundSpeed += Deceleration;
                                }
                                else if (GroundSpeed < TopSpeed)
                                {
                                    GroundSpeed += Acceleration;
                                    if(GroundSpeed > TopSpeed)
                                    {
                                        GroundSpeed = TopSpeed;
                                    }
                                }
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

                            if (!Braking)
                            {
                                FacingRight = false;
                            }
                            if (HLock <= 0.0f)
                            {
                                if (GroundSpeed > 0) //Heading right, now going left
                                {
                                    GroundSpeed -= 0.125f;
                                }
                            }

                        }
                        else if (theController.Right.Down)
                        {
                            if (!Braking)
                            {
                                FacingRight = true;
                            }
                            if (HLock <= 0.0f)
                            {
                                if (GroundSpeed < 0)
                                {
                                    GroundSpeed += 0.125f;
                                }
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
                        revSound.Stop();
                        revSound.Pitch = 1.0f + (CurrentSpindashStrength / MaxSpindashStrength) / 1.2f;
                        revSound.Play();
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
                    rollingSound.Play();
                }

                // Spindash release
                if (theController.Down.Released && CurrentSpindashStrength > 0.0f)
                {
                    revSound.Stop();
                    dashGoSound.Play();
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
                if (theController.Left.Down)
                {
                    if (!Braking)
                    {
                        FacingRight = false;
                    }
                 //   if (HLock <= 0.0f)
                //    {
                        XSpeed -= AirAccel;
                //    }
                }
                else if (theController.Right.Down)
                {
                    if (!Braking)
                    {
                        FacingRight = true;
                    }
                //    if (HLock <= 0.0f)
                //    {
                        XSpeed += AirAccel;
                //    }
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

                // Air Curl (Megamix)
                if(!Rolling && theController.A.Pressed)
                {
                    Rolling = true;
                }

                // Air Dash (Megamix)
                if(CanAirdash && Rolling && (theController.B.Pressed || theController.C.Pressed))
                {
                    //Launch forwards!
                    CanAirdash = false;
                    XSpeed = DashSpeed * ((FacingRight) ? 1 : -1);
                    YSpeed = 0;
                    dashGoSound.Play();
                }


            }
        }

        private void DrawDebugView()
        {
            if (!DebugView)
            {
                return;
            }

            groundSensorA.DrawSelf(Color.Red);
            groundSensorB.DrawSelf(Color.Green);
            //ceilingSensorC.DrawSelf(Color.Yellow);
            //ceilingSensorD.DrawSelf(Color.Magenta);
            wallSensor.DrawSelf(Color.Cyan);

            // Draw pop lines
            // Push newest pop stuff on
            if(TotalPopX != 0 || TotalPopY != 0)
            {
                DebugPopLineBuffer.Add(new KeyValuePair<Vector2, Vector2>(new Vector2(XPos, XPos - TotalPopX), new Vector2(YPos, YPos - TotalPopY)));
            }
            
            if(DebugPopLineBuffer.Count > 60)
            {
                DebugPopLineBuffer.RemoveAt(0);

            }
            foreach (KeyValuePair<Vector2, Vector2> kv in DebugPopLineBuffer)
            {

                Draw.Line(kv.Key.X, kv.Value.X, kv.Key.Y, kv.Value.X, Color.Magenta, 3);
                Draw.Line(kv.Key.X, kv.Value.X, kv.Key.X, kv.Value.Y, Color.Yellow, 3);

            }

            Text newText = new Text("Angle: " + Angle.ToString() + " | Sensor A Height: " + groundSensorA.lastHeightHit.ToString() + " | NumTiles: " + TileList.Count.ToString(), 10);
            newText.Smooth = false;
            newText.Scroll = 0;
            newText.Render(0, 0);
        }

        public override void Render()
        {
            base.Render();
            DrawDebugView();
            return;
        }

        private void CheckCeilingSensors()
        {
            ceilingSensorC.APos = XPos + 9;
            ceilingSensorC.BPos1 = YPos - 16 - CurrentHeight;
            ceilingSensorC.BPos2 = YPos;
            ceilingSensorC.verticalSensor = true;
            ceilingSensorC.keepCheck = true;

            ceilingSensorD.APos = XPos - 9;
            ceilingSensorD.BPos1 = YPos - 16 - CurrentHeight;
            ceilingSensorD.BPos2 = YPos;
            ceilingSensorD.verticalSensor = true;
            ceilingSensorD.keepCheck = true;
            
            if(YSpeed <= 0)
            {
                // The tiles that will be located.
                Tile sensorATile = null;
                Tile sensorBTile = null;

                // Sense those tiles!
                int senseMode = 0;
                if (CurrentFloorMode == FloorMode.FLOOR)
                {
                    senseMode = 0;
                }
                if (CurrentFloorMode == FloorMode.RIGHTWALL)
                {
                    senseMode = 1;
                }
                if (CurrentFloorMode == FloorMode.CEILING)
                {
                    senseMode = 2;
                }
                if (CurrentFloorMode == FloorMode.LEFTWALL)
                {
                    senseMode = 3;
                }
                //senseMode = 2;
                Sensor.CollisionInfo colInfoA = ceilingSensorC.Sense(TileList, senseMode);
                Sensor.CollisionInfo colInfoB = ceilingSensorD.Sense(TileList, senseMode);

                sensorATile = colInfoA.tileHit;
                sensorBTile = colInfoB.tileHit;

                float heightOfA = 0.0f, fullheightOfA = 0.0f, heightOfB = 0.0f, fullheightOfB = 0.0f;

                if (sensorATile != null)
                {
                    // Capture sensor A's result.
                    int heightMapArrayIndex = (int)groundSensorA.APos - (int)sensorATile.X;

                    heightOfA = ceilingSensorC.lastHeightHit;

                    fullheightOfA = heightOfA + (Global.maxlvlheight - (int)sensorATile.Y);

                    // If the tile is empty of collision, don't collide with it. Duh!
                    if (sensorATile.myTileInfo.flatheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                    {
                        sensorATile = null;
                    }

                }
                if (sensorBTile != null)
                {
                    // Capture sensor B's result.
                    int heightMapArrayIndex = (int)groundSensorB.APos - (int)sensorBTile.X;

                    heightOfB = ceilingSensorD.lastHeightHit;

                    fullheightOfB = heightOfB + (Global.maxlvlheight - (int)sensorBTile.Y);

                    // If the tile is empty of collision, don't collide with it. Duh!
                    if (sensorBTile.myTileInfo.flatheightArray == HeightArrays.HEIGHT_ARRAY_EMPTY)
                    {
                        sensorBTile = null;
                    }
                }

                // Do Collision response
                if(fullheightOfA >= fullheightOfB && sensorATile != null)
                {
                    if(YPos - 20 < sensorATile.Y + heightOfA)
                    {
                        TotalPopY -= YPos - (sensorATile.Y + heightOfA + CurrentHeight + 1);
                        if (((sensorATile.myTileInfo.Angle > 90 && sensorATile.myTileInfo.Angle < 135) || (sensorATile.myTileInfo.Angle > 225 && sensorATile.myTileInfo.Angle < 270)))
                        {
                            //re-attach
                            TotalPopY += 2;
                            Angle = sensorATile.myTileInfo.Angle;
                            CurrentFloorMode = FloorMode.CEILING;
                            Jumping = false;
                            CurrentMoveType = MoveType.GROUND;
                            GroundSpeed = YSpeed * -Math.Sign(Math.Cos(Angle));
                            
                        }
                        else
                        {
                            TotalPopY += 2;
                            YSpeed = 0;
                            Jumping = false;
                         
                        }
                    }
                }
                else if (sensorBTile != null)
                {
                    if(YPos - 20 < sensorBTile.Y + heightOfB)
                    {
                        TotalPopY -= YPos - (sensorBTile.Y + heightOfB + CurrentHeight + 1);
                        if (((sensorBTile.myTileInfo.Angle > 90 && sensorBTile.myTileInfo.Angle < 135) || (sensorBTile.myTileInfo.Angle > 225 && sensorBTile.myTileInfo.Angle < 270)))
                        {
                            //re-attach

                            TotalPopY += 2;
                            Angle = sensorBTile.myTileInfo.Angle;
                            CurrentFloorMode = FloorMode.CEILING;
                            Jumping = false;
                            CurrentMoveType = MoveType.GROUND;
                            GroundSpeed = YSpeed * -Math.Sign(Math.Cos(Angle));
                            
                        }
                        else
                        {
                            TotalPopY += 2;
                            //fall
                            YSpeed = 0;
                            Jumping = false;
                            
                        }
                    }
                }

            }

        }

        #endregion

 
        public void LerpNet(float LerpAmt)
        {
            
            XPos = Util.Lerp(XPos, DXPos, LerpAmt);
            YPos = Util.Lerp(YPos, DYPos, LerpAmt);
            XSpeed = Util.Lerp(XSpeed, DXSpeed, LerpAmt);
            YSpeed = Util.Lerp(YSpeed, DYSpeed, LerpAmt);
            GroundSpeed = Util.Lerp(GroundSpeed, DGroundSpeed, LerpAmt);
            Angle = Util.Lerp(Angle, DAngle, LerpAmt);
        }

    }

    
    

}
