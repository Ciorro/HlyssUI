using System.Collections.Generic;

namespace HlyssUI.Layout.LayoutControllers
{
    static class LayoutResolver
    {
        private static List<LayoutController> _layoutControllers = new List<LayoutController>()
        {
            new RowLayoutController(),
            new ColumnLayoutController(),
            new WrapLayoutController(),
            new RelativeLayoutController(),
            new ScrollLayoutController()
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
