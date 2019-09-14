using HlyssUI.Extensions;
using HlyssUI.Layout;
using HlyssUI.Themes;

namespace HlyssUI.Components
{
    public class ToggleSwitch : Component
    {
        public delegate void ToggleHandler(object sender, bool isToggled);
        public event ToggleHandler Toggled;

        #region Styles

        protected readonly Style OnStyle = new Style()
        {
            {"primary-color", "accent" },
            {"border-thickness", "0" },
            {"border-radius", int.MaxValue.ToString() }
        };

        protected readonly Style OffStyle = Style.EmptyStyle;

        protected readonly Style OnHoverStyle = new Style()
        {
            {"primary-color", "accent -20" }
        };

        protected readonly Style OffHoverStyle = new Style()
        {
            {"primary-color", "primary -20" }
        };

        protected readonly Style OnPressedStyle = new Style()
        {
            {"primary-color", "accent -40" }
        };

        protected readonly Style OffPressedStyle = new Style()
        {
            {"primary-color", "primary -40" }
        };

        protected readonly Style ToggleOnStyle = new Style()
        {
            {"primary-color", Theme.GetColor("accent").GetLegibleColor().ToHex() }
        };

        protected readonly Style ToggleOffStyle = new Style()
        {
            {"primary-color", "secondary" }
        };
        #endregion

        public bool IsToggled
        {
            get { return _toggled; }
            set
            {
                _toggled = value;
                Toggled?.Invoke(this, value);
                toggle();
            }
        }

        public string Label
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;
            }
        }

        private bool _toggled;
        private Panel _body;
        private Panel _toggle;
        private Label _label;

        public ToggleSwitch(string label = "")
        {
            Layout = LayoutType.Row;
            _body = new Panel();
            _toggle = new Panel();
            _label = new Label(label);

            Autosize = true;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            _body.Width = "40px";
            _body.Height = "20px";
            _body.MarginRight = "5px";
            AddChild(_body);

            _toggle.Width = "12px";
            _toggle.Height = "12px";
            _toggle.MarginLeft = "4px";
            _toggle.MarginTop = "4px";
            _toggle.Transition = "out";
            _toggle.Style.SetValue("round", true);
            _body.AddChild(_toggle);
            _toggle.Hoverable = false;
            _toggle.Name = "toggle";

            _label.Margin = "2px";
            AddChild(_label);

            IsToggled = false;

            DefaultStyle = DefaultStyle.Combine(new Style()
            {
                {"border-radius", int.MaxValue.ToString() }
            });
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsToggled = !IsToggled;
        }

        private void toggle()
        {
            if (_toggled)
            {
                _toggle.MarginLeft = "24px";

                _body.DefaultStyle = OnStyle;
                _body.HoverStyle = OnHoverStyle;
                _body.PressedStyle = OnPressedStyle;

                _toggle.DefaultStyle = ToggleOnStyle;
            }
            else
            {
                _toggle.MarginLeft = "4px";

                _body.DefaultStyle = OffStyle;
                _body.HoverStyle = OffHoverStyle;
                _body.PressedStyle = OffPressedStyle;

                _toggle.DefaultStyle = ToggleOffStyle;
            }
        }
    }
}
