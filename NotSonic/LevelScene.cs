using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using TiledSharp;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [NotSonic] Project.
// Date: 2015/05/30
//----------------
// Purpose: This is the main gameplay state. This is constructed from level information.

namespace NotSonic
{
    
    class LevelScene : Scene
    {
        // The Player
        NotSonic.Entities.SonicPlayer thePlayer;
        List<NotSonic.Entities.SonicPlayer> thePlayers;

        // List of Tiles
        List<NotSonic.Components.Tile> tileList;
        // List of Loops
        List<Loop> loopList = new List<Loop>();

        // Map Name
        public string mapName;
        public string friendlyMapName;

        // Title card
        public TitleCard theTitleCard;
        
        // Tiled TMX Loader instance
        public TmxMap tmxMapData;
        Tilemap tilemap;
        Tilemap bgtilemap;
        Tilemap closeTilemap;
        Tilemap collisionmap;

        // Freezelock
        bool freezeLocked = false;
        float freezeLockTime = 0.0f;

        // Water level
        bool hasWater = true;
        float waterLevel = 480.0f;

        // Slomo for testing
        bool sloMo = false;
        
        // Ring Count
        public int Rings = 0;

        // Ring sound emitter?
        Sound ringSound = new Sound(Assets.SND_RING, false);

        // Shader fun
        Shader LUTShade = new Shader(ShaderType.Fragment, Assets.LUTSHADER);
        Image LUTTable = new Image(Assets.LUTIMAGE);
        Shader DarkenShader = new Shader(ShaderType.Fragment, Assets.DARKSHADER);

        // Mushroom Hill Dot M P 3
        Music mushroomHillMusic = new Music(Assets.MUS_MUSH);
        Sound deadSound = new Sound(Assets.SND_DEAD);

        // The camera shaking stuff
        CameraShaker theCamShaker = new CameraShaker();

