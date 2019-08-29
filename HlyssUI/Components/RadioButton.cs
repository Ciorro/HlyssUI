using HlyssUI.Layout;
using HlyssUI.Themes;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class RadioButton : Component
    {
        public bool Marked
        {
            get { return _marked; }
            set
            {
                if (value)
                {
                    _box.Style["secondary"] = Theme.GetColor("accent");
                    _mark.Style["primary"] = Theme.GetColor("accent");

                    unmarkOthers();
                }
                else
                {
                    _mark.Style["primary"] = Theme.GetColor("transparent");
                    _box.Style["secondary"] = Theme.GetColor("secondary");
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

        private Panel _box;
        private Panel _mark;
        private Label _label;
        private bool _marked;

        public RadioButton(string label = "")
        {
            Layout = LayoutType.Row;
            Style.Round = true;

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
            _mark.Style.Round = true;
            _mark.Width = "12px";
            _mark.Height = "12px";
            _mark.Style["secondary"] = Themes.Theme.GetColor("transparent");
            _mark.Style["primary"] = Themes.Theme.GetColor("transparent");
            _box.AddChild(_mark);

            _label.Margin = "2px";
            AddChild(_label);

            CascadeStyle = true;
            
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 20);
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            Style["primary"] = Theme.GetColor("primary");

            
        }

        public override void OnPressed()
        {
            base.OnPressed();
            Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 40);

            
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
