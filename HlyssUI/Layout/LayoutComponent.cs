using HlyssUI.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Layout
{
    public abstract class LayoutComponent : Component
    {
        public bool LayoutChanged;
        public abstract void RefreshLayout();
    }
}