        public LevelScene(string mapFilename)
        {

            // Load the height maps
            HeightArrays.GenerateHeightMap();

            // Get map name
            mapName = mapFilename;
            
            // Load the map
            LoadMap();
            SetProperties();

            this.UseCameraBounds = true;
            this.ApplyCamera = true;

            // Set the camera bounds based on the map size.
            this.CameraBounds.X = 0;
            this.CameraBounds.Y = 0;
            this.CameraBounds.Width = 16 * tmxMapData.Width;
            this.CameraBounds.Height = 16 * tmxMapData.Height;


            // Make da player!!
            // Use the Objects layer, with the position of the SonicStart object.
            CreateObjects();

                      
            
            
            // Create the parallax bg bits using tmx data
            
            Image para_Far = new Image(Assets.GetAsset(tmxMapData.ImageLayers["ParaFar"].Properties["EngineName"]));
            para_Far.RepeatX = true;
            para_Far.ScrollY = tmxMapData.ImageLayers["ParaFar"].Properties.ValueAsFloat("ScrollY");
            para_Far.ScrollX = tmxMapData.ImageLayers["ParaFar"].Properties.ValueAsFloat("ScrollX");
            para_Far.Y = tmxMapData.ImageLayers["ParaFar"].Y;
            AddGraphic(para_Far);

            Image para_Mid2 = new Image(Assets.GetAsset(tmxMapData.ImageLayers["ParaNear"].Properties["EngineName"]));
            para_Mid2.RepeatX = true;
            para_Mid2.ScrollY = tmxMapData.ImageLayers["ParaNear"].Properties.ValueAsFloat("ScrollY") * 0.6f;
            para_Mid2.ScrollX = tmxMapData.ImageLayers["ParaNear"].Properties.ValueAsFloat("ScrollX") * 0.6f;
            para_Mid2.Scale = 0.6f;
            para_Mid2.Color = Color.Gray * 0.5f;
            para_Mid2.Y = tmxMapData.ImageLayers["ParaNear"].Y + 120.0f;
            AddGraphic(para_Mid2);

            Image para_Mid = new Image(Assets.GetAsset(tmxMapData.ImageLayers["ParaNear"].Properties["EngineName"]));
            para_Mid.RepeatX = true;
            para_Mid.ScrollY = tmxMapData.ImageLayers["ParaNear"].Properties.ValueAsFloat("ScrollY") * 0.8f;
            para_Mid.ScrollX = tmxMapData.ImageLayers["ParaNear"].Properties.ValueAsFloat("ScrollX") * 0.8f;
            para_Mid.Scale = 0.8f;
            
            para_Mid.Color = Color.Gray;
            para_Mid.Y = tmxMapData.ImageLayers["ParaNear"].Y + 64.0f;
            AddGraphic(para_Mid);

            Image para_Close = new Image(Assets.GetAsset(tmxMapData.ImageLayers["ParaNear"].Properties["EngineName"]));
            para_Close.RepeatX = true;
            para_Close.ScrollY = tmxMapData.ImageLayers["ParaNear"].Properties.ValueAsFloat("ScrollY");
            para_Close.ScrollX = tmxMapData.ImageLayers["ParaNear"].Properties.ValueAsFloat("ScrollX");
            para_Close.Y = tmxMapData.ImageLayers["ParaNear"].Y;
            AddGraphic(para_Close);


            AddGraphic(bgtilemap);

            AddGraphic(tilemap);

            AddGraphic(collisionmap);
            
            Entity abovePlayerMap = new Entity(0, 0, closeTilemap);
            abovePlayerMap.Layer = thePlayer.Layer - 1;
            Add(abovePlayerMap);

            foreach( NotSonic.Entities.SonicPlayer ply in thePlayers)
            {
                Add(ply);
                
            }

            
                        
            Add(theCamShaker);


            // Create title card
            theTitleCard = new TitleCard(friendlyMapName);
            Add(theTitleCard);





            Otter.Debugger.Instance.ShowPerformance(5);

            // Make debug commands, and shaders

            Global.theGame.Surface.Shader = LUTShade;
            Otter.Debugger.CommandFunction myFunc = new Debugger.CommandFunction(Impulse);
            Otter.CommandType[] cmdArgs = new Otter.CommandType[1];
            cmdArgs[0] = CommandType.Int;
            Debugger.Instance.RegisterCommand("i", myFunc, cmdArgs);
        }


