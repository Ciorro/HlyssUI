using HlyssUI.Layout;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Components
{
    public class Menu : Flyout
    {
        public List<MenuItem> Items
        {
            get { return Children.Cast<MenuItem>().ToList(); }
            set { Children = value.Cast<Component>().ToList(); }
        }

        public Menu()
        {
            Width = "200px";
            Layout = LayoutType.Column;
            Overflow = OverflowType.Scroll;
            Padding = "5px 1px";
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

        public override void OnClicked()
        {
            base.OnClicked();
            Hide();
        }

        protected override void FitInWindow()
        {
            base.FitInWindow();

            if (Visible)
            {
                if (TargetSize.Y > App.Root.TargetSize.Y)
                {
                    Height = $"{App.Root.TargetSize.Y}px";
                    AutosizeY = false;
                    Top = "0px";
                }
                else
                {
                    AutosizeY = true;
                }
            }
        }
    }
}
