using HlyssUI.Layout;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class RadioButton : Component
    {
        public delegate void MarkHandler(object sender, bool isToggled);
        public event MarkHandler Marked;

        private Dictionary<bool, string> _styles = new Dictionary<bool, string>()
        {
            {false, "radio_button_off_default" },
            {true, "radio_button_on_default" }
        };

        public bool IsMarked
        {
            get { return _marked; }
            set
            {
                if (value)
                {
                    UnmarkOthers();
                }

                Style = _styles[value];

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
            //TODO: Radio button nie z paneli tylko wlasny component :D\

            Layout = LayoutType.Row;

            _mark = new Panel()
            {
                Width = "12px",
                Height = "12px"
            };

            _box = new Panel()
            {
                Autosize = true,
                Padding = "4px",
                MarginRight = "5px",
                Children = new List<Component>() { _mark }
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
            IsMarked = false;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            //Style.SetValue("primary-color", "primary -20");
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            //Style.SetValue("primary-color", "primary");
        }

        public override void OnPressed(Mouse.Button button)
        {
            base.OnPressed(button);
            //Style.SetValue("primary-color", "primary -40");
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
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