        public void LoadMap()
        {
            // Load map using the map loader.
            tmxMapData = new TmxMap(mapName);

            // Load Level from Tiled map.
            tileList = new List<Components.Tile>();
            tilemap = new Tilemap(Assets.TILE_SHEET, tmxMapData.Height * 16, 16);
            bgtilemap = new Tilemap(Assets.TILE_SHEET, tmxMapData.Height * 16, 16);
            closeTilemap = new Tilemap(Assets.TILE_SHEET, tmxMapData.Height * 16, 16);
            collisionmap = new Tilemap(Assets.TILE_SHEET_ANGLES, tmxMapData.Height * 16, 16);
            Global.maxlvlheight = tmxMapData.Height * 16;
            Global.maxlvlwidth = tmxMapData.Width * 16;
            tilemap.AddLayer("vis", 19);
            tilemap.UsePositions = true;
            closeTilemap.AddLayer("vis", 19);
            closeTilemap.UsePositions = true;
            bgtilemap.AddLayer("vis", 19);
            bgtilemap.UsePositions = true;
            collisionmap.AddLayer("vis", 19);
            collisionmap.UsePositions = true;

            // For each tile in the Solid layer, we want to create a Tile object, and use the relevant image.
            for (int i = 0; i < tmxMapData.Layers["Foreground"].Tiles.Count; i++)
            {
                closeTilemap.SetTile(tmxMapData.Layers["Foreground"].Tiles[i].X * 16, tmxMapData.Layers["Foreground"].Tiles[i].Y * 16, tmxMapData.Layers["Foreground"].Tiles[i].Gid - 1, "base");
                if (tmxMapData.Layers["Foreground"].Tiles[i].Gid != 0)
                {
                    closeTilemap.SetTile(tmxMapData.Layers["Foreground"].Tiles[i].X * 16, tmxMapData.Layers["Foreground"].Tiles[i].Y * 16, tmxMapData.Layers["Foreground"].Tiles[i].HorizontalFlip, tmxMapData.Layers["Foreground"].Tiles[i].VerticalFlip);
                }
                
            }
            for (int i = 0; i < tmxMapData.Layers["Background"].Tiles.Count; i++)
            {
                bgtilemap.SetTile(tmxMapData.Layers["Background"].Tiles[i].X * 16, tmxMapData.Layers["Background"].Tiles[i].Y * 16, tmxMapData.Layers["Background"].Tiles[i].Gid - 1, "base");
                if (tmxMapData.Layers["Background"].Tiles[i].Gid != 0)
                {
                    bgtilemap.SetTile(tmxMapData.Layers["Background"].Tiles[i].X * 16, tmxMapData.Layers["Background"].Tiles[i].Y * 16, tmxMapData.Layers["Background"].Tiles[i].HorizontalFlip, tmxMapData.Layers["Background"].Tiles[i].VerticalFlip);
                }

            }
            for (int i = 0; i < tmxMapData.Layers["Solid_Height"].Tiles.Count; i++)
            {
                collisionmap.SetTile(tmxMapData.Layers["Solid_Height"].Tiles[i].X * 16, tmxMapData.Layers["Solid_Height"].Tiles[i].Y * 16, tmxMapData.Layers["Solid_Height"].Tiles[i].Gid - tmxMapData.Tilesets["Test_Height"].FirstGid, "base");
                if (tmxMapData.Layers["Solid_Height"].Tiles[i].Gid != 0)
                {
                    collisionmap.SetTile(tmxMapData.Layers["Solid_Height"].Tiles[i].X * 16, tmxMapData.Layers["Solid_Height"].Tiles[i].Y * 16, tmxMapData.Layers["Solid_Height"].Tiles[i].HorizontalFlip, tmxMapData.Layers["Solid_Height"].Tiles[i].VerticalFlip);
                }

            }
            for (int i = 0; i < tmxMapData.Layers["Solid"].Tiles.Count; i++)
            {
                // Set otter tile
                tilemap.SetTile(tmxMapData.Layers["Solid"].Tiles[i].X * 16, tmxMapData.Layers["Solid"].Tiles[i].Y * 16, tmxMapData.Layers["Solid"].Tiles[i].Gid - 1, "base");

                if (tmxMapData.Layers["Solid"].Tiles[i].Gid != 0)
                {
                    // Heightmap Data
                    // In the Solid_Height layer, we find the appropriate tile type. Angles are included in this.
                    int heightMapID = tmxMapData.Layers["Solid_Height"].Tiles[i].Gid;
                    bool heightFlipX = tmxMapData.Layers["Solid_Height"].Tiles[i].HorizontalFlip;
                    bool heightFlipY = tmxMapData.Layers["Solid_Height"].Tiles[i].VerticalFlip;
                    if (heightMapID >= tmxMapData.Tilesets["Test_Height"].FirstGid)
                    {
                        heightMapID = tmxMapData.Layers["Solid_Height"].Tiles[i].Gid - tmxMapData.Tilesets["Test_Height"].FirstGid;
                    }
                    else
                    {
                        heightMapID = 0;
                    }


                    // Create tile.
                    NotSonic.Components.Tile newTile = new NotSonic.Components.Tile(tmxMapData.Layers["Solid"].Tiles[i].X * 16, tmxMapData.Layers["Solid"].Tiles[i].Y * 16, heightMapID, heightFlipX, heightFlipY);

                    tilemap.SetTile(tmxMapData.Layers["Solid"].Tiles[i].X * 16, tmxMapData.Layers["Solid"].Tiles[i].Y * 16, tmxMapData.Layers["Solid"].Tiles[i].HorizontalFlip, tmxMapData.Layers["Solid"].Tiles[i].VerticalFlip);


                    float tileAngle = tmxMapData.Tilesets["Test_Height"].Tiles[heightMapID].Properties.ValueAsFloat("Angle");

                    // Set the tile's graphic properly.
                    newTile.tileImage.Frame = tmxMapData.Layers["Solid"].Tiles[i].Gid - 1;
                    newTile.tileImage.FlippedX = tmxMapData.Layers["Solid"].Tiles[i].HorizontalFlip;
                    newTile.tileImage.FlippedY = tmxMapData.Layers["Solid"].Tiles[i].VerticalFlip;
                    newTile.myTileInfo.Angle = tileAngle;



                    newTile.Layer = 19;

                    tileList.Add(newTile);
                }

            }
            
        }

