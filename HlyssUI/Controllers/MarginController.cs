using HlyssUI.Components;
using HlyssUI.Layout;

namespace HlyssUI.Controllers
{
    class MarginController : Controller
    {
        private Spacing _from = new Spacing();

        public MarginController(Component component) : base(component)
        {
        }

        public override void Start()
        {
            UpdateTween();

            tween.Start();
            _from = component.Margins;
        }

        public override bool Update()
        {
            bool isRunning = tween.IsRunning;

            if (isRunning)
            {
                tween.Update();

                int left = (int)(_from.Left + (component.TargetMargins.Left - _from.Left) * tween.Percentage);
                int right = (int)(_from.Right + (component.TargetMargins.Right - _from.Right) * tween.Percentage);
                int top = (int)(_from.Top + (component.TargetMargins.Top - _from.Top) * tween.Percentage);
                int bottom = (int)(_from.Bottom + (component.TargetMargins.Bottom - _from.Bottom) * tween.Percentage);

                component.Margins = new Spacing(left, right, top, bottom);
            }

            return isRunning;
        }

        protected override void UpdateTween()
        {
            if (component.StyleManager.GetString("margin-ease") != tween.Name)
            {
                TweenType = component.StyleManager.GetString("margin-ease");
            }

            if (component.StyleManager.GetString("margin-ease-duration") != tween.Name)
            {
                tween.Duration = component.StyleManager.GetFloat("margin-ease-duration");
            }
        }
    }
}
