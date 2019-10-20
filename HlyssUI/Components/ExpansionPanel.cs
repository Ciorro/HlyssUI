﻿using HlyssUI.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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

                    if(ExpandMargins)
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
                    Layout = HlyssUI.Layout.LayoutType.Row,
                    Name = "expansionpanel_topbar",
                    Children = new List<Component>()
                    {
                        new Label()
                        {
                            Font = Fonts.MontserratMedium,
                            Name = "expansionpanel_header",
                            DefaultStyle = new Themes.Style()
                            {
                                {"character-size", "15" }
                            }
                        },
                        new Spacer(),
                        new Icon(Icons.AngleDown)
                        {
                            Name = "expansionpanel_icon"
                        }
                    },
                    DefaultStyle = new Themes.Style()
                    {
                        {"primary-color", "secondary +20" }
                    }
                },
                new Component()
                {
                    Width = "100%",
                    Padding = "1px",
                    AutosizeY = true,
                    Name = "expansionpanel_content"
                }
            };

            AutosizeY = true;
            SlotName = "expansionpanel_content";
            Layout = HlyssUI.Layout.LayoutType.Column;
            DisableClipping = false;

            DefaultStyle = new Themes.Style()
            {
                {"size-ease","out" }
            };

            GetChild("expansionpanel_topbar").Clicked += (object sender) => Expanded = !Expanded;

            Expanded = false;
        }

        private void FoldOtherPanels()
        {
            List<ExpansionPanel> panels = new List<ExpansionPanel>();

            foreach (var panel in Parent.Children)
            {
                if (panel is ExpansionPanel)
                    panels.Add(panel as ExpansionPanel);
            }

            foreach (var panel in panels)
            {
                if (panel != this)
                    panel.Expanded = false;
            }
        }
    }
}
