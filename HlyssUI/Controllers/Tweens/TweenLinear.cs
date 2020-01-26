using HlyssUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Controllers.Tweens
{
    class TweenLinear : Tween
    {
        public TweenLinear() : base("linear") { }

        public override Tween Get()
        {
            return new TweenLinear();
        }

        public override void Update()
        {
            base.Update();

            timePassed += _deltaTime.Current;

            progress = timePassed / Duration;

            if (timePassed >= Duration)
            {
                progress = 1;
                Finish();
                return;
            }
        }
    }
}
