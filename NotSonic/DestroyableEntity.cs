﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace NotSonic
{
    class DestroyableEntity : Entity
    {
        // Pretty much the same as any other ent but it can be destroyed. Has ref to player.

        public NotSonic.Entities.SonicPlayer thePlayer = null;
        public float CollisionWidth = 0;
        public float CollisionHeight = 0;
        public bool Destroyed = false;
        public bool FinishedDestroy = false;
        public float DestroyTime = 0;
        public Sound OrchHitSound = new Sound(Assets.SND_ORCH);
        public Sound PopSound = new Sound(Assets.SND_POP);
        Sound warpSound = new Sound(Assets.SND_WARP);
        public DestroyableEntity(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
            this.Collider = new BoxCollider((int)CollisionWidth, (int)CollisionHeight, new int[] { 0 });
            this.Collider.CenterOrigin();
        }

        public override void Update()
        {
            if(Graphic != null)
            {
                Graphic.CenterOrigin();
            }
            base.Update();

            // Check for player entity.
            if(thePlayer != null)
            {
                CheckForPlayer();
            }
        }

        public override void UpdateLast()
        {
            base.UpdateLast();
            if(Destroyed && !FinishedDestroy && DestroyTime < 1)
            {
                if (Graphic != null)
                {
                    Graphic.Shader = Global.paleShader;
                }
                DestroyTime += 1;
                Layer = 21;
                return;
            }
            if(DestroyTime > 0 && !FinishedDestroy)
            {
                GetDestroyed();
                FinishedDestroy = true;
                return;
            }

            if(FinishedDestroy)
            {
                RemoveSelf();
            }

            
        }

        void CheckForPlayer()
        {
            // Check for a collision w/ this player.
            bool outsideBounds = this.Collider.Collide(X, Y, thePlayer) == null;
            if (!outsideBounds)
            {
                if (thePlayer.myMovement.Rolling && !Destroyed)
                {
                    // Add to player combo.
                    thePlayer.comboAmt += 1;
                    thePlayer.comboTime = 60.0f;
                    thePlayer.myMovement.CanAirdash = true;

                    
                    MessageEvent msg = new MessageEvent();
                    msg.myType = "DESTROYED";
                    Global.eventList.Add(msg);
                    Destroyed = true;



                    // Rebound! (Dont want to rebound if we're rolling down a hill)
                    if(thePlayer.myMovement.CurrentMoveType == NotSonic.Components.SonicMovement.MoveType.AIR)
                    {
                        if (thePlayer.Y > Y || thePlayer.myMovement.YSpeed < 0)
                        {
                            thePlayer.myMovement.YSpeed -= 1.0f * (float)Math.Sign(thePlayer.myMovement.YSpeed);
                        }
                        else if (thePlayer.Y < Y && thePlayer.myMovement.YSpeed > 0)
                        {
                            if (Global.theController.A.Down || Global.theController.B.Down || Global.theController.C.Down)
                            {
                                thePlayer.myMovement.YSpeed *= -1;
                            }
                            else
                            {
                                thePlayer.myMovement.YSpeed *= Math.Max(-1.0f, thePlayer.myMovement.YSpeed * -1.0f);
                            }
                        }
                    }

                    // Accelerate!!
                    if(thePlayer.comboAmt > 5)
                    {
                        thePlayer.myMovement.XSpeed *= (1.0f + 0.005f * thePlayer.comboAmt);
                        thePlayer.myMovement.YSpeed *= (1.0f + 0.002f * thePlayer.comboAmt);
                    }
                }

                


            }
        }

        public void GetDestroyed()
        {
            // Do something here.
            if (Graphic != null)
            {
                
                if(thePlayer.comboAmt > 2)
                {
                    OrchHitSound.Pitch = Math.Min(1.0f + ((thePlayer.comboAmt - 3) * 0.05f), 10.0f);
                    OrchHitSound.Play();
                    float popPitch = 1.0f - (thePlayer.comboAmt * 0.05f);

                    if (popPitch < 0.1f)
                    {
                        popPitch = 0.1f;
                    }
                    PopSound.Pitch = popPitch;
                    PopSound.Play();

                    if(thePlayer.comboAmt == 20)
                    {
                        // Play a warp noise
                        warpSound.Play();
                    }

                }
                else
                {
                    PopSound.Play();
                }
                
                
                Graphic.Visible = false;
            }
        }
    }
}
