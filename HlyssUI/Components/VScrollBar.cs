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
        private Clock _animationClock = new Clock();

        public float Speed = 1;
        public float Percentage = 0;
        public int ContentHeight;
        public Component Target = null;

        public VScrollBar(GuiScene scene, int contentHeight) : base(scene)
        {
            this.ContentHeight = contentHeight;

            _background = new RectangleShape();

            _slider = new RectangleShape();
            _slider.Position = (Vector2f)GlobalPosition;

            Width = "15px";
            Height = "200px";
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_background);
            target.Draw(_slider);
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _background.Size = (Vector2f)Size;
            _background.Position = (Vector2f)GlobalPosition;

            _slider.Size = new Vector2f(Size.X, _slider.Size.Y);

            updateSlider();
            keepInbounds();
        }

        public override void OnPressed()
        {
            Vector2i mpos = Mouse.GetPosition(Gui.Window);

            if (_slider.GetGlobalBounds().Contains(mpos.X, mpos.Y))
            {
                _clickOffset = (int)(mpos.Y - _slider.Position.Y);
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

            if ((Target == null && Hovered) || ((Target != null && Target.Bounds.Contains(mPos.X, mPos.Y)) || Hovered))
            {
                _slider.Position += new Vector2f(0, (Size.Y / 50) * scroll * Speed) * -1;
                keepInbounds();
                Percentage = getPercentageFromSliderPosition();

                ScheduleRefresh();
            }
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            Color bgColor = Style["secondary"];
            bgColor.A = 64;
            _background.FillColor = bgColor;

            _slider.FillColor = Style["secondary"];
        }

        public float getPercentageFromSliderPosition()
        {
            if (ContentHeight <= Size.Y)
            {
                return 0;
            }

            float sliderPos = _slider.Position.Y - GlobalPosition.Y;
            float mensurationWidth = Size.Y - _slider.Size.Y;

            return sliderPos / mensurationWidth;
        }

        public void setSliderPosition(float percentage)
        {
            float mensurationWidth = Size.Y - _slider.Size.Y;
            _slider.Position = new Vector2f(GlobalPosition.X, GlobalPosition.Y + mensurationWidth * percentage);
        }

        private void updateSlider()
        {
            _slider.Size = new Vector2f(_slider.Size.X, (int)System.Math.Max(((float)Size.Y / ContentHeight) * Size.Y, Size.Y * 0.1f));

            if (_active == true)
            {
                Vector2i mpos = Mouse.GetPosition(Gui.Window);
                _slider.Position = new Vector2f(GlobalPosition.X, mpos.Y - _clickOffset);
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
            if (_slider.Position.Y < GlobalPosition.Y)
            {
                _slider.Position = new Vector2f(GlobalPosition.X, GlobalPosition.Y);
            }
            if (_slider.Position.Y + _slider.Size.Y > GlobalPosition.Y + Size.Y)
            {
                _slider.Position = new Vector2f(GlobalPosition.X, GlobalPosition.Y + Size.Y - _slider.Size.Y);
            }
        }
    }
}
