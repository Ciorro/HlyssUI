using HlyssUI.Components;
using SFML.System;

namespace HlyssUI.Controllers
{
    class SizeController : Controller
    {
        private Vector2i _from = new Vector2i();

        public SizeController(Component component) : base(component)
        {
        }

        public override void Start()
        {
            UpdateTween();
            tween.Start();
            _from = component.Size;
        }

        public override bool Update()
        {
            bool isRunning = tween.IsRunning;

            if (isRunning)
            {
                tween.Update();

                int width = (int)(_from.X + (component.TargetSize.X - _from.X) * tween.Percentage);
                int height = (int)(_from.Y + (component.TargetSize.Y - _from.Y) * tween.Percentage);

                component.Size = new Vector2i(width, height);
            }

            return isRunning;
        }

        protected override void UpdateTween()
        {
            base.UpdateTween();

            if (component.DefaultStyle.ContainsKey("size-ease") && component.DefaultStyle.GetString("size-ease") != tween.Name)
            {
                TweenType = component.DefaultStyle.GetString("size-ease");
            }
        }
    }
}
