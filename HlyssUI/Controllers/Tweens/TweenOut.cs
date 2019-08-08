using HlyssUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Controllers.Tweens
{
    class TweenOut : Tween
    {
        public TweenOut() : base("out") { }

        public override void Update()
        {
            float offset = Math.Abs(100 - progress);

            if (Math.Abs(offset) < 0.01f)
            {
                progress = 100;
                Finish();
                return;
            }

            offset *= DeltaTime.Current * Speed;
            progress += offset;
        }
    }
}
