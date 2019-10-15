using HlyssUI.Layout;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Components
{
    public class Menu : Flyout
    {
        public List<ListItem> Items
        {
            get { return Children.Cast<ListItem>().ToList(); }
            set { Children = value.Cast<Component>().ToList(); }
        }

        public Menu()
        {
            Width = "200px";
            Layout = LayoutType.Column;
            Padding = "1px";
        }

        protected override void OnShown()
        {
            base.OnShown();
            AutosizeY = true;
        }

        protected override void OnHidden()
        {
            base.OnHidden();
            AutosizeY = false;
            Height = "0px";
        }
    }
}
