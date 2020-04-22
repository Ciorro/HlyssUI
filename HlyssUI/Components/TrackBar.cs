using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace HlyssUI.Components
{
    public class TrackBar : Component
    {
        public delegate void Confirmed(object sender, int value);
        public event Confirmed OnConfirmed;

        public int Value { get; set; }

        private RectangleShape _bar;
        private RectangleShape _valueBar;
        private CircleShape _pointer;
        private bool _active = false;
        private int _currentOffset = 0;

        public int MaxValue = 100;

        public TrackBar()
        {
            _bar = new RectangleShape();
            _valueBar = new RectangleShape();

            _pointer = new CircleShape(7);
            _pointer.OutlineThickness = -2;

            Value = 0;
        }

        public override void Draw(RenderTarget target)
        {
            Form.Window.Draw(_bar);
            Form.Window.Draw(_valueBar);
            Form.Window.Draw(_pointer);
        }

        public override void OnPressed(Mouse.Button button)
        {
            base.OnPressed(button);
            _active = true;
        }

        public override void Update()
        {
            base.Update();

            _bar.Position = (Vector2f)GlobalPosition + new Vector2f(0, (int)(Size.Y / 2f));
            _bar.Size = new Vector2f(Size.X, 2);

            _valueBar.Position = (Vector2f)GlobalPosition + new Vector2f(0, (int)(Size.Y / 2f));
            _valueBar.Size = new Vector2f(_currentOffset, 2);

            if (_active == true)
            {
                int offset = Mouse.GetPosition(Form.Window).X - GlobalPosition.X;
                offset = setOffsetInbounds(offset);
                _currentOffset = offset;
                Value = OffsetToValue(_currentOffset);
            }
            else
                _currentOffset = ValueToOffset(Value);

            _pointer.Position = new Vector2f(GlobalPosition.X + _currentOffset - _pointer.Radius, _bar.Position.Y - _pointer.Radius + 1);

            if (Hovered || _active)
            {
                _pointer.OutlineColor = StyleManager.GetColor("accent-color");
            }
            else
            {
                _pointer.OutlineColor = StyleManager.GetColor("secondary-color");
            }

            _bar.FillColor = StyleManager.GetColor("secondary-color");
            _valueBar.FillColor = StyleManager.GetColor("accent-color");
            _pointer.FillColor = StyleManager.GetColor("primary-color");
        }

        private int setOffsetInbounds(int offset)
        {
            if (offset > Size.X)
            {
                offset = Size.X;
            }
            if (offset < 0)
            {
                offset = 0;
            }

            return offset;
        }

        public override void OnMouseReleasedAnywhere(Vector2i location, Mouse.Button button)
        {
            if (_active == true)
                OnConfirmed?.Invoke(this, Value);

            _active = false;
        }

        private int ValueToOffset(int value)
        {
            return (int)(value / ((float)MaxValue / Size.X));
        }

        private int OffsetToValue(int offset)
        {
            return (int)((offset / (float)Size.X) * MaxValue);
        }
    }
}