        public override void UpdateFirst()
        {
            base.UpdateFirst();

            foreach(NotSonic.Entities.SonicPlayer ply in thePlayers)
            {
                if (ply != thePlayer)
                {
                    ply.myController.UpdateFirst();
                }
            }
        }

        public override void Update()
        {
            // Freezelock with title card
            if(theTitleCard.Waited == false)
            {
                freezeLockTime = 1.0f;
            }

            // Process messages first.
            ProcessMessages();

            // Check freezelock
            CheckFreezeLock();

            
            // Check slomo
            if(sloMo && Math.Floor(Game.Timer) % 2 == 0)
            {
                freezeLocked = true;
                freezeLockTime = 1;
            }

            foreach(NotSonic.Entities.SonicPlayer ply in thePlayers)
            {

                if (ply.myMovement.YPos > Global.maxlvlheight)
                {
                    // DEAD!!
                    Otter.Debugger.Instance.Log("Player Death.");
                    ply.myMovement.XPos = 64;
                    ply.myMovement.YPos = 64;
                    ply.myMovement.XSpeed = 0;
                    ply.myMovement.YSpeed = 0;
                    ply.myMovement.GroundSpeed = 0;
                    deadSound.Play();


                }


                // Prep the tileList based on the player's position and speed.
                if (ply.myMovement.TileList == null)
                {
                    ply.myMovement.TileList = new List<NotSonic.Components.Tile>();
                }

                ply.myMovement.TileList.Clear();
                float checkRadius = 48 + Math.Max(ply.myMovement.XSpeed, ply.myMovement.YSpeed);
                foreach (NotSonic.Components.Tile tile in tileList)
                {
                    float curX, curY;
                    curX = tile.X - ply.X;
                    curY = tile.Y - ply.Y;
                    if (curX * curX + curY * curY < checkRadius * checkRadius)
                    {
                        ply.myMovement.TileList.Add(tile);
                    }
                }

                
                
                

                // Check if player is underwater
                if (ply.Y > waterLevel)
                {
                    if (ply.myMovement.Underwater == false)
                    {
                        ply.myMovement.EnterWater();
                    }
                }
                else
                {
                    if (ply.myMovement.Underwater == true)
                    {
                        ply.myMovement.ExitWater();
                    }

                }
            }

            thePlayer.myMovement.groundSensorA.CollisionTilemap = collisionmap;

            base.Update();
        }

        public override void UpdateLast()
        {
            base.UpdateLast();

            // Recentering Camera Based on Player Position & Direction

            float targetCamX = this.CameraCenterX;
            float targetCamY = this.CameraCenterY;


            float distToPlayer = Math.Abs(targetCamX - thePlayer.X);
            float amtExceeded = 16 - distToPlayer;
            // X- Cam
            if(distToPlayer > 16)
            {
                targetCamX -= Math.Min(Math.Abs(amtExceeded * Math.Sign(targetCamX - thePlayer.X)), 16.0f) * Math.Sign(targetCamX - thePlayer.X);
            }
            // Y- Cam
            if(thePlayer.myMovement.CurrentMoveType == Components.SonicMovement.MoveType.AIR)
            {
                float ydistToPlayer = Math.Abs(targetCamY - thePlayer.Y);
                float yamtExceeded = 64 - ydistToPlayer;
                if (ydistToPlayer > 64)
                {
                    targetCamY -= Math.Min(Math.Abs(yamtExceeded * Math.Sign(targetCamY - thePlayer.Y)), 16.0f) * Math.Sign(targetCamY - thePlayer.Y);
                }
            }
            else
            {
                float relYPos = thePlayer.Y - targetCamY;
                if(relYPos != 96)
                {
                    targetCamY += Math.Min(Math.Abs(relYPos), 6) * Math.Sign(relYPos);
                }
            }

            this.CenterCamera(targetCamX, targetCamY);


            // Shader crud
            LUTShade.SetParameter("texture", Global.theGame.Surface.GetTexture());
            LUTShade.SetParameter("lut", LUTTable.Texture);
            LUTShade.SetParameter("belowwater", (CameraCenterY + 120.0f) / 240.0f);
            LUTShade.SetParameter("cutoff", waterLevel / 240.0f);
            LUTShade.SetParameter("time", Global.theGame.Timer);
            DarkenShader.SetParameter("freezetime", freezeLockTime);
            DarkenShader.SetParameter("comboamt", thePlayer.comboAmt);
       

        }

