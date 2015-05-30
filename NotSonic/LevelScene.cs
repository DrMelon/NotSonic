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
            this.CameraBounds.X = 0;
            this.CameraBounds.Y = 0;
            this.CameraBounds.Width = 400;
            this.CameraBounds.Height = 240;

            // Load Level from Tiled map.
            tileList = new List<Components.Tile>();

            // DEBUG: Create blank tile for tilelist
            tileList.Add(new Components.Tile(64, 64, 0.0f, HeightArrays.HEIGHT_ARRAY_FULL));
            tileList.Add(new Components.Tile(64 + 16, 64, 0.0f, HeightArrays.HEIGHT_ARRAY_FULL));
            tileList.Add(new Components.Tile(64 + 32, 64, 0.0f, HeightArrays.HEIGHT_ARRAY_FULL));
            tileList.Add(new Components.Tile(64 + 32 + 16, 64, 0.0f, HeightArrays.HEIGHT_ARRAY_FULL));
            tileList.Add(new Components.Tile(64 + 64, 64, 0.0f, HeightArrays.HEIGHT_ARRAY_FULL));
            tileList.Add(new Components.Tile(64 + 64 + 16, 64 + 16, 0.0f, HeightArrays.HEIGHT_ARRAY_FULL));
            tileList.Add(new Components.Tile(64 + 64 + 32, 64 + 16, 0.0f, HeightArrays.HEIGHT_ARRAY_FULL));
            tileList.Add(new Components.Tile(64 + 64 + 64, 64 + 32, 45.0f, HeightArrays.HEIGHT_SLP_45_UP));
            tileList.Add(new Components.Tile(64 + 64 + 64 + 16, 64 + 16, 45.0f, HeightArrays.HEIGHT_SLP_45_UP));


            thePlayer = new Entities.SonicPlayer(tileList, 70, 32);
            
            foreach(NotSonic.Components.Tile t in tileList)
            {
                Add(t);
            }

            Add(thePlayer);

        }
    }
}
