using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [NotSonic] Project.
// Date: 2015/05/28
//----------------
// Purpose: This contains static references to file paths for loading assets.

namespace NotSonic
{
    class Assets
    {

        public const string SONIC_SHEET = "../../Assets/GFX/Sonic_3K.png"; //32x40 sprites
        public const string EXAMPLE_TILE = "../../Assets/GFX/TILE0.png"; //16x16 sprites
        public const string TILE_SHEET = "../../Assets/GFX/Mushroom_Hill_bank.png"; //16x16 sprites
        public const string TILE_SHEET_ANGLES = "../../Assets/GFX/Angle_Tiles.png"; //16x16 sprites

        public const string PARA_TREES_1 = "../../Assets/GFX/Parallax/Mush_Trees_1.png";
        public const string PARA_TREES_2 = "../../Assets/GFX/Parallax/Mush_Trees_2.png";

        public const string LUTSHADER = "../../Assets/GLSL/lutshader.frag";
        public const string LUTIMAGE = "../../Assets/GLSL/lut.png";

        public const string SND_JUMP = "../../Assets/SND/jump.wav";
        public const string SND_ROLL = "../../Assets/SND/roll.wav";
        public const string SND_DEAD = "../../Assets/SND/dead.wav";

        public const string MUS_MUSH = "../../Assets/MUS/mush.ogg";

        public const string MAP_TEST = "../../Assets/MAP/Test_Mush.tmx"; // Tiled TMX File.
    }
}