        public void CreateObjects()
        {
            thePlayers = new List<Entities.SonicPlayer>();
            for (int i = 0; i < tmxMapData.ObjectGroups.Count; i++ )
            {
                for (int j = 0; j < tmxMapData.ObjectGroups[i].Objects.Count; j++)
                {
                    // Process current object
                    TmxObject tmObj = tmxMapData.ObjectGroups[i].Objects[j];
                    if(tmObj.Name == "SonicStart")
                    {
                        thePlayer = new Entities.SonicPlayer(Global.playerSession.GetController<NotSonic.System.SegaController>(), null, (float)tmObj.X, (float)tmObj.Y);
                        
                        thePlayers.Add(thePlayer);
                        
                    }
                    if(tmObj.Name == "Ring")
                    {
                        Ring newRing = new Ring((float)tmObj.X, (float)tmObj.Y);
                        Add(newRing); 
                    }
                    if (tmObj.Name == "Loop")
                    {
                        string atilesstring, btilesstring;
                        tmObj.Properties.TryGetValue("ATiles", out atilesstring);
                        tmObj.Properties.TryGetValue("BTiles", out btilesstring);
                        Loop newLoop = new Loop(tileList, (float)tmObj.X, (float)tmObj.Y, atilesstring, btilesstring);
                        Add(newLoop);
                        loopList.Add(newLoop);
                    }
                }
            }
        }

        public void SetProperties()
        {
            waterLevel = tmxMapData.Properties.ValueAsFloat("WaterLevel");
            tmxMapData.Properties.TryGetValue("MapName", out friendlyMapName);
        }

        public void ToggleShader()
        {
            if(Global.theGame.Surface.Shader == null)
            {
                Global.theGame.Surface.Shader = LUTShade;
            }
            else
            {
                Global.theGame.Surface.Shader = null;
            }
        }

        public void PlayMusic()
        {
            mushroomHillMusic.Play();
        }

        public void ProcessMessages()
        {
            // Loop through all messages
            foreach (MessageEvent msg in Global.eventList)
            {
                ProcessMessage(msg);
            }

            // Messages checked, clear list. 
            Global.eventList.Clear();
        }

        public void ProcessMessage(MessageEvent msg)
        {
            if(msg.myType == "DESTROYED")
            {
                if(thePlayer.comboAmt > 2)
                {
                    freezeLockTime += 2.0f * thePlayer.comboAmt;
                    if(freezeLockTime > 35)
                    {
                        freezeLockTime = 35;
                    }
                    if(thePlayer.comboAmt > 4 && thePlayer.comboAmt <= 6)
                    {
                        theCamShaker.ShakeCamera();
                        
                    }
                    if(thePlayer.comboAmt > 6)
                    {
                        theCamShaker.ShakeCamera(20 + thePlayer.comboAmt * 1.1f, 0.05f * thePlayer.comboAmt);
                    }
                }
                
            }

            if(msg.myType == "ring_collect")
            {
                int ringAdd = 0;
                int.TryParse(msg.infoString, out ringAdd);
                Rings += ringAdd;
                
                
                ringSound.Play();

            }
        }

