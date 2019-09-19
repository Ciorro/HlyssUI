using HlyssUI.Themes;
using System;
using System.Collections.Generic;
using System.Text;

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

        private Icon _icon;
        private Label _label;

        public ListItem(string label = "")
        {
            _icon = new Icon(Graphics.Icons.Empty)
            {
                MarginRight = "10px",
                IconSize = 20
            };

            _label = new Label(label)
            {
                Height = "100%",
                AutosizeY = false
            };

            Padding = "10px";
            Width = "100%";
            AutosizeY = true;
            DisableClipping = false;

            DefaultStyle = new Style()
            {
                {"border-thickness", "0" }
            };

            HoverStyle = new Style()
            {
                {"primary-color", "primary -20" }
            };

            PressedStyle = new Style()
            {
                {"primary-color", "primary -40" }
            };
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            AddChild(_icon);
            AddChild(_label);
        }
    }
}
