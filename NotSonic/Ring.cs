using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace NotSonic
{
    class Ring : Entity
    {

        public Spritemap<string> mySprite;
        public BoxCollider myCollider;
        public int CollideTag = 0;

        public Ring(float x, float y)
        {
            X = x + 8;
            Y = y + 8;

            mySprite = new Spritemap<string>(Assets.GFX_RING, 16, 16);
            mySprite.Add("default", new Anim(new int[] { 0, 1, 2, 3 }, new float[] {10f}));
            myCollider = new BoxCollider(16, 16, 1);
            mySprite.Play("default");
            

            AddCollider(myCollider);
            AddGraphic(mySprite);

            mySprite.CenterOrigin();
            myCollider.CenterOrigin();
        }

        public override void Update()
        {
            base.Update();

            if(Collide(X, Y, CollideTag) != null)
            {
                MessageEvent ringMsg = new MessageEvent();
                ringMsg.myType = "ring_collect";
                ringMsg.infoString = "1";
                Global.eventList.Add(ringMsg);
                RemoveSelf();
                
            }

        }
    }
}
