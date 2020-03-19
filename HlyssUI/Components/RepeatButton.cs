using SFML.System;
using SFML.Window;

namespace HlyssUI.Components
{
    public class RepeatButton : Button
    {
        public uint Delay { get; set; } = 500;
        public uint Interval { get; set; } = 33;

        private Clock _clock = new Clock();
        private bool _firstClick = true;

        public RepeatButton(string label = "") : base(label) { }

        public override void Update()
        {
            base.Update();

            if (IsPressed)
            {
                if (!_firstClick && _clock.ElapsedTime.AsMilliseconds() >= Interval)
                {
                    OnClicked();
                    _clock.Restart();
                }
                else if (_clock.ElapsedTime.AsMilliseconds() >= Delay)
                {
                    OnClicked();
                    _firstClick = false;
                    _clock.Restart();
                }
            }
            else
                _clock.Restart();
        }

        public override void OnReleased(Mouse.Button button)
        {
            base.OnReleased(button);
            _firstClick = true;
        }
    }
}
