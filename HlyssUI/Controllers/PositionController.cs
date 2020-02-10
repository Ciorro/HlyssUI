using HlyssUI.Components;
using SFML.System;

namespace HlyssUI.Controllers
{
    class PositionController : Controller
    {
        private Vector2i _from = new Vector2i();

        public PositionController(Component component) : base(component)
        {
        }

        public override void Start()
        {
            UpdateTween();

            tween.Start();
            _from = component.RelativePosition;
        }

        public override bool Update()
        {
            bool isRunning = tween.IsRunning;

            if (isRunning)
            {
                tween.Update();

                int x = (int)(_from.X + (component.TargetRelativePosition.X - _from.X) * tween.Percentage);
                int y = (int)(_from.Y + (component.TargetRelativePosition.Y - _from.Y) * tween.Percentage);

                component.RelativePosition = new Vector2i(x, y);
            }

            return isRunning;
        }

        protected override void UpdateTween()
        {
            if (component.StyleManager.GetString("position-ease") != tween.Name)
            {
                TweenType = component.StyleManager.GetString("position-ease");
            }

            if (component.StyleManager.GetString("position-ease-duration") != tween.Name)
            {
                tween.Duration = component.StyleManager.GetFloat("position-ease-duration");
            }
        }
    }
}
