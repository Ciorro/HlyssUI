using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Controllers.Tweens
{
    class TweenInstant : Tween
    {
        public TweenInstant() : base("instant") { }

        public override Tween Get()
        {
            return new TweenInstant();
        }

        public override void Update()
        {
            if (progress == 100)
                Finish();

            progress = 100;
        }
    }
}
