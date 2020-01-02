using HlyssUI.Components;
using HlyssUI.Layout;

namespace HlyssUI.Controllers
{
    class PaddingController : Controller
    {
        private Spacing _from = new Spacing();

        public PaddingController(Component component) : base(component)
        {
        }

        public override void Start()
        {
            if (component.StyleChanged)
                UpdateTween();

            tween.Start();
            _from = component.Paddings;
        }

        public override bool Update()
        {
            bool isRunning = tween.IsRunning;

            if (isRunning)
            {
                tween.Update();

                int left = (int)(_from.Left + (component.TargetPaddings.Left - _from.Left) * tween.Percentage);
                int right = (int)(_from.Right + (component.TargetPaddings.Right - _from.Right) * tween.Percentage);
                int top = (int)(_from.Top + (component.TargetPaddings.Top - _from.Top) * tween.Percentage);
                int bottom = (int)(_from.Bottom + (component.TargetPaddings.Bottom - _from.Bottom) * tween.Percentage);

                component.Paddings = new Spacing(left, right, top, bottom);
            }

            return isRunning;
        }

        protected override void UpdateTween()
        {
            base.UpdateTween();

            //if (component.DefaultStyle.ContainsKey("padding-ease") && component.DefaultStyle.GetString("padding-ease") != tween.Name)
            //{
            //    TweenType = component.DefaultStyle.GetString("padding-ease");
            //}

            //if (component.DefaultStyle.ContainsKey("padding-ease-duration") && component.DefaultStyle.GetString("padding-ease-duration") != tween.Name)
            //{
            //    tween.Duration = component.DefaultStyle.GetFloat("padding-ease-duration");
            //}
        }
    }
}
