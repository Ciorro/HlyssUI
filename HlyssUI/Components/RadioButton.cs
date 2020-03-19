using HlyssUI.Components.Interfaces;
using HlyssUI.Components.Internals;
using HlyssUI.Layout;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class RadioButton : Component, ISelectable
    {
        public event ISelectable.SelectedHandler OnSelect;
        public event ISelectable.UnselectedHandler OnUnselect;

        public bool IsSelected
        {
            get { return _mark.Marked; }
            set
            {
                if (value != _mark.Marked)
                {
                    if (value)
                    {
                        UnmarkOthers();
                        OnSelect?.Invoke(this);
                    }
                    else
                        OnUnselect?.Invoke(this);

                    _mark.Marked = value;
                }
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
            IsSelected = false;
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
            IsSelected = true;
        }

        private void UnmarkOthers()
        {
            if (Parent == null)
                return;

            foreach (var parentChild in Parent.Children)
            {
                if (parentChild is ISelectable && parentChild != this)
                {
                    ((ISelectable)parentChild).IsSelected = false;
                }
            }
        }
    }
}
