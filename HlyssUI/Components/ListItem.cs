using HlyssUI.Themes;
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
                AutosizeToText = false
            };

            Children = new List<Component>()
            {
                _icon, _label
            };

            Padding = "5px 10px";
            Width = "100%";
            AutosizeY = true;
            DisableClipping = false;

            DefaultStyle = new Style()
            {
                {"border-thickness", "0" }
            };

            HoverStyle = new Style()
            {
                {"primary-color", "secondary" }
            };

            PressedStyle = new Style()
            {
                {"primary-color", "secondary -20" }
            };
        }
    }
}
