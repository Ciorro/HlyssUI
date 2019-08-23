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

        public override void OnValueChanged()
        {
            tween.Start();
            _from = component.Position;
        }

        public override bool Update()
        {
            if (tween.IsRunning)
            {
                tween.Update();

                int x = (int)(_from.X + (component.TargetPosition.X - _from.X) * tween.Percentage);
                int y = (int)(_from.Y + (component.TargetPosition.Y - _from.Y) * tween.Percentage);

                component.Position = new Vector2i(x, y);
            }

            return tween.IsRunning;
        }
    }
}
