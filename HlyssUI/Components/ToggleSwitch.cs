using HlyssUI.Layout;

namespace HlyssUI.Components
{
    public class ToggleSwitch : Box
    {
        public bool Toggled
        {
            get { return _toggled; }
            set
            {
                _toggled = value;
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

        public ToggleSwitch(GuiScene scene, string label = "") : base(scene)
        {
            Layout = LayoutType.Row;

            _body = new Panel(scene);
            _body.Width = "40px";
            _body.Height = "20px";
            _body.MarginRight = "5px";
            _body.CoverParent = false;
            AddChild(_body);

            _toggle = new Panel(scene);
            _toggle.Width = "12px";
            _toggle.Height = "12px";
            _toggle.Left = "3px";
            _toggle.Top = "3px";
            _toggle.CoverParent = false;
            _toggle.Style.Round = true;
            _body.AddChild(_toggle);

            _label = new Label(scene, label);
            _label.Margin = "2px";
            _label.CoverParent = false;
            AddChild(_label);

            Style.Round = true;
            CascadeStyle = true;
            Toggled = false;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            if (Toggled)
                Transition($"color: primary to accent darken 20");
            else
                Transition($"color: primary to primary darken 20");
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            if (Toggled)
                Transition($"color: primary to accent");
            else
                Transition($"color: primary to primary");
        }

        public override void OnPressed()
        {
            base.OnPressed();
            if (Toggled)
                Transition($"color: primary to accent darken 40");
            else
                Transition($"color: primary to primary darken 40");
        }

        public override void OnClicked()
        {
            base.OnClicked();
            Toggled = !Toggled;
        }

        private void toggle()
        {
            if(_toggled)
            {
                _toggle.Transition("position: to 24px 4px", "color: primary to ffffff", "color: secondary to ffffff");
                Transition("color: primary to accent", "color: secondary to accent");
            }
            else
            {
                _toggle.Transition("position: to 4px 4px", "color: primary to secondary", "color: secondary to secondary");
                Transition("color: primary to primary", "color: secondary to secondary");
            }
        }
    }
}
