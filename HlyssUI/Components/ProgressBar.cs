using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Components
{
    public class ProgressBar : Component
    {
        public int Value
        {
            get { return _value; }
            set
            {
                this._value = value;
                if (value > MaxValue)
                {
                    this._value = MaxValue;
                }
            }
        }

        public float Percentage
        {
            get { return (float)Value / (float)MaxValue; }
        }

        public int MaxValue { get; set; } = 100;

        private RectangleShape _background;
        private RectangleShape _fill;
        private int _value = 0;

        public ProgressBar()
        {
            _background = new RectangleShape();
            _fill = new RectangleShape();

            Width = "200px";
            Height = "2px";
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _background.Position = (Vector2f)GlobalPosition;
            _background.Size = (Vector2f)TargetSize;

            _fill.Position = (Vector2f)GlobalPosition;
            _fill.Size = new Vector2f(TargetSize.X * Percentage, TargetSize.Y);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _background.FillColor = Style.GetDarker(Style["Primary"], 20);
            _fill.FillColor = Style["Accent"];
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_background);
            target.Draw(_fill);
        }
    }
}
