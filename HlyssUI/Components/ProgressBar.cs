using HlyssUI.Controllers.Tweens;
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

        public bool Intermediate
        {
            get { return _intermediate; }
            set
            {
                _intermediate = value;
                _isExpanding = true;
                _tweenEase = new TweenIn();
                _tweenEase.Start();
            }
        }

        public int MaxValue { get; set; } = 100;

        private RectangleShape _background;
        private RectangleShape _fill;
        private int _value = 0;
        private bool _intermediate;

        private Tween _tweenEase = new TweenIn();
        private bool _isExpanding;

        public ProgressBar()
        {
            _background = new RectangleShape();
            _fill = new RectangleShape();

            Width = "200px";
            Height = "2px";
        }

        public override void Update()
        {
            base.Update();
            HandleIntermediate();
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _background.FillColor = Style.GetColor("primary -20");
            _fill.FillColor = Style.GetColor("accent-color");
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_background);
            target.Draw(_fill);
        }

        private void HandleIntermediate()
        {
            if (Intermediate)
            {
                _tweenEase.Update();

                if (_isExpanding)
                {
                    _fill.Position = (Vector2f)GlobalPosition;
                    _fill.Size = new Vector2f(TargetSize.X * _tweenEase.Percentage, TargetSize.Y);

                    if (_tweenEase.IsFinished)
                    {
                        _isExpanding = false;
                        _tweenEase = new TweenOut()
                        {
                            Speed = 12
                        };
                        _tweenEase.Start();
                    }
                }
                else
                {
                    _fill.Size = new Vector2f(TargetSize.X - (TargetSize.X * _tweenEase.Percentage), TargetSize.Y);
                    _fill.Position = new Vector2f(GlobalPosition.X + (TargetSize.X * _tweenEase.Percentage), GlobalPosition.Y);

                    if (_tweenEase.IsFinished)
                    {
                        _isExpanding = true;
                        _tweenEase = new TweenIn();
                        _tweenEase.Start();
                    }
                }
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _background.Position = (Vector2f)GlobalPosition;
            _background.Size = (Vector2f)TargetSize;

            if (!Intermediate)
            {
                _fill.Position = (Vector2f)GlobalPosition;
                _fill.Size = new Vector2f(TargetSize.X * Percentage, TargetSize.Y);
            }
        }
    }
}
