using HlyssUI.Layout;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Layout.LayoutControllers
{
    static class LayoutResolver
    {
        private static List<LayoutController> _layoutControllers = new List<LayoutController>()
        {
            new RowLayoutController(),
            new ColumnLayoutController(),
            new WrapLayoutController()
        };

        public static LayoutController GetLayout(LayoutType type)
        {
            foreach (var controller in _layoutControllers)
            {
                if (controller.Type == type)
                    return controller.Get();
            }

            return new RowLayoutController();
        }
    }
}
