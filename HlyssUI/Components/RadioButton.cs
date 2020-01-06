using HlyssUI.Components.Internals;
using HlyssUI.Layout;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class RadioButton : Component
    {
        public delegate void MarkHandler(object sender, bool isToggled);
        public event MarkHandler Marked;

        public bool IsMarked
        {
            get { return _mark.Marked; }
            set
            {
                if (value)
                {
                    UnmarkOthers();
                }

                _mark.Marked = value;

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

        private RadioButtonMark _mark;
        private Label _label;

        public RadioButton(string label = "")
        {
            Layout = LayoutType.Row;

            _mark = new RadioButtonMark()
            {
                Width = "20px",
                Height = "20px",
                MarginRight = "5px"
            };

            _label = new Label(label)
            {
                Margin = "2px"
            };

            Children = new List<Component>()
            {
                _mark, _label
            };

            Autosize = true;
            IsMarked = false;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            _mark.Hovered = true;
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            _mark.Hovered = false;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsMarked = true;
        }

        private void UnmarkOthers()
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
