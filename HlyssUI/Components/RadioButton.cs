using HlyssUI.Layout;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class RadioButton : Box
    {
        public bool Marked
        {
            get { return _marked; }
            set
            {
                if (value)
                {
                    _box.Transition("color: secondary to accent");
                    _mark.Transition("color: primary to accent");

                    unmarkOthers();
                }
                else
                {
                    _mark.Transition("color: primary to transparent");
                    _box.Transition("color: secondary to secondary");
                }

                _marked = value;
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
        private Panel _mark;
        private Label _label;
        private bool _marked;

        public RadioButton(GuiScene scene, string label = "") : base(scene)
        {
            Layout = LayoutType.Row;
            Style.Round = true;

            _box = new Card(scene);
            _box.Padding = "4px";
            _box.MarginRight = "5px";
            AddChild(_box);

            _mark = new Panel(scene);
            _mark.CoverParent = false;
            _mark.CascadeStyle = true;
            _mark.Style.Round = true;
            _mark.Width = "12px";
            _mark.Height = "12px";
            _mark.Style["secondary"] = Themes.Theme.GetColor("transparent");
            _mark.Style["primary"] = Themes.Theme.GetColor("transparent");
            _box.AddChild(_mark);

            _label = new Label(scene, label);
            _label.Margin = "2px";
            AddChild(_label);

            CascadeStyle = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Transition($"color: primary to primary darken 20");
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            Transition($"color: primary to primary");
        }

        public override void OnPressed()
        {
            base.OnPressed();
            Transition($"color: primary to primary darken 40");
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
            child.CoverParent = false;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            Marked = true;
        }

        private void unmarkOthers()
        {
            if (Parent == null)
                return;

            List<RadioButton> others = new List<RadioButton>();

            foreach (var parentChild in Parent.Children)
            {
                if (parentChild.GetType() == GetType() && parentChild != this)
                {
                    ((RadioButton)parentChild).Marked = false;
                }
            }
        }
    }
}
