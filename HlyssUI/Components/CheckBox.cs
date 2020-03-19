using HlyssUI.Components.Interfaces;
using HlyssUI.Extensions;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class CheckBox : Component, ISelectable
    {
        public event ISelectable.SelectedHandler OnSelect;
        public event ISelectable.UnselectedHandler OnUnselect;

        private Dictionary<bool, string> _styles = new Dictionary<bool, string>()
        {
            {false, "checkbox_off_default" },
            {true, "checkbox_on_default" }
        };

        private Panel _box;
        private Icon _check;
        private Label _label;
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _box.Style = _styles[value];

                    if (value)
                        OnSelect?.Invoke(this);
                    else
                        OnUnselect?.Invoke(this);
                }

                _isSelected = value;
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

            _check = new Icon(Icons.Check);

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
            IsSelected = false;
            _box.Style = _styles[false];
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsSelected = !IsSelected;
        }
    }
}
