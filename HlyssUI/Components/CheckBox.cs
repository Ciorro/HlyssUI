using HlyssUI.Extensions;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;

namespace HlyssUI.Components
{
    public class CheckBox : Component
    {
        public delegate void CheckHandler(object sender, bool isToggled);
        public event CheckHandler Checked;

        #region Styles

        protected readonly Style OutlineDefault = Style.DefaultStyle;

        protected readonly Style OutlineHover = new Style()
        {
            {"primary-color", "primary -20" }
        };

        protected readonly Style OutlinePressed = new Style()
        {
            {"primary-color", "primary -40" }
        };

        protected readonly Style FillDefault = new Style()
        {
            {"primary-color", "accent" },
            {"border-thickness", "0" },
            {"text-color", Theme.GetColor("accent").GetLegibleColor().ToHex() }
        };

        protected readonly Style FillHover = new Style()
        {
            {"primary-color", "accent +40" }
        };

        protected readonly Style FillPressed = new Style()
        {
            {"primary-color", "accent +20" }
        };

        protected readonly Style FlatDefault = new Style()
        {
            {"border-thickness", "0" }
        };

        protected readonly Style FlatHover = new Style()
        {
            {"primary-color", "secondary -20" }
        };

        protected readonly Style FlatPressed = new Style()
        {
            {"primary-color", "secondary -40" }
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
                    DefaultStyle = FillDefault;
                    HoverStyle = FillHover;
                    PressedStyle = FillPressed;

                    _check.Visible = true;
                }
                else
                {
                    DefaultStyle = Style.DefaultStyle;
                    HoverStyle = Style.DefaultStyle;
                    PressedStyle = Style.DefaultStyle;

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

            _box = new Panel();
            _box.Autosize = true;
            _check = new Icon(Icons.Check);
            _label = new Label(label);

            Autosize = true;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            _box.Padding = "2px";
            _box.MarginRight = "5px";
            AddChild(_box);

            _check.CascadeStyle = true;
            _check.Visible = false;
            //_check.Style["text"] = Style.GetLegibleColor(Style["accent"]);
            _box.AddChild(_check);

            _label.Margin = "2px";
            AddChild(_label);

            CascadeStyle = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            if (IsChecked)
                Style.SetValue("primary-color", "accent -20");
            else
                Style.SetValue("primary-color", "primary -20");
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            if (IsChecked)
                Style.SetValue("primary-color", "accent");
            else
                Style.SetValue("primary-color", "primary");
        }

        public override void OnPressed()
        {
            base.OnPressed();
            if (IsChecked)
                Style.SetValue("primary-color", "accent -40");
            else
                Style.SetValue("primary-color", "primary -40");
        }

        public override void OnReleased()
        {
            base.OnReleased();
            if (IsChecked)
                Style.SetValue("primary-color", "accent -20");
            else
                Style.SetValue("primary-color", "primary -20");
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsChecked = !IsChecked;
        }
    }
}
