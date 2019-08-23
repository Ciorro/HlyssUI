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

        public override void OnValueChanged()
        {
            tween.Start();
            _from = component.Size;
        }

        public override bool Update()
        {
            tween.Update();

            int width = (int)(_from.X + (component.TargetSize.X - _from.X) * tween.Percentage);
            int height = (int)(_from.Y + (component.TargetSize.Y - _from.Y) * tween.Percentage);

            component.Size = new Vector2i(width, height);

            return tween.IsRunning;
        }
    }
}
