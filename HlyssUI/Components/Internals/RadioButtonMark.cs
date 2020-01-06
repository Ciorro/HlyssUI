using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace HlyssUI.Components.Internals
{
    internal class RadioButtonMark : Component
    {
        private CircleShape _outerBody;
        private CircleShape _mark;
        private bool _marked = false;

        private Dictionary<bool, string> _styles = new Dictionary<bool, string>()
        {
            {false, "radio_button_off_default" },
            {true, "radio_button_on_default" }
        };

        public bool Marked
        {
            get { return _marked; }
            set
            {
                Style = _styles[value];
                _marked = value;
            }
        }

        public RadioButtonMark()
        {
            _outerBody = new CircleShape()
            {
                OutlineThickness = -1
            };
            _mark = new CircleShape();
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _outerBody.Radius = Size.X / 2;
            _mark.Radius = (Size.X / 2) * 0.6f;

            int markOffset = (int)(_outerBody.Radius - _mark.Radius);

            _outerBody.Position = (Vector2f)GlobalPosition;
            _mark.Position = (Vector2f)GlobalPosition + new Vector2f(markOffset, markOffset);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _outerBody.FillColor = StyleManager.GetColor("primary-color");
            _outerBody.OutlineColor = StyleManager.GetColor("secondary-color");

            _mark.FillColor = StyleManager.GetColor("secondary-color");
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);

            target.Draw(_outerBody);
            if (Marked)
                target.Draw(_mark);
        }
    }
}
