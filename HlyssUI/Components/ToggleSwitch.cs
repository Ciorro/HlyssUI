using HlyssUI.Components.Interfaces;
using HlyssUI.Components.Internals;
using HlyssUI.Extensions;
using HlyssUI.Layout;
using HlyssUI.Themes;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class ToggleSwitch : Component, ISelectable
    {
        public delegate void ToggleHandler(object sender, bool isToggled);
        public event ToggleHandler OnToggle;

        public event ISelectable.SelectedHandler OnSelect;
        public event ISelectable.UnselectedHandler OnUnselect;

        public bool IsSelected
        {
            get { return _toggle.Toggled; }
            set
            {
                if (value != _toggle.Toggled)
                {
                    if (value)
                        OnSelect?.Invoke(this);
                    else
                        OnUnselect?.Invoke(this);

                    OnToggle?.Invoke(this, value);
                    _toggle.Toggled = value;
                    Label = value ? OnContent : OffContent;
                }
            }
        }

        public string OnContent { get; set; }
        public string OffContent { get; set; }

        private string Label
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;
            }
        }

        private Switch _toggle;
        private Label _label;

        public ToggleSwitch(string onContent = "", string offContent = "")
        {
            OnContent = onContent;
            OffContent = string.IsNullOrEmpty(offContent) ? onContent : offContent;

            Layout = LayoutType.Row;
            Autosize = true;

            _toggle = new Switch()
            {
                Width = "40px",
                Height = "20px",
                MarginRight = "5px",
                Toggled = false
            };

            _label = new Label(offContent)
            {
                Margin = "2px"
            };

            Children = new List<Component>()
            {
                _toggle, _label
            };
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            _toggle.Hovered = true;
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            _toggle.Hovered = false;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsSelected = !IsSelected;
        }
    }
}
