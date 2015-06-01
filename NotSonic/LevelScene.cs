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
            
            foreach(NotSonic.Components.Tile t in tileList)
            {
                Add(t);
            }

            Add(thePlayer);

            

        }

        public void LoadMap()
        {
            // Load map using the map loader.
            tmxMapData = new TmxMap(mapName);

            // Load Level from Tiled map.
            tileList = new List<Components.Tile>();

            // For each tile in the Solid layer, we want to create a Tile object, and use the relevant image.
            for(int i = 0; i < tmxMapData.Layers["Solid"].Tiles.Count; i++)
            {
                
                if(tmxMapData.Layers["Solid"].Tiles[i].Gid != 0)
                {
                    // Heightmap Data
                    // In the Solid_Height layer, we find the appropriate tile type. Angles are included in this.
                    int heightMapID = tmxMapData.Layers["Solid_Height"].Tiles[i].Gid;
                    if(heightMapID >= 324)
                    {
                        heightMapID = tmxMapData.Layers["Solid_Height"].Tiles[i].Gid - 324;
                    }

                    NotSonic.Components.Tile newTile = new NotSonic.Components.Tile(tmxMapData.Layers["Solid"].Tiles[i].X * 16, tmxMapData.Layers["Solid"].Tiles[i].Y * 16, heightMapID);
                    
                    
                    // Set the tile's graphic properly.
                    newTile.tileImage.Frame = tmxMapData.Layers["Solid"].Tiles[i].Gid - 1;
                    newTile.tileImage.FlippedX = tmxMapData.Layers["Solid"].Tiles[i].HorizontalFlip;
                    newTile.tileImage.FlippedY = tmxMapData.Layers["Solid"].Tiles[i].VerticalFlip;
                    
                    
                    tileList.Add(newTile);
                }
                


                

            }

            
           
            
        }

        public override void UpdateLast()
        {
            base.UpdateLast();

            // Recentering Camera Based on Player Position & Direction

            float targetCamX = 0;
            float targetCamY = 0;


         
            targetCamX = (thePlayer.X + thePlayer.spriteSheet.Width / 2.0f);
            targetCamY = thePlayer.Y + 20; // Above player a little.

            if(Math.Abs(targetCamX - this.CameraCenterX) > 8)
            {
                targetCamX = Util.Approach(this.CameraCenterX, targetCamX, (float)Math.Min(Math.Abs(thePlayer.X - this.CameraCenterX), 16.0f));
            }


            
            targetCamY = Util.Approach(this.CameraCenterY, targetCamY, (float)Math.Min(Math.Abs(thePlayer.Y - this.CameraCenterY), 16.0f));

            this.CenterCamera(targetCamX, targetCamY);

        }
    }
}
