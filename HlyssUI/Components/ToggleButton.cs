using HlyssUI.Components.Interfaces;

namespace HlyssUI.Components
{
    public class ToggleButton : Button, ISelectable
    {
        public event ISelectable.SelectedHandler OnSelect;
        public event ISelectable.UnselectedHandler OnUnselect;

        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                    Appearance = value ? ButtonStyle.Filled : ButtonStyle.Outline;

                _isSelected = value;
            }
        }

        public ToggleButton(string label) : base(label) { }

        public override void OnClicked()
        {
            base.OnClicked();
            IsSelected = !IsSelected;
        }
    }
}
