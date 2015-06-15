using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;


// Simple test badnik!

namespace NotSonic
{
    class BadnikTest : DestroyableEntity
    {
        public BadnikTest(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
            Graphic = new Image(Assets.BADNIK_SHEET);
            CollisionWidth = Graphic.HalfWidth;
            CollisionHeight = Graphic.HalfHeight;
            Group = Global.GROUP_ACTIVEOBJECTS;
        }

        public override void Update()
        {
            base.Update();
        }

    }
}
