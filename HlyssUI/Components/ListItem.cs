using HlyssUI.Layout;
using HlyssUI.Themes;
using System;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class ListItem : Panel
    {
        public string Label
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        public Graphics.Icons Icon
        {
            set { _icon.IconType = value; }
        }

        public Action Action { get; set; }

        private Icon _icon;
        private Label _label;

        public ListItem(string label = "")
        {
            _icon = new Icon(Graphics.Icons.Empty)
            {
                MarginRight = "10px",
                Name = "listitem_icon"
            };

            _label = new Label(label)
            {
                Height = "100%",
                AutosizeToText = false,
                Name = "listitem_text"
            };

            Children = new List<Component>()
            {
                _icon, _label
            };

            Padding = "10px";
            Width = "100%";
            AutosizeY = true;
            CenterContent = true;
            Overflow = OverflowType.Hidden;
            Style = "list_item_default";
        }

        public override void OnClicked()
        {
            base.OnClicked();
            Action?.Invoke();
        }
    }
}
