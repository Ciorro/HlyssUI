using HlyssUI.Layout;
using HlyssUI.Themes;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class RadioButton : Component
    {
        public delegate void MarkHandler(object sender, bool isToggled);
        public event MarkHandler Marked;

        public bool IsMarked
        {
            get { return _marked; }
            set
            {
                if (value)
                {
                    _box.Style.SetValue("secondary-color", "accent");
                    _mark.Style.SetValue("opacity", 1);

                    unmarkOthers();
                }
                else
                {
                    _box.Style.SetValue("secondary-color", "secondary");
                    _mark.Style.SetValue("opacity", 0);
                }

                _marked = value;
                Marked?.Invoke(this, value);
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

        private Panel _box;
        private Panel _mark;
        private Label _label;
        private bool _marked;

        public RadioButton(string label = "")
        {
            Layout = LayoutType.Row;
            Style.SetValue("round", true);

            _box = new Panel();
            _box.Autosize = true;
            _mark = new Panel();
            _label = new Label(label);

            Autosize = true;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            _box.Padding = "4px";
            _box.MarginRight = "5px";
            AddChild(_box);

            _mark.CoverParent = false;
            _mark.CascadeStyle = true;
            _mark.Width = "12px";
            _mark.Height = "12px";
            _mark.Style.SetValue("round", true);
            _mark.Style.SetValue("opacity", 0);
            _box.AddChild(_mark);

            _label.Margin = "2px";
            AddChild(_label);

            CascadeStyle = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Style.SetValue("primary-color", "primary -20");
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            Style.SetValue("primary-color", "primary");
        }

        public override void OnPressed()
        {
            base.OnPressed();
            Style.SetValue("primary-color", "primary -40");
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
            child.CoverParent = false;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsMarked = true;
        }

        private void unmarkOthers()
        {
            if (Parent == null)
                return;

            foreach (var parentChild in Parent.Children)
            {
                if (parentChild.GetType() == GetType() && parentChild != this)
                {
                    ((RadioButton)parentChild).IsMarked = false;
                }
            }
        }
    }
}
