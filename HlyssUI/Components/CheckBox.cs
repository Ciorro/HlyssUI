using HlyssUI.Layout;
using SFML.Graphics;

namespace HlyssUI.Components
{
    public class CheckBox : Box
    {
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if(value)
                {
                    Transition("color: primary to accent");
                    _box.Transition("color: secondary to accent");
                    _check.Visible = true;
                }
                else
                {
                    Transition("color: primary to primary");
                    _box.Transition("color: secondary to secondary");
                    _check.Visible = false;
                }

                _checked = value;
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

        private Card _box;
        private Icon _check;
        private Label _label;
        private bool _checked;

        public CheckBox(GuiScene scene, string label = "") : base(scene)
        {
            Layout = LayoutType.Row;

            _box = new Card(scene);
            _box.Padding = "2px";
            _box.MarginRight = "5px";
            AddChild(_box);

            _check = new Icon(scene, Utils.Icons.Check);
            _check.CoverParent = false;
            _check.CascadeStyle = true;
            _check.Visible = false;
            _check.Style["text"] = Themes.Style.GetLegibleColor(Style["accent"]);
            _box.AddChild(_check);

            _label = new Label(scene, label);
            _label.Margin = "2px";
            AddChild(_label);

            CascadeStyle = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            if(Checked)
                Transition($"color: primary to accent darken 20");
            else
                Transition($"color: primary to primary darken 20");
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            if(Checked)
                Transition($"color: primary to accent");
            else
                Transition($"color: primary to primary");
        }

        public override void OnPressed()
        {
            base.OnPressed();
            if(Checked)
                Transition($"color: primary to accent darken 40");
            else
                Transition($"color: primary to primary darken 40");
        }

        public override void OnReleased()
        {
            base.OnReleased();
            if(Checked)
                Transition($"color: primary to accent darken 20");
            else
                Transition($"color: primary to primary darken 20");

        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
            child.CoverParent = false;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            Checked = !Checked;
        }
    }
}
