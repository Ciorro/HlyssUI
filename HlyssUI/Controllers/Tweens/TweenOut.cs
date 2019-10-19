using HlyssUI.Utils;
using System;

namespace HlyssUI.Controllers.Tweens
{
    class TweenOut : Tween
    {
        public TweenOut() : base("out") { }

        public override Tween Get()
        {
            return new TweenOut();
        }

        public override void Update()
        {
            timePassed += DeltaTime.Current;

            progress = ((float)-Math.Pow(((timePassed / Duration) - 1), 8) + 1);
            
            if (timePassed >= Duration)
            {
                progress = 1;
                Finish();
                return;
            }
        }
    }
}
