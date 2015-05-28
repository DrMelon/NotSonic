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
// Purpose: This mimics a sega genesis 3-button controller.

namespace NotSonic.System
{
    class SegaController : Controller
    {
        public static int JoyButtonA = 0;
        public static int JoyButtonB = 1;
        public static int JoyButtonC = 2;
        public static int JoyButtonStart = 3;


        public Button A { get { return Button(Controls.A); } }
        public Button B { get { return Button(Controls.B); } }
        public Button C { get { return Button(Controls.C); } }

        public Button Start { get { return Button(Controls.Start); } }

        public Button Up { get { return Button(Controls.Up); } }
        public Button Down { get { return Button(Controls.Down); } }
        public Button Left { get { return Button(Controls.Left); } }
        public Button Right { get { return Button(Controls.Right); } }

        public SegaController(params int[] joystickId)
        {
            AddButton(Controls.A);
            AddButton(Controls.B);
            AddButton(Controls.C);
 
            AddButton(Controls.Start);


            AddButton(Controls.Up);
            AddButton(Controls.Down);
            AddButton(Controls.Left);
            AddButton(Controls.Right);

            // This links to real controllers? Testing needed.
            foreach (var joy in joystickId) {
                A.AddJoyButton(0, joy);
                B.AddJoyButton(1, joy);
                C.AddJoyButton(2, joy);
                Start.AddJoyButton(3, joy);

                Up
                    .AddAxisButton(AxisButton.YMinus, joy)
                    .AddAxisButton(AxisButton.PovYMinus, joy);
                Down
                    .AddAxisButton(AxisButton.YPlus, joy)
                    .AddAxisButton(AxisButton.PovYPlus, joy);
                Right
                    .AddAxisButton(AxisButton.XPlus, joy)
                    .AddAxisButton(AxisButton.PovXPlus, joy);
                Left
                    .AddAxisButton(AxisButton.XMinus, joy)
                    .AddAxisButton(AxisButton.PovXMinus, joy);
            }
        }

        enum Controls {
            A,
            B,
            C,
            Start,
            Up,
            Down,
            Left,
            Right
        }
    }
}

        