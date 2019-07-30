using HlyssUI.Components;
using SFML.System;
using SFML.Window;
using System.Diagnostics;

namespace HlyssUI.Updaters
{
    class PositionUpdater
    {
        private GuiScene _scene;
        private bool _needsRefresh = false;

        public PositionUpdater(GuiScene scene)
        {
            _scene = scene;
        }

        public void Update(Component baseComponent)
        {
            Scan(baseComponent);

            if (_needsRefresh)
            {
                _needsRefresh = false;
                RefreshComponents(_scene.BaseNode);
            }
        }

        private void Scan(Component baseComponent)
        {
            if (baseComponent.NeedsRefresh)
            {
                refresh(baseComponent);
                _needsRefresh = true;
                return;
            }

            foreach (var child in baseComponent.Children)
            {
                Scan(child);
            }
        }

        private void RefreshComponents(Component component)
        {
            refresh(component);

            foreach (var child in component.Children)
            {
                RefreshComponents(child);
            }
        }

        static int i = 0;

        private void refresh(Component component)
        {
            Vector2i parentPos = component.Parent != null ? component.Parent.GlobalPosition : new Vector2i();
            Vector2i parentPad = component.Parent != null ? new Vector2i(component.Parent.Pl, component.Parent.Pt) : new Vector2i();

            int x = component.X;
            int y = component.Y;

            component.GlobalPosition = new Vector2i(x + component.Ml, y + component.Mt) + parentPad + parentPos;
            component.Size = new Vector2i(component.W, component.H);
            component.Position = new Vector2i(x, y);

            component.OnRefresh();
        }
    }
}
