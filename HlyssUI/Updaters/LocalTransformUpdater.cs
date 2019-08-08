using HlyssUI.Components;
using HlyssUI.Layout;
using SFML.System;

namespace HlyssUI.Updaters
{
    class LocalTransformUpdater
    {
        public void Update(Component baseComponent)
        {
            Scan(baseComponent);
        }

        private void Scan(Component baseComponent)
        {
            if (baseComponent.TransformChanged)
            {
                refresh(baseComponent);
            }

            foreach (var child in baseComponent.Children)
            {
                Scan(child);
            }
        }

        private void refresh(Component component)
        {
            component.Size = new Vector2i(component.W, component.H);
            component.Position = new Vector2i(component.X, component.Y);
            component.Margins = new Spacing(component.Ml, component.Mr, component.Mt, component.Mb);
            component.Paddings = new Spacing(component.Pl, component.Pr, component.Pt, component.Pb);
        }
    }
}
