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
        private Clock _animationClock = new Clock();

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

            _slider.Size = new Vector2f(_slider.Size.X, TargetSize.Y);

            updateSlider();
            keepInbounds();
        }

        public override void OnPressed()
        {
            Vector2i mpos = Mouse.GetPosition(Gui.Window);

            if (_slider.GetGlobalBounds().Contains(mpos.X, mpos.Y))
            {
                _clickOffset = (int)(mpos.X - _slider.Position.X);
                _active = true;

                ScheduleRefresh();
            }
        }

        public override void OnMouseReleasedAnywhere(Vector2i location, Mouse.Button button)
        {
            _active = false;
        }

        public override void OnScrolledAnywhere(float scroll)
        {
            Vector2i mPos = Mouse.GetPosition(Gui.Window);

            if (Keyboard.IsKeyPressed(ScrollKey) && ((Target == null && Hovered) || ((Target != null && Target.Bounds.Contains(mPos.X, mPos.Y)) || Hovered)))
            {
                _slider.Position += new Vector2f((TargetSize.X / 50) * scroll * Speed, 0) * -1;
                keepInbounds();
                Percentage = getPercentageFromSliderPosition();

                ScheduleRefresh();
            }
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            Color bgColor = Style.GetColor("secondary-color");
            bgColor.A = 64;
            _background.FillColor = bgColor;

            _slider.FillColor = Style.GetColor("secondary-color");
        }

        private float getPercentageFromSliderPosition()
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
            _slider.Position = new Vector2f(GlobalPosition.X + mensurationWidth * percentage, GlobalPosition.Y);
        }

        private void updateSlider()
        {
            _slider.Size = new Vector2f((int)System.Math.Max(((float)TargetSize.X / ContentWidth) * TargetSize.X, TargetSize.X * 0.1f), _slider.Size.Y);

            if (_active == true)
            {
                Vector2i mpos = Mouse.GetPosition(Gui.Window);
                _slider.Position = new Vector2f(mpos.X - _clickOffset, GlobalPosition.Y);
                keepInbounds();
                Percentage = getPercentageFromSliderPosition();

                ScheduleRefresh();
            }
            else
            {
                setSliderPosition(Percentage);
            }
        }

        private void keepInbounds()
        {
            if (_slider.Position.X < GlobalPosition.X)
            {
                _slider.Position = new Vector2f(GlobalPosition.X, GlobalPosition.Y);
            }
            if (_slider.Position.X + _slider.Size.X > GlobalPosition.X + TargetSize.X)
            {
                _slider.Position = new Vector2f(GlobalPosition.X + TargetSize.X - _slider.Size.X, GlobalPosition.Y);
            }
        }
    }
}
