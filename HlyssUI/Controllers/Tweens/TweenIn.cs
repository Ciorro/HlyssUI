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
            float offset = Math.Abs(-0.01f - progress);

            offset *= DeltaTime.Current * Speed;
            progress += offset;

            if (Percentage > 1)
            {
                progress = 100;
                Finish();
                return;
            }
        }
    }
}
