using System;
using System.Collections.Generic;
using System.Text;
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

        public override void OnValueChanged()
        {
            tween.Start();
            _from = component.Margins;
        }

        public override bool Update()
        {
            tween.Update();

            int left = (int)(_from.Left + (component.TargetMargins.Left - _from.Left) * tween.Percentage);
            int right = (int)(_from.Right + (component.TargetMargins.Right - _from.Right) * tween.Percentage);
            int top = (int)(_from.Top + (component.TargetMargins.Top - _from.Top) * tween.Percentage);
            int bottom = (int)(_from.Bottom + (component.TargetMargins.Bottom - _from.Bottom) * tween.Percentage);

            component.Margins = new Spacing(left, right, top, bottom);

            return tween.IsRunning;
        }
    }
}
