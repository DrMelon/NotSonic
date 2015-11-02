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
            Global.eventList = new List<MessageEvent>();

            // Render at 720p.
            Global.theGame.SetWindowScale(2);

            // Initialize player controls.
            Global.playerSession = Global.theGame.AddSession("PlayerControls");
            
            // Emulate a 3-button 'ABC' sega controller
            Global.playerSession.Controller = new SegaController(0);
            Global.theController = Global.playerSession.GetController<SegaController>();
            
            // Keyboard Controls
            Global.theController.A.AddKey(Key.Z);
            Global.theController.B.AddKey(Key.X);
            Global.theController.C.AddKey(Key.C);

            Global.theController.Start.AddKey(Key.Return);

            Global.theController.Left.AddKey(Key.Left);
            Global.theController.Right.AddKey(Key.Right);
            Global.theController.Up.AddKey(Key.Up);
            Global.theController.Down.AddKey(Key.Down);

            Assets.GenerateAssetNames();
            Global.theGame.FirstScene = new LevelScene(Assets.MAP_TEST);
            Global.theGame.Color = new Color("5D516E");
            

            // Begin otter!
           
            
            Global.theGame.Start();



        }
    }
}
