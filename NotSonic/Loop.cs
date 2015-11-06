using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace NotSonic
{
    class Loop : Entity
    {
        public BoxCollider LoopSwitchA;
        public BoxCollider LoopSwitchB;
        public BoxCollider LoopSwitchC;
        public BoxCollider LoopSwitchD;
        // These are the lists of tiles to toggle on or off.
        
        public List<NotSonic.Components.Tile> ATiles;
        public List<NotSonic.Components.Tile> BTiles;

        public bool AMode = true;

        public bool DrawDebug = false;

        public Loop(List<NotSonic.Components.Tile> masterList, float x, float y, string atilesstring, string btilesstring)
        {
            X = x;
            Y = y;
            ATiles = CreateTileListFromString(masterList, atilesstring);
            BTiles = CreateTileListFromString(masterList, btilesstring);

            LoopSwitchA = new BoxCollider(30, 64, 50);
            LoopSwitchA.X -= 4 * 16;


            AddCollider(LoopSwitchA);

            LoopSwitchB = new BoxCollider(30, 64, 50);
            LoopSwitchB.X += 4 * 16;
    

            AddCollider(LoopSwitchB);


            LoopSwitchC = new BoxCollider(30, 64, 50);
            LoopSwitchC.X -= 6 * 16;
            LoopSwitchC.Y += 6 * 16;


            AddCollider(LoopSwitchC);

            LoopSwitchD = new BoxCollider(30, 64, 50);
            LoopSwitchD.X += 6 * 16;
            LoopSwitchD.Y += 6 * 16;


            AddCollider(LoopSwitchD);

            // Starting off in Amode...
            AMode = false;
            Toggle();
        }

        public void Toggle()
        {
            AMode = !AMode;
            foreach (NotSonic.Components.Tile t in ATiles)
            {
                t.myTileInfo.On = !AMode;

            }
            foreach (NotSonic.Components.Tile t in BTiles)
            {
                t.myTileInfo.On = AMode;

            }
        }

        public void SetAMode(bool set)
        {
            AMode = !set;
            Toggle();
        }

        public override void Update()
        {
            base.Update();

            if (LoopSwitchA.Overlap(X, Y, 0) || LoopSwitchD.Overlap(X, Y, 0))
            {
                SetAMode(false);
            }
            if (LoopSwitchB.Overlap(X, Y, 0) || LoopSwitchC.Overlap(X, Y, 0))
            {
                SetAMode(true);
            }
        }

        public override void Render()
        {
            base.Render();

            if(DrawDebug)
            {
                Draw.Rectangle(X - 16 * 4, Y, 30, 64, Color.None, (AMode ? Color.Red : Color.Blue), 1);
                Draw.Rectangle(X + 16 * 4, Y, 30, 64, Color.None, (AMode ? Color.Red : Color.Blue), 1);
                Draw.Rectangle(X - 16 * 6, Y + 6 * 16, 30, 64, Color.None, (AMode ? Color.Red : Color.Blue), 1);
                Draw.Rectangle(X + 16 * 6, Y + 6 * 16, 30, 64, Color.None, (AMode ? Color.Red : Color.Blue), 1);
            }

        }

        public List<NotSonic.Components.Tile> CreateTileListFromString(List<NotSonic.Components.Tile> masterList, string tilesstring)
        {
            List<NotSonic.Components.Tile> newlist = new List<NotSonic.Components.Tile>();

            // Tokenize string on ;'s
            // and again on ,'s
            string[] theTiles = tilesstring.Split(';');
            
            foreach(string s in theTiles)
            {
                string[] coords = s.Split(',');
                int XCoord = 0;
                int.TryParse(coords.ToArray()[0], out XCoord);
                int YCoord = 0;
                int.TryParse(coords.ToArray()[1], out YCoord);

                // Fetch a tile into the list using the coordinates.
                foreach(NotSonic.Components.Tile t in masterList)
                {
                    if((int)t.X / 16 == XCoord && (int)t.Y / 16 == YCoord)
                    {
                        newlist.Add(t);
                    }
                }
            }


            return newlist;

        }

    }
}
