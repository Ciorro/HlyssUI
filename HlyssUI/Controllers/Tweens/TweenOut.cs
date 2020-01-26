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
            base.Update();

            timePassed += _deltaTime.Current;

            progress = ((float)-Math.Pow(((timePassed / Duration) - 1), Power) + 1);

            if (timePassed >= Duration)
            {
                progress = 1;
                Finish();
                return;
            }
        }
    }
}
