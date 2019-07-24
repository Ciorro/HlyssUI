using HlyssUI.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Updaters
{
    class StyleUpdater
    {
        public void Update(Component baseNode)
        {
            foreach (var child in baseNode.Children)
            {
                if (baseNode.CascadeColor)
                    child.Style = baseNode.Style;

                child.OnStyleChanged();
                Update(child);
            }
        }
    }
}
