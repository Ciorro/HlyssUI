using HlyssUI.Graphics;
using HlyssUI.Layout;
using System;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class ListItem : Panel
    {
        public string Label
        {
            get { return (FindChild("listitem_label") as Label).Text; }
            set
            {
                Label label = FindChild("listitem_label") as Label;

                if (label.Text != value)
                    label.Text = value;
            }
        }

        public Icons Icon
        {
            set
            {
                Icon icon = FindChild("listitem_icon") as Icon;

                if (icon.IconType != value)
                    icon.IconType = value;
            }
        }

        public string IconWidth
        {
            get { return _iconWidth; }
            set
            {
                if(_iconWidth != value)
                {
                    FindChild("listitem_icon_container").Width = value;
                    _iconWidth = value;
                }
            }
        }

        public Action<object> Action { get; set; }

        private string _iconWidth = "48px";

        public ListItem(string label = "")
        {
            Width = "100%";
            AutosizeY = true;
            CenterContent = true;
            Style = "list_item_default";
            Padding = "5px 0px";

            Children = new List<Component>()
            {
                new Component()
                {
                    Width = "48px",
                    AutosizeY = true,
                    CenterContent = true,
                    Layout = LayoutType.Column,
                    Name = "listitem_icon_container",
                    Children = new List<Component>()
                    {
                        new Icon(Icons.Empty)
                        {
                            Name = "listitem_icon",
                            Style = "list_item_icon_default"
                        }
                    }
                },
                new Label(label)
                {
                    Name = "listitem_label",
                    MarginLeft = "10px"
                }
            };
        }

        public override void OnClicked()
        {
            base.OnClicked();
            Action?.Invoke(this);
        }
    }
}
