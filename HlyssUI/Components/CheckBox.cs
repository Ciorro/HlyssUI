using HlyssUI.Extensions;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class CheckBox : Component
    {
        public delegate void CheckHandler(object sender, bool isChecked);
        public event CheckHandler Checked;

        #region Styles

        protected readonly Style OnStyle = new Style()
        {
            {"primary-color", "accent" },
            {"text-color", Theme.GetColor("accent").GetLegibleColor().ToHex() },
            {"border-thickness", "0" }
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
        #endregion

        private Panel _box;
        private Icon _check;
        private Label _label;
        private bool _checked;

        public bool IsChecked
        {
            get { return _checked; }
            set
            {
                if (value)
                {
                    _box.DefaultStyle = OnStyle;
                    _box.HoverStyle = OnHoverStyle;
                    _box.PressedStyle = OnPressedStyle;

                    _check.Visible = true;
                }
                else
                {
                    _box.DefaultStyle = OffStyle;
                    _box.HoverStyle = OffHoverStyle;
                    _box.PressedStyle = OffPressedStyle;

                    _check.Visible = false;
                }

                _checked = value;
                Checked?.Invoke(this, value);
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

        public CheckBox(string label = "")
        {
            Layout = LayoutType.Row;

            _check = new Icon(Icons.Check)
            {
                Visible = false
            };

            _box = new Panel()
            {
                Autosize = true,
                Padding = "2px",
                MarginRight = "5px",
                Children = new List<Component>() { _check }
            };

            _label = new Label(label)
            {
                Margin = "2px"
            };

            Children = new List<Component>()
            {
                _box, _label
            };

            Autosize = true;
            IsChecked = false;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsChecked = !IsChecked;
        }
    }
}
