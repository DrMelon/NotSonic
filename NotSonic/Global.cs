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
// Purpose: This contains useful global information, like the current player session.

namespace NotSonic
{
    class Global
    {
        public static Game theGame = null;
        public static Session playerSession = null;
        public static bool paused = false;
        public static int GROUP_ACTIVEOBJECTS = 1;
        public static NotSonic.System.SegaController theController;
        public static List<MessageEvent> eventList;
        public static Shader paleShader = new Shader(Assets.PALESHADER);

        public static int maxlvlwidth = 16000;
        public static int maxlvlheight = 16000;

    }
}
