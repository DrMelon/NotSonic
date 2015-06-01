using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using NotSonic.System;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [NotSonic] Project.
// Date: 2015/05/28
//----------------
// Purpose: Main Program Entry Point. Initializes Otter Window.


namespace NotSonic
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init internal resolution of 400x240 - 16:9 version of MegaDrive/Genesis spec.
            Global.theGame = new Game("NotSonic", 400, 240, 60, false);

            // Render at 720p.
            Global.theGame.SetWindow(1280, 720, false, true);

            // Initialize player controls.
            Global.playerSession = Global.theGame.AddSession("PlayerControls");
            
            // Emulate a 3-button 'ABC' sega controller
            Global.playerSession.Controller = new SegaController();
            
            // Keyboard Controls
            Global.playerSession.GetController<SegaController>().A.AddKey(Key.Z);
            Global.playerSession.GetController<SegaController>().B.AddKey(Key.X);
            Global.playerSession.GetController<SegaController>().C.AddKey(Key.C);

            Global.playerSession.GetController<SegaController>().Start.AddKey(Key.Return);

            Global.playerSession.GetController<SegaController>().Left.AddKey(Key.Left);
            Global.playerSession.GetController<SegaController>().Right.AddKey(Key.Right);
            Global.playerSession.GetController<SegaController>().Up.AddKey(Key.Up);
            Global.playerSession.GetController<SegaController>().Down.AddKey(Key.Down);


            Global.theGame.FirstScene = new LevelScene(Assets.MAP_TEST);
            Global.theGame.Color = new Color("736763");

            // Begin otter!
            Global.theGame.Start();



        }
    }
}
