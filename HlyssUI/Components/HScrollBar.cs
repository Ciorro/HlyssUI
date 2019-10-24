using HlyssUI.Controllers.Tweens;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace HlyssUI.Components
{
    public class HScrollBar : Component
    {
        private RectangleShape _background;
        private RectangleShape _slider;
        private int _clickOffset;
        private bool _active = false;
        private TweenOut _tween = new TweenOut();

        public float Speed = 1;
        public float Percentage = 0;
        public int ContentWidth;
        public Keyboard.Key ScrollKey = Keyboard.Key.LShift;
        public Component Target = null;

        public HScrollBar(int contentWidth)
        {
            this.ContentWidth = contentWidth;

            _background = new RectangleShape();

            _slider = new RectangleShape();
            _slider.Position = (Vector2f)GlobalPosition;

            Width = "100%";
            Height = "15px";

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
                _slider.Size = new Vector2f(_slider.Size.X, (H - 2) * _tween.Percentage + 2);
                _slider.Position = new Vector2f(_slider.Position.X, GlobalPosition.Y + H - _slider.Size.Y);
            }
            else if (_tween.IsRunning)
            {
                Color bgColor = _background.FillColor;
                _background.FillColor = new Color(bgColor.R, bgColor.G, bgColor.B, (byte)(64 * (1 - _tween.Percentage)));
                _slider.Size = new Vector2f(_slider.Size.X, (H - 2) * (1 - _tween.Percentage) + 2);
                _slider.Position = new Vector2f(_slider.Position.X, GlobalPosition.Y + H - _slider.Size.Y);
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
                _clickOffset = (int)(mpos.X - _slider.Position.X);
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
            if (Keyboard.IsKeyPressed(ScrollKey) && Target != null && Target.Hovered)
            {
                _slider.Position += new Vector2f((TargetSize.X / 50) * scroll * Speed, 0) * -1;
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

            _tween.Start();
        }

        private float GetPercentageFromSliderPosition()
        {
            if (ContentWidth <= TargetSize.X)
            {
                return 0;
            }

            float sliderPos = _slider.Position.X - GlobalPosition.X;
            float mensurationWidth = TargetSize.X - _slider.Size.X;

            return sliderPos / mensurationWidth;
        }

        private void setSliderPosition(float percentage)
        {
            float mensurationWidth = TargetSize.X - _slider.Size.X;
            _slider.Position = new Vector2f(GlobalPosition.X + mensurationWidth * percentage, _slider.Position.Y);
        }

        private void UpdateSlider()
        {
            _slider.Size = new Vector2f((int)System.Math.Max(((float)TargetSize.X / ContentWidth) * TargetSize.X, TargetSize.X * 0.1f), _slider.Size.Y);

            if (_active == true)
            {
                Vector2i mpos = Mouse.GetPosition(App.Window);
                _slider.Position = new Vector2f(mpos.X - _clickOffset, _slider.Position.Y);
                KeepInbounds();
                Percentage = GetPercentageFromSliderPosition();

                ScheduleRefresh();
            }
            else
            {
                setSliderPosition(Percentage);
            }
        }

        private void KeepInbounds()
        {
            if (_slider.Position.X < GlobalPosition.X)
            {
                _slider.Position = new Vector2f(GlobalPosition.X, _slider.Position.Y);
            }
            if (_slider.Position.X + _slider.Size.X > GlobalPosition.X + TargetSize.X)
            {
                _slider.Position = new Vector2f(GlobalPosition.X + TargetSize.X - _slider.Size.X, _slider.Position.Y);
            }
        }
    }
}
