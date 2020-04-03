using HlyssUI.Components.Interfaces;
using HlyssUI.Graphics;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class NavigationItem : ListItem, ISelectable
    {
        public event ISelectable.SelectedHandler OnSelect;
        public event ISelectable.UnselectedHandler OnUnselect;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    if (value)
                    {
                        UnmarkOthers();
                        OnSelect?.Invoke(this);
                    }
                    else
                    {
                        OnUnselect?.Invoke(this);
                    }

                    _isSelected = value;
                }
            }
        }

        public List<Component> Content { get; set; } = new List<Component>();

        private bool _isSelected = false;
        private RoundedRectangle _selection = new RoundedRectangle();

        public NavigationItem(string label, Icons icon) : base(label)
        {
            Icon = icon;
            Padding = "8px 0px";
        }

        public override void OnClicked()
        {
            base.OnClicked();
            IsSelected = true;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _selection.Position = (Vector2f)GlobalPosition;
            _selection.Size = new Vector2f(4, Size.Y);
            _selection.UpdateGeometry();
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _selection.FillColor = StyleManager.GetColor("accent-color");
            _selection.BorderRadius = new Layout.BorderRadius(0, 2, 2, 0);
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);

            if (IsSelected)
                target.Draw(_selection);
        }

        private void UnmarkOthers()
        {
            if (Parent == null)
                return;

            foreach (var parentChild in Parent.Children)
            {
                if (parentChild is ISelectable)
                {
                    ((ISelectable)parentChild).IsSelected = false;
                }
            }
        }
    }
}
