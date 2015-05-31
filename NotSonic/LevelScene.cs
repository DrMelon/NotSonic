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
// Purpose: This is the main gameplay state. This is constructed from level information.

namespace NotSonic
{
    class LevelScene : Scene
    {
        // The Player
        NotSonic.Entities.SonicPlayer thePlayer;

        // List of Tiles
        List<NotSonic.Components.Tile> tileList;

        public LevelScene()
        {
            this.UseCameraBounds = true;
            this.ApplyCamera = true;
            this.CameraBounds.X = -400;
            this.CameraBounds.Y = -400;
            this.CameraBounds.Width = 1600;
            this.CameraBounds.Height = 1600;

            // Load Level from Tiled map.
            tileList = new List<Components.Tile>();

            // DEBUG: Create blank tile for tilelist
            tileList.Add(new Components.Tile(64 - 16, 64 - 16, NotSonic.Components.Tile.TileType.TILE_SLOPE_45_DOWN));
            tileList.Add(new Components.Tile(64, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 16, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 32, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 32 + 16, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 64, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 64 + 16, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 64 + 32, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 64 + 32 + 16, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 64 + 64, 64 - 16, NotSonic.Components.Tile.TileType.TILE_LOOP_UP_10));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 16, 64 - 16, NotSonic.Components.Tile.TileType.TILE_LOOP_UP_20));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 32, 64 - 16, NotSonic.Components.Tile.TileType.TILE_LOOP_UP_30));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 32, 64 - 32, NotSonic.Components.Tile.TileType.TILE_LOOP_UP_45));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 32 + 16, 64 - 32, NotSonic.Components.Tile.TileType.TILE_LOOP_UP_60));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 32 + 16, 64 - 32 - 16, NotSonic.Components.Tile.TileType.TILE_LOOP_UP_70));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 32 + 16, 64 - 64, NotSonic.Components.Tile.TileType.TILE_LOOP_UP_85));
            tileList.Add(new Components.Tile(64 + 64 + 64, 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 64, 64 - 64, NotSonic.Components.Tile.TileType.TILE_BASIC));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 64, 64 - 64 - 16, NotSonic.Components.Tile.TileType.TILE_BASIC));

            thePlayer = new Entities.SonicPlayer(tileList, 70, 32);
            
            foreach(NotSonic.Components.Tile t in tileList)
            {
                Add(t);
            }

            Add(thePlayer);

            

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
