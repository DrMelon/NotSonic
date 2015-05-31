using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Otter;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [NotSonic] Project.
// Date: 2015/05/30
//----------------
// Purpose: This is used to "sense" collisions with the ground and objects by the player characters.
// It's basically a simple line-cast, with room for additional info.
//  /!\ UNUSED CURRENTLY, NEEDS FIX /!\

namespace NotSonic.Components
{
    class Sensor
    {
        public struct CollisionInfo
        {
            // Whether this collision is valid or not
            public bool thisIsNull;
            // The tile we hit, if any
            public NotSonic.Components.Tile tileHit;
            // The distance it took to hit this.
            public float Distance;
        }

        // Start position and end position variables. Updated by whatever holds the instances.
        float StartX;
        float StartY;
        float EndX;
        float EndY;


        // This function checks for any tiles in the collision.
        // Need a way to locate nearby tiles in the tile list, rather than iterating. Iterating will do for now.
        public CollisionInfo Sense(List<Tile> tileList)
        {
            // Initialize new collision.
            CollisionInfo newCollision = new CollisionInfo();
            newCollision.tileHit = null;
            newCollision.thisIsNull = true;
            newCollision.Distance = -1.0f;

            // Also make sure to sanitize the inputs
            if (StartX > EndX)
            {
                float temp = EndX;
                EndX = StartX;
                StartX = temp;
            }

            if(StartY > EndY)
            {
                float temp = EndY;
                EndY = StartY;
                StartY = temp;
            }


            // Check for a tile using our position.
            foreach (Tile currentTile in tileList)
            {
                // Are we colliding on the Y-Axis, by checking X's?
                if(!(EndX < currentTile.X || StartX > currentTile.X + 16.0f))
                {
                    newCollision.tileHit = currentTile;
                    newCollision.Distance = currentTile.X - StartX;
                    newCollision.thisIsNull = false;
                    // Are we colliding on the X-Axis, by checking Y's?
                    if (!(EndY < currentTile.Y || StartY > currentTile.Y + 16.0f))
                    {
                        newCollision.tileHit = currentTile;
                        newCollision.Distance = currentTile.Y - StartY;
                        newCollision.thisIsNull = false;
                        break;
                    }
                }

            }


            return newCollision;
        }
    }
}
