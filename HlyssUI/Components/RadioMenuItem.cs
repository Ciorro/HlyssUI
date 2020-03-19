using HlyssUI.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class RadioMenuItem : MenuItem, ISelectable
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
                {
                    if (value)
                    {
                        Icon = Graphics.Icons.DotCircle;
                        UnmarkOthers();
                        OnSelect?.Invoke(this);
                    }
                    else
                    {
                        Icon = Graphics.Icons.Circle;
                        OnUnselect?.Invoke(this);
                    }

                    _isSelected = value;
                }
            }
        }

        public RadioMenuItem(string label = "") : base(label) 
        {
            Icon = Graphics.Icons.Circle;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsSelected = !IsSelected;
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
