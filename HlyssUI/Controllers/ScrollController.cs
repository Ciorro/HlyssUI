using HlyssUI.Components;
using HlyssUI.Controllers.Tweens;
using SFML.System;
using HlyssUI.Extensions;

namespace HlyssUI.Controllers
{
    class ScrollController : Controller
    {
        private Vector2i _from = new Vector2i();
        private Vector2i _to = new Vector2i();

        public ScrollController(Component component) : base(component) { }

        public override void Start()
        {
            if (component.Overflow == Layout.OverflowType.Scroll)
            {
                UpdateTween();

                _from = component.ScrollOffset;
                _to = component.TargetScrollOffset;

                tween.Start();
            }
        }

        public override bool Update()
        {
            bool isRunning = tween.IsRunning && component.Overflow == Layout.OverflowType.Scroll;

            if (isRunning)
            {
                tween.Update();

                int x = (int)(_from.X + (_to.X - _from.X) * tween.Percentage);
                int y = (int)(_from.Y + (_to.Y - _from.Y) * tween.Percentage);

                component.ScrollOffset = new Vector2i(x, y);
            }

            return isRunning;
        }

        protected override void UpdateTween()
        {
            bool smooth = component.StyleManager.GetBool("smooth-scroll");
            bool isSmooth = tween is TweenOut;

            if (isSmooth != smooth)
            {
                TweenType = (smooth) ? "out" : "instant";
                tween.Duration = 0.5f;
            }
        }
    }
}
