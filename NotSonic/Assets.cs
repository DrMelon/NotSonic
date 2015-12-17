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

        public const string SONIC_SHEET = "../../Assets/GFX/Sonic_3K.png"; //40x40 sprites
        public const string EXAMPLE_TILE = "../../Assets/GFX/TILE0.png"; //16x16 sprites
        //public const string TILE_SHEET = "../../Assets/GFX/Mushroom_Hill_bank.png"; //16x16 sprites
        public const string TILE_SHEET = "../../Assets/GFX/Purple-Place.png"; 
        public const string TILE_SHEET_ANGLES = "../../Assets/GFX/newangletiles_bank.png"; //16x16 sprites
        public const string BADNIK_SHEET = "../../Assets/GFX/badnik.png";
        public const string GFX_RING = "../../Assets/GFX/rings.png";

        public const string PARA_TREES_1 = "../../Assets/GFX/Parallax/Mush_Trees_1.png";
        public const string PARA_TREES_2 = "../../Assets/GFX/Parallax/Mush_Trees_2.png";

        public const string LUTSHADER = "../../Assets/GLSL/lutshader.frag";
        //public const string LUTSHADER = "../../Assets/GLSL/dither.frag";
        public const string DARKSHADER = "../../Assets/GLSL/dark.frag";
        public const string PALESHADER = "../../Assets/GLSL/pale.frag";
        public const string LUTIMAGE = "../../Assets/GLSL/lut_b.png";

        public const string SND_JUMP = "../../Assets/SND/jump.wav";
        public const string SND_ROLL = "../../Assets/SND/roll.wav";
        public const string SND_DEAD = "../../Assets/SND/dead.wav";
        public const string SND_REV = "../../Assets/SND/rev.wav";
        public const string SND_DASHGO = "../../Assets/SND/dashgo.wav";
        public const string SND_ORCH = "../../Assets/SND/orch2.wav";
        public const string SND_POP = "../../Assets/SND/pop.wav";
        public const string SND_BRAKE = "../../Assets/SND/brake.wav";
        public const string SND_EXPLODE = "../../Assets/SND/explode.wav";
        public const string SND_WARP = "../../Assets/SND/warp.wav";
        public const string SND_VO_IMPRESSIVE = "../../Assets/SND/announcer/impressive.wav";
        public const string SND_VO_BLUESTREAK = "../../Assets/SND/announcer/bluestreak.wav";
        public const string SND_VO_EXCELLENT = "../../Assets/SND/announcer/excellent.wav";

        public const string MUS_MUSH = "../../Assets/MUS/mush.ogg";

        public const string MAP_TEST = "../../Assets/MAP/Test_Mush.tmx"; // Tiled TMX File.

        public static Dictionary<string, string> lookupAssets = new Dictionary<string, string>();

        public static void GenerateAssetNames()
        {
            Assets.lookupAssets.Add("PARA_TREES_1", Assets.PARA_TREES_1);
            Assets.lookupAssets.Add("PARA_TREES_2", Assets.PARA_TREES_2);
        }

        public static string GetAsset(string tryget)
        {
            string val;
            Assets.lookupAssets.TryGetValue(tryget, out val);
            return val;
        }
    }
}
