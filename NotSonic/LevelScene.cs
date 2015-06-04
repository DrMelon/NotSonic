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

        // List of Tiles
        List<NotSonic.Components.Tile> tileList;

        // Map Name
        public string mapName;
        
        // Tiled TMX Loader instance
        public TmxMap tmxMapData;
        Tilemap tilemap;


        // Shader fun
        Shader LUTShade = new Shader(ShaderType.Fragment, Assets.LUTSHADER);
        Image LUTTable = new Image(Assets.LUTIMAGE);

        // Mushroom Hill Dot M P 3
        Music mushroomHillMusic = new Music(Assets.MUS_MUSH);


        public LevelScene(string mapFilename)
        {

            // Get map name
            mapName = mapFilename;
            
            // Load the map
            LoadMap();

            this.UseCameraBounds = true;
            this.ApplyCamera = true;

            // Set the camera bounds based on the map size.
            this.CameraBounds.X = 0;
            this.CameraBounds.Y = 0;
            this.CameraBounds.Width = 16 * tmxMapData.Width;
            this.CameraBounds.Height = 16 * tmxMapData.Height;



            thePlayer = new Entities.SonicPlayer(tileList, 70, 32);

                      
            
            
            // Create the parallax bg bits
            Image para_Far = new Image(Assets.PARA_TREES_1);
            para_Far.RepeatX = true;
            para_Far.ScrollY = 0.15f;
            para_Far.ScrollX = 0.3f;
            para_Far.Y = 230;
            AddGraphic(para_Far);

            Image para_Close = new Image(Assets.PARA_TREES_2);
            para_Close.RepeatX = true;
            para_Close.ScrollY = 0.2f;
            para_Close.ScrollX = 0.5f;
            para_Close.Y = -160;
            AddGraphic(para_Close);




            AddGraphic(tilemap);

            Add(thePlayer);



            
            



            Otter.Debugger.Instance.ShowPerformance(5);

            // Make debug command to toggle shader

            Global.theGame.Surface.Shader = null;
            Otter.Debugger.CommandFunction myFunc = new Debugger.CommandFunction(ToggleShader);
            Debugger.Instance.RegisterCommand("instagram", myFunc, (Otter.CommandType[])new Otter.CommandType[0]);
            Otter.Debugger.CommandFunction myFunc2 = new Debugger.CommandFunction(PlayMusic);
            Debugger.Instance.RegisterCommand("playmusic", myFunc2, (Otter.CommandType[])new Otter.CommandType[0]);
        }


        public void LoadMap()
        {
            // Load map using the map loader.
            tmxMapData = new TmxMap(mapName);

            // Load Level from Tiled map.
            tileList = new List<Components.Tile>();
            tilemap = new Tilemap(Assets.TILE_SHEET, tmxMapData.Height * 16, 16);
            Global.maxlvlheight = tmxMapData.Height * 16;
            Global.maxlvlheight = tmxMapData.Width * 16;
            tilemap.AddLayer("vis", 19);
            

            // For each tile in the Solid layer, we want to create a Tile object, and use the relevant image.
            for(int i = 0; i < tmxMapData.Layers["Solid"].Tiles.Count; i++)
            {
                tilemap.SetTile(tmxMapData.Layers["Solid"].Tiles[i].X * 16, tmxMapData.Layers["Solid"].Tiles[i].Y * 16, tmxMapData.Layers["Solid"].Tiles[i].Gid - 1, "base");
                tilemap.UsePositions = true;
                if(tmxMapData.Layers["Solid"].Tiles[i].Gid != 0)
                {
                    // Heightmap Data
                    // In the Solid_Height layer, we find the appropriate tile type. Angles are included in this.
                    int heightMapID = tmxMapData.Layers["Solid_Height"].Tiles[i].Gid;
                    bool heightFlipX = tmxMapData.Layers["Solid_Height"].Tiles[i].HorizontalFlip;
                    bool heightFlipY = tmxMapData.Layers["Solid_Height"].Tiles[i].VerticalFlip;
                    if(heightMapID >= tmxMapData.Tilesets["Test_Height"].FirstGid)
                    {
                        heightMapID = tmxMapData.Layers["Solid_Height"].Tiles[i].Gid - tmxMapData.Tilesets["Test_Height"].FirstGid;
                    }

                    // Create tile.
                    NotSonic.Components.Tile newTile = new NotSonic.Components.Tile(tmxMapData.Layers["Solid"].Tiles[i].X * 16, tmxMapData.Layers["Solid"].Tiles[i].Y * 16, heightMapID, heightFlipX, heightFlipY);

                  

                    
                    // Set the tile's graphic properly.
                    newTile.tileImage.Frame = tmxMapData.Layers["Solid"].Tiles[i].Gid - 1;
                    newTile.tileImage.FlippedX = tmxMapData.Layers["Solid"].Tiles[i].HorizontalFlip;
                    newTile.tileImage.FlippedY = tmxMapData.Layers["Solid"].Tiles[i].VerticalFlip;



                    newTile.Layer = 19;
                    
                    tileList.Add(newTile);
                }

                

                
                

            }

            
           
            
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
       

        }

        public void ToggleShader(params string[] target)
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

        public void PlayMusic(params string[] target)
        {
            mushroomHillMusic.Play();
        }


        public override void Render()
        {
            
            base.Render();

            
        }
    }
}
