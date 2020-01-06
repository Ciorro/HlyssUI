using HlyssUI.Graphics;
using HlyssUI.Layout;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class ExpansionPanel : Panel
    {
        private bool _expanded = false;

        public string Header
        {
            get
            {
                Label header = FindChild("expansionpanel_header") as Label;
                if (header != null)
                    return header.Text;
                else return string.Empty;
            }
            set
            {
                Label header = FindChild("expansionpanel_header") as Label;
                if (header != null)
                    header.Text = value;
            }
        }

        public bool Expanded
        {
            get
            {
                return _expanded;
            }
            set
            {
                _expanded = value;
                Component content = GetChild("expansionpanel_content");
                Icon icon = FindChild("expansionpanel_icon") as Icon;

                if (value)
                {
                    content.Visible = true;
                    icon.IconType = Icons.AngleUp;

                    if (ExpandMargins)
                    {
                        MarginTop = "5px";
                        MarginBottom = "5px";
                    }

                    if (Unique)
                        FoldOtherPanels();
                }
                else
                {
                    content.Visible = false;
                    icon.IconType = Icons.AngleDown;

                    if (ExpandMargins)
                    {
                        MarginTop = "0px";
                        MarginBottom = "0px";
                    }
                }
            }
        }

        public bool ExpandMargins { get; set; } = true;
        public bool Unique { get; set; } = true;

        public ExpansionPanel()
        {
            Children = new List<Component>()
            {
                new Panel()
                {
                    Width = "100%",
                    Height = "40px",
                    Padding = "5px",
                    CenterContent = true,
                    Layout = LayoutType.Row,
                    Name = "expansionpanel_topbar",
                    Style = "expansion_panel_header_default",
                    Children = new List<Component>()
                    {
                        new Label()
                        {
                            Font = Fonts.MontserratMedium,
                            Name = "expansionpanel_header"
                        },
                        new Spacer(),
                        new Icon(Icons.AngleDown)
                        {
                            Name = "expansionpanel_icon"
                        }
                    }
                },
                new Component()
                {
                    Width = "100%",
                    Padding = "1px",
                    AutosizeY = true,
                    Overflow = OverflowType.Hidden,
                    Name = "expansionpanel_content"
                }
            };

            AutosizeY = true;
            SlotName = "expansionpanel_content";
            Layout = LayoutType.Column;
            Style = "expansion_panel_default";

            GetChild("expansionpanel_topbar").Clicked += (object sender) => Expanded = !Expanded;

            Expanded = false;
        }

        private void FoldOtherPanels()
        {
            foreach (var panel in Parent.Children)
            {
                if (panel is ExpansionPanel && panel != this)
                    (panel as ExpansionPanel).Expanded = false;
            }
        }
    }
}
