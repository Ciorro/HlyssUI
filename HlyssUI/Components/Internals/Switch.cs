using HlyssUI.Controllers.Tweens;
using HlyssUI.Graphics;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace HlyssUI.Components.Internals
{
    internal class Switch : Component
    {
        private RoundedRectangle _outerBody;
        private CircleShape _toggle;
        private TweenOut _tween;
        private bool _toggled = false;
        private int _toggledOffset = 0;

        private Dictionary<bool, string> _styles = new Dictionary<bool, string>()
        {
            {false, "toggle_off_default" },
            {true, "toggle_on_default" }
        };

        public bool Toggled
        {
            get { return _toggled; }
            set
            {
                _toggled = value;
                Style = _styles[value];
                _tween.Start();
            }
        }

        public Switch()
        {
            _outerBody = new RoundedRectangle()
            {
                OutlineThickness = -1
            };

            _toggle = new CircleShape();

            _tween = new TweenOut()
            {
                Duration = 0.2f
            };
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _outerBody.Size = (Vector2f)Size;
            _outerBody.Radius = (uint)Size.Y;
            _outerBody.UpdateGeometry();

            _toggle.Radius = (Size.Y / 2) * 0.6f;


            _outerBody.Position = (Vector2f)GlobalPosition;
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _outerBody.FillColor = StyleManager.GetColor("primary-color");
            _outerBody.OutlineColor = StyleManager.GetColor("secondary-color");

            _toggle.FillColor = StyleManager.GetColor("text-color");
        }

        public override void Update()
        {
            base.Update();

            _tween.Update();

            if(_toggled && _tween.IsRunning)
            {
                _toggledOffset = (int)(20 * _tween.Percentage);
            }
            else if(_tween.IsRunning)
            {
                _toggledOffset = 20 + (int)(20 * -_tween.Percentage);
            }

            int markOffset = (int)(_outerBody.Radius - _toggle.Radius);
            _toggle.Position = (Vector2f)GlobalPosition + new Vector2f(markOffset + _toggledOffset, markOffset);
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);

            target.Draw(_outerBody);
            target.Draw(_toggle);
        }
    }
}
