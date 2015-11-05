using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
namespace NotSonic
{
    class TitleCard : Entity
    {
        public Image TitleCard_Black;
        public Image TitleCard_Bottom;
        public Image TitleCard_Middle;
        public Image TitleCard_Top;
        public RichText TitleCard_Text;
        public bool Entered = false;
        public bool Waited = false;
        public bool Exited = false;
        public float EnterTime = 60 * 1.5f;
        public float WaitTime = 60 * 1.5f;
        public float ExitTime = 60 * 1.5f;

        public TitleCard(string text)
        {
            // Create bg
            TitleCard_Black = Image.CreateRectangle(400, 240, Color.Black);
            TitleCard_Bottom = new Image(Assets.TITLE_CARD_B);
            TitleCard_Middle = new Image(Assets.TITLE_CARD_M);
            TitleCard_Top = new Image(Assets.TITLE_CARD_T);

            // They should all be 400x240, and so on-screen by default.
            // They don't scroll.
            TitleCard_Black.Scroll = 0;
            TitleCard_Bottom.Scroll = 0;
            TitleCard_Middle.Scroll = 0;
            TitleCard_Top.Scroll = 0;

            // The bottom, middle, and top start offsceen in different places.
            TitleCard_Bottom.Y = -240;
            TitleCard_Middle.X = -400;
            TitleCard_Top.X = 400;

            // They'll slide in/out depending on the phase.


            // Create text
            TitleCard_Text = new RichText(text + "\nact 1", Assets.TITLE_CARD_FONT, 36);
            TitleCard_Text.Scroll = 0;
            TitleCard_Text.Smooth = false;
            TitleCard_Text.X = 100;
            TitleCard_Text.Y = 280; // slide up from bottom

            // Add in render order
            AddGraphic(TitleCard_Black);
            AddGraphic(TitleCard_Bottom);
            AddGraphic(TitleCard_Middle);
            AddGraphic(TitleCard_Top);
            AddGraphic(TitleCard_Text);

        }

        public void Reset()
        {
            Entered = false;
            Waited = false;
            Exited = false;
            EnterTime = 60 * 1.5f;
            WaitTime = 60 * 1.5f;
            ExitTime = 60 * 1.5f;

            TitleCard_Bottom.Y = -240;
            TitleCard_Middle.X = -400;
            TitleCard_Top.X = 400;
            TitleCard_Text.Y = 280;
            TitleCard_Text.X = 100;
        }

        public float ConvertReverseTime(float currentTime, float maxTime)
        {
            //reverse-time; so instead of 10 out of 60 seconds elapsed it'll be 50 out of 60... 
            return 1.0f - (currentTime/maxTime);
        }

        public override void Update()
        {
            base.Update();


            if(!Entered)
            {
                // slide things in!
                TitleCard_Bottom.Y = Util.Lerp(-240, 0, ConvertReverseTime(EnterTime, 60 * 1.5f));
                TitleCard_Middle.X = Util.Lerp(-400, 0, ConvertReverseTime(EnterTime, 60 * 1.5f));
                TitleCard_Top.X = Util.Lerp(400, 0, ConvertReverseTime(EnterTime, 60 * 1.5f));
                TitleCard_Text.Y = Util.Lerp(280, 100, ConvertReverseTime(EnterTime, 60 * 1.5f));

                // decrement timer
                if(EnterTime > 0)
                {
                    EnterTime--;
                }
                else
                {
                    Entered = true;
                }
            }
            else if(!Waited)
            {
                // waiting does nothing, except change the alpha of the blackness to 0
                TitleCard_Black.Color.A = 0;


                //decrement timer
                if(WaitTime > 0)
                {
                    WaitTime--;
                }
                else
                {
                    Waited = true;
                }
            }
            else if(!Exited)
            {
                // slide things out!
                TitleCard_Bottom.Y = Util.Lerp(0, -240, ConvertReverseTime(ExitTime, 60 * 1.5f));
                TitleCard_Middle.X = Util.Lerp(0, -400, ConvertReverseTime(ExitTime, 60 * 1.5f));
                TitleCard_Top.X = Util.Lerp(0, 400, ConvertReverseTime(ExitTime, 60 * 1.5f));
                TitleCard_Text.Y = Util.Lerp(100, 280, ConvertReverseTime(ExitTime, 60 * 1.5f));


                //decrement timer
                if(ExitTime > 0)
                {
                    ExitTime--;
                }
                else
                {
                    Exited = true;
                    RemoveSelf();
                }
            }

        }


    }
}
