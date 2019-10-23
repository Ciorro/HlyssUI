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
            _from = component.Position;
        }

        public override bool Update()
        {
            bool isRunning = tween.IsRunning;

            if (isRunning)
            {
                tween.Update();

                int x = (int)(_from.X + (component.TargetPosition.X - _from.X) * tween.Percentage);
                int y = (int)(_from.Y + (component.TargetPosition.Y - _from.Y) * tween.Percentage);

                component.Position = new Vector2i(x, y);
            }

            return isRunning;
        }

        protected override void UpdateTween()
        {
            base.UpdateTween();

            if (component.DefaultStyle.ContainsKey("position-ease") && component.DefaultStyle.GetString("position-ease") != tween.Name)
            {
                TweenType = component.DefaultStyle.GetString("position-ease");
            }

            if (component.DefaultStyle.ContainsKey("position-ease-duration") && component.DefaultStyle.GetString("position-ease-duration") != tween.Name)
            {
                tween.Duration = component.DefaultStyle.GetFloat("position-ease-duration");
            }
        }
    }
}
