using HlyssUI.Components.Internals;
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

        public bool IsToggled
        {
            get { return _toggle.Toggled; }
            set
            {
                Toggled?.Invoke(this, value);
                _toggle.Toggled = value;
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

        private Switch _toggle;
        private Label _label;

        public ToggleSwitch(string label = "")
        {
            Layout = LayoutType.Row;

            _toggle = new Switch()
            {
                Width = "40px",
                Height = "20px",
                MarginRight = "5px"
            };

            _label = new Label(label)
            {
                Margin = "2px"
            };

            Children = new List<Component>()
            {
                _toggle, _label
            };

            Autosize = true;
            IsToggled = false;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            _toggle.Hovered = true;
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            _toggle.Hovered = false;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsToggled = !IsToggled;
        }
    }
}
