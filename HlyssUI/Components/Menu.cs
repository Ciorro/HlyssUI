﻿using HlyssUI.Layout;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Components
{
    public class Menu : Flyout
    {
        public List<MenuItem> Items
        {
            get { return (FindChild("menu_item_list") as ScrollArea2).SlotContent[0].Children.Cast<MenuItem>().ToList(); }
            set { (FindChild("menu_item_list") as ScrollArea2).SlotContent[0].Children = value.Cast<Component>().ToList(); }
        }

        public Menu()
        {
            Width = "200px";
            Layout = LayoutType.Column;
            Padding = "1px";

            Children = new List<Component>()
            {
                new ScrollArea2()
                {
                    Name = "menu_item_list",
                    Width = "100%",
                    AutosizeY = true,
                    SlotContent = new List<Component>()
                    { 
                        new Component()
                        {
                            Width = "100%",
                            AutosizeY = true,
                            Layout = LayoutType.Column
                        }
                    }
                }
            };

            FindChild("scrollarea_content").AutosizeY = true;
            GetChild("menu_item_list").Slot.AutosizeY = true;
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
                    FindChild("menu_item_list").Height = $"{App.Root.TargetSize.Y}px";
                    FindChild("menu_item_list").AutosizeY = false;
                    Top = "0px";
                }
                else
                {
                    FindChild("menu_item_list").AutosizeY = true;
                }
            }
        }
    }
}
