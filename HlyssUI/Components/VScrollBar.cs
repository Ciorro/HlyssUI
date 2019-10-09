using HlyssUI.Controllers.Tweens;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace HlyssUI.Components
{
    public class VScrollBar : Component
    {
        private RectangleShape _background;
        private RectangleShape _slider;
        private int _clickOffset;
        private bool _active = false;
        private TweenOut _tween = new TweenOut();

        public float Speed = 1;
        public float Percentage = 0;
        public int ContentHeight;
        public Component Target = null;

        public VScrollBar(int contentHeight)
        {
            this.ContentHeight = contentHeight;

            _background = new RectangleShape();

            _slider = new RectangleShape();
            _slider.Position = (Vector2f)GlobalPosition;

            Width = "15px";
            Height = "100%";

            _tween.Start();
        }

        public override void Update()
        {
            base.Update();

            _tween.Update();

            if ((Hovered || _active) && _tween.IsRunning)
            {
                Color bgColor = _background.FillColor;
                _background.FillColor = new Color(bgColor.R, bgColor.G, bgColor.B, (byte)(64 * _tween.Percentage));
                _slider.Size = new Vector2f((W - 2) * _tween.Percentage + 2, _slider.Size.Y);
                _slider.Position = new Vector2f(GlobalPosition.X + W - _slider.Size.X, _slider.Position.Y);
            }
            else if (_tween.IsRunning)
            {
                Color bgColor = _background.FillColor;
                _background.FillColor = new Color(bgColor.R, bgColor.G, bgColor.B, (byte)(64 * (1 - _tween.Percentage)));
                _slider.Size = new Vector2f((W - 2) * (1 - _tween.Percentage) + 2, _slider.Size.Y);
                _slider.Position = new Vector2f(GlobalPosition.X + W - _slider.Size.X, _slider.Position.Y);
            }
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_background);
            target.Draw(_slider);
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _background.Size = (Vector2f)TargetSize;
            _background.Position = (Vector2f)GlobalPosition;

            UpdateSlider();
            KeepInbounds();
        }

        public override void OnPressed()
        {
            Vector2i mpos = Mouse.GetPosition(App.Window);

            if (_slider.GetGlobalBounds().Contains(mpos.X, mpos.Y))
            {
                _clickOffset = (int)(mpos.Y - _slider.Position.Y);
                _active = true;

                ScheduleRefresh();
            }
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();

            if (!_active)
                _tween.Start();
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();

            if (!_active)
                _tween.Start();
        }

        public override void OnMouseReleasedAnywhere(Vector2i location, Mouse.Button button)
        {
            if (_active && !Hovered)
                _tween.Start();
            _active = false;
        }

        public override void OnScrolledAnywhere(float scroll)
        {
            Vector2i mPos = Mouse.GetPosition(App.Window);

            if ((Target == null && Hovered) || ((Target != null && Target.Bounds.Contains(mPos.X, mPos.Y)) || Hovered))
            {
                _slider.Position += new Vector2f(0, (TargetSize.Y / 50) * scroll * Speed) * -1;
                KeepInbounds();
                Percentage = GetPercentageFromSliderPosition();

                ScheduleRefresh();
            }
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _background.FillColor = Theme.GetColor("999999");
            _slider.FillColor = Theme.GetColor("999999");
        }

        public float GetPercentageFromSliderPosition()
        {
            if (ContentHeight <= TargetSize.Y)
            {
                return 0;
            }

            float sliderPos = _slider.Position.Y - GlobalPosition.Y;
            float mensurationWidth = TargetSize.Y - _slider.Size.Y;

            return sliderPos / mensurationWidth;
        }

        public void SetSliderPosition(float percentage)
        {
            float mensurationWidth = TargetSize.Y - _slider.Size.Y;
            _slider.Position = new Vector2f(_slider.Position.X, GlobalPosition.Y + mensurationWidth * percentage);
        }

        private void UpdateSlider()
        {
            _slider.Size = new Vector2f(_slider.Size.X, (int)System.Math.Max(((float)TargetSize.Y / ContentHeight) * TargetSize.Y, TargetSize.Y * 0.1f));

            if (_active == true)
            {
                Vector2i mpos = Mouse.GetPosition(App.Window);
                _slider.Position = new Vector2f(_slider.Position.X, mpos.Y - _clickOffset);
                KeepInbounds();
                Percentage = GetPercentageFromSliderPosition();

                ScheduleRefresh();
            }
            else
            {
                SetSliderPosition(Percentage);
            }
        }

        private void KeepInbounds()
        {
            if (_slider.Position.Y < GlobalPosition.Y)
            {
                _slider.Position = new Vector2f(_slider.Position.X, GlobalPosition.Y);
            }
            if (_slider.Position.Y + _slider.Size.Y > GlobalPosition.Y + TargetSize.Y)
            {
                _slider.Position = new Vector2f(_slider.Position.X, GlobalPosition.Y + TargetSize.Y - _slider.Size.Y);
            }
        }
    }
}
