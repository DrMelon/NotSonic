using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Lidgren.Network;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [NotSonic] Project.
// Date: 2015/05/28
//----------------
// Purpose: This mimics a sega genesis 3-button controller.
// Now network-capable!!

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

        // Network stuff
        public NetClient thePeer;
        public int NetID = 0;

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

        public void BeginNetworkingController(NetClient peer)
        {
            thePeer = peer;
            
            
        }

        public Int32 SerializeState()
        {
            Int32 bitflags = 0;

            if(A.Down)
            {
                bitflags |= (1 << 0);
            }
            if(B.Down)
            {
                bitflags |= (1 << 1);
            }
            if(C.Down)
            {
                bitflags |= (1 << 2);
            }
            if(Start.Down)
            {
                bitflags |= (1 << 3);
            }
            if(Up.Down)
            {
                bitflags |= (1 << 4);
            }
            if(Down.Down)
            {
                bitflags |= (1 << 5);
            }
            if(Left.Down)
            {
                bitflags |= (1 << 6);
            }
            if(Right.Down)
            {
                bitflags |= (1 << 7);
            }


            return bitflags;
        }

        public void DeSerializeState(Int32 bitflags)
        {
            
            if((bitflags & (1 << 0)) > 0)
            {
                A.ForceState(true);
            }
            else
            {
                A.ForceState(false);
            }
            if ((bitflags & (1 << 1)) > 0)
            {
                B.ForceState(true);
            }
            else
            {
                B.ForceState(false);
            }
            if ((bitflags & (1 << 2)) > 0)
            {
                C.ForceState(true);
            }
            else
            {
                C.ForceState(false);
            }
            if ((bitflags & (1 << 3)) > 0)
            {
                Start.ForceState(true);
            }
            else
            {
                Start.ForceState(false);
            }
            if ((bitflags & (1 << 4)) > 0)
            {
                Up.ForceState(true);
            }
            else
            {
                Up.ForceState(false);
            }
            if ((bitflags & (1 << 5)) > 0)
            {
                Down.ForceState(true);
            }
            else
            {
                Down.ForceState(false);
            }
            if ((bitflags & (1 << 6)) > 0)
            {
                Left.ForceState(true);
            }
            else
            {
                Left.ForceState(false);
            }
            if ((bitflags & (1 << 7)) > 0)
            {
                Right.ForceState(true);
            }
            else
            {
                Right.ForceState(false);
            }
        }

        public void SendInputs()
        {
            
            // Send on network!
            var netmsg = thePeer.CreateMessage();
            netmsg.Write(NetFlags.NETMSG_INPUTS);
            netmsg.Write(NetID);
            netmsg.Write(SerializeState());
            thePeer.SendMessage(netmsg, NetDeliveryMethod.ReliableOrdered);

            
        }

        public void ReceiveInputs(Int32 inputs)
        {
            
            DeSerializeState(inputs);
            
        }
    }
}

        