using HlyssUI.Extensions;
using HlyssUI.Layout;
using HlyssUI.Themes;
using System.Collections.Generic;

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
            {"primary-color", Theme.GetColor("accent").GetLegibleColor().ToHex() },
            {"position-ease", "out" }
        };

        protected readonly Style ToggleOffStyle = new Style()
        {
            {"primary-color", "secondary" },
            {"position-ease", "out" }
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

            _toggle = new Panel()
            {
                Width = "12px",
                Height = "12px",
                MarginTop = "4px",
                Hoverable = false
            };

            _body = new Panel()
            {
                Width = "40px",
                Height = "20px",
                MarginRight = "5px",
                Layout = LayoutType.Relative,
                Children = new List<Component>() { _toggle }
            };

            _label = new Label(label)
            {
                Margin = "2px"
            };

            Children = new List<Component>()
            {
                _body, _label
            };

            Autosize = true;
            IsToggled = false;

            DefaultStyle = new Style()
            {
                {"border-radius", int.MaxValue.ToString() }
            };
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
                _toggle.Left = "24px";

                _body.DefaultStyle = OnStyle;
                _body.HoverStyle = OnHoverStyle;
                _body.PressedStyle = OnPressedStyle;

                _toggle.DefaultStyle = ToggleOnStyle;
            }
            else
            {
                _toggle.Left = "4px";

                _body.DefaultStyle = OffStyle;
                _body.HoverStyle = OffHoverStyle;
                _body.PressedStyle = OffPressedStyle;

                _toggle.DefaultStyle = ToggleOffStyle;
            }
        }
    }
}
