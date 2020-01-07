using HlyssUI.Components;
using SFML.System;

namespace HlyssUI.Controllers
{
    class ScrollController : Controller
    {
        private Vector2i _from = new Vector2i();
        private bool _contentOverflowsComponent = false;

        public ScrollController(Component component) : base(component) { }

        public override void Start()
        {
            if (component.Overflow == Layout.OverflowType.Scroll)
            {
                tween.Start();
                _from = component.ScrollOffset;
            }
        }

        public override bool Update()
        {
            bool isRunning = tween.IsRunning && component.Overflow == Layout.OverflowType.Scroll;

            if (isRunning)
            {
                tween.Update();

                int x = (int)(_from.X + (component.TargetScrollOffset.X - _from.X) * tween.Percentage);
                int y = (int)(_from.Y + (component.TargetScrollOffset.Y - _from.Y) * tween.Percentage);

                component.ScrollOffset = new Vector2i(x, y);
            }

            return isRunning;
        }
    }
}
