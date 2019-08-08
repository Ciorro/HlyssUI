using HlyssUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Controllers.Tweens
{
    class TweenIn : Tween
    {
        public TweenIn() : base("in") { }

        public override void Update()
        {
            //TODO: Implements transition in

            float offset = Math.Abs(100 - progress);

            if (Math.Abs(offset) < 1)
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
