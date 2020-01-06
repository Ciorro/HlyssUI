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

        #region Styles

        //protected readonly Style OnStyle = new Style()
        //{
        //    {"primary-color", "accent" },
        //    {"border-thickness", "0" },
        //    {"border-radius", int.MaxValue.ToString() }
        //};

        //protected readonly Style OffStyle = Style.EmptyStyle;

        //protected readonly Style OnHoverStyle = new Style()
        //{
        //    {"primary-color", "accent -20" }
        //};

        //protected readonly Style OffHoverStyle = new Style()
        //{
        //    {"primary-color", "primary -20" }
        //};

        //protected readonly Style OnPressedStyle = new Style()
        //{
        //    {"primary-color", "accent -40" }
        //};

        //protected readonly Style OffPressedStyle = new Style()
        //{
        //    {"primary-color", "primary -40" }
        //};

        //protected readonly Style ToggleOnStyle = new Style()
        //{
        //    {"primary-color", Theme.GetColor("accent").GetLegibleColor().ToHex() },
        //    {"position-ease", "out" }
        //};

        //protected readonly Style ToggleOffStyle = new Style()
        //{
        //    {"primary-color", "secondary" },
        //    {"position-ease", "out" }
        //};
        #endregion

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
