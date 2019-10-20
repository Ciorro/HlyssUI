using HlyssUI.Utils;
using System;

namespace HlyssUI.Controllers.Tweens
{
    class TweenIn : Tween
    {
        public TweenIn() : base("in") { }

        public override Tween Get()
        {
            return new TweenIn();
        }

        public override void Update()
        {
            timePassed += DeltaTime.Current;

            progress = ((float)Math.Round(Math.Pow(timePassed / Duration, Power), 4));

            if (timePassed >= Duration)
            {
                progress = 1;
                Finish();
                return;
            }
        }
    }
}
