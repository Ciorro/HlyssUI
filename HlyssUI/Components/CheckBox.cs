using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;

namespace HlyssUI.Components
{
    public class CheckBox : Component
    {
        public delegate void CheckHandler(object sender, bool isToggled);
        public event CheckHandler Checked;

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
                    Style["primary"] = Theme.GetColor("accent");
                    _box.Style["secondary"] = Theme.GetColor("accent");
                    _check.Visible = true;
                }
                else
                {
                    Style["primary"] = Theme.GetColor("primary");
                    _box.Style["secondary"] = Theme.GetColor("secondary");
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

            _check.CoverParent = false;
            _check.CascadeStyle = true;
            _check.Visible = false;
            _check.Style["text"] = Style.GetLegibleColor(Style["accent"]);
            _box.AddChild(_check);

            _label.Margin = "2px";
            AddChild(_label);

            CascadeStyle = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            if (IsChecked)
                Style["primary"] = Style.GetDarker(Theme.GetColor("accent"), 20);
            else
                Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 20);


        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            if (IsChecked)
                Style["primary"] = Theme.GetColor("accent");
            else
                Style["primary"] = Theme.GetColor("primary");


        }

        public override void OnPressed()
        {
            base.OnPressed();
            if (IsChecked)
                Style["primary"] = Style.GetDarker(Theme.GetColor("accent"), 40);
            else
                Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 40);


        }

        public override void OnReleased()
        {
            base.OnReleased();
            if (IsChecked)
                Style["primary"] = Style.GetDarker(Theme.GetColor("accent"), 20);
            else
                Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 20);



        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
            child.CoverParent = false;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsChecked = !IsChecked;
        }
    }
}