        public void CheckFreezeLock()
        {
            if(freezeLockTime > 0 && !freezeLocked)
            {
                
                freezeLocked = true;
                if (thePlayer.comboAmt > 2)
                {
                    SetDarkener(true);
                    DarkenShader.SetParameter("maxfreeze", freezeLockTime);
                    // Pick and set an animation for the player to be in.
                    thePlayer.spriteSheet.Play("freezers");
                    Range pickRange = new Range(0, 4);
                    thePlayer.spriteSheet.CurrentFrameIndex = pickRange.RandInt;
                    thePlayer.spriteSheet.FlippedX = Rand.Bool;
                    thePlayer.spriteSheet.FlippedY = Rand.Bool;
                    thePlayer.spriteSheet.Update();


                }
                PauseGroupToggle(Global.GROUP_ACTIVEOBJECTS);
            }

            if(freezeLocked)
            {
                if(freezeLockTime > 0)
                {
                    freezeLockTime--;
                }
                else
                {
                    freezeLockTime = 0;
                    SetDarkener(false);
                    freezeLocked = false;
                    PauseGroupToggle(Global.GROUP_ACTIVEOBJECTS);
                }
            }
        }

        public void Impulse(params string[] target)
        {
            // Clone of Valve's Impulse command - accepts an integer, will perform a task based on that integer. So I don't need to register more than one command.
            if(target.Count() > 0)
            {
                string commandPassed = target[0];
                if(commandPassed == "0")
                {
                    // Toggle Debug View
                    thePlayer.myMovement.ToggleDebugView();
                    foreach(Loop l in loopList)
                    {
                        l.DrawDebug = !l.DrawDebug;
                    }

                }
                if(commandPassed == "1")
                {
                    // Toggle Shaders
                    ToggleShader();
                }
                if(commandPassed == "2")
                {
                    // Play Music
                    PlayMusic();
                }
                if(commandPassed == "3")
                {
                    thePlayer.GrossMode = !thePlayer.GrossMode;
                }
                if(commandPassed == "4")
                {
                    MakeBadniks();
                }
                if(commandPassed == "5")
                {
                    sloMo = !sloMo;
                }
                if(commandPassed == "6")
                {
                    // record
                    Global.playerSession.Controller.Record();
                    
                    
                }
                if (commandPassed == "7")
                {
                    // save record
                    Global.playerSession.Controller.Stop();
                    Global.playerSession.Controller.SaveRecording(Assets.INPUT_RECORD_DEBUG);
                }
                if (commandPassed == "8")
                {
                    // play 
                    foreach(NotSonic.Entities.SonicPlayer ply in thePlayers)
                    {
                        ply.myController.PlaybackFile(Assets.INPUT_RECORD_DEBUG);
                    }
                    //Global.playerSession.Controller.PlaybackFile(Assets.INPUT_RECORD_DEBUG);
                    
                }
                if(commandPassed == "100")
                {
                    //Reset
                    Global.theGame.SwitchScene(new LevelScene(Assets.MAP_TEST));
                }

            }
        }


        public void SetDarkener(bool set)
        {
            // Set the shader to the darkening shader for all non-active objects in scene.
            foreach(Entity ent in this.GetEntities<Entity>())
            {
                if(ent.Group != Global.GROUP_ACTIVEOBJECTS && ent.Graphic != null)
                {
                    if (set)
                    {
                        ent.Graphic.Shader = DarkenShader;
                    }
                    else
                    {
                        ent.Graphic.ClearShader();
                    }
                }

            }
            foreach (Graphic g in GetGraphics())
            {
                if (set)
                {
                    g.Shader = DarkenShader;
                }
                else
                {
                    g.ClearShader();
                }

            }

        }

        private void MakeBadniks()
        {
            // DEBUG: CREATE A BADNIK
            for (int i = 0; i < 32; i++)
            {
                BadnikTest newBad = new BadnikTest(100.0f + i * (32), 96.0f);
                newBad.thePlayer = thePlayer;
                Add(newBad);
            }
        }

        public override void Render()
        {
            
            base.Render();

            
        }
    }
}
