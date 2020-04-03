using HlyssUI.Graphics;
using HlyssUI.Layout;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Components
{
    public class NavigationView : Component
    {
        public const string FoldModeNavWidth = "48px";

        public string Header
        {
            get { return (FindChild("navigation_header") as ListItem).Label; }
            set
            {
                (FindChild("navigation_header") as ListItem).Label = value;
            }
        }
        public string ExpandedModeWidth
        {
            get { return _expandedModeWidth; }
            set
            {
                if (_expandedModeWidth != value)
                {
                    if (IsExpanded)
                        Width = value;

                    _expandedModeWidth = value;
                }
            }
        }

        public bool ExpandOnHover { get; set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {

                    FindChild("navigation_panel").Width = value ? ExpandedModeWidth : FoldModeNavWidth;
                }

                _isExpanded = value;
            }
        }

        public bool DisplayHeader
        {
            get { return FindChild("navigation_header").Visible; }
            set
            {
                FindChild("navigation_header").Visible = value;
            }
        }

        public bool FixedMode
        {
            get { return _fixedMode; }
            set
            {
                if (value != _fixedMode)
                {
                    FindChild("navigation_view").PositionType = value ? PositionType.Fixed : PositionType.Static;

                    if (value)
                        FindChild("navigation_panel").AddStyle("navigation_panel_fixed_mode_default");
                    else
                        FindChild("navigation_panel").RemoveStyle("navigation_panel_fixed_mode_default");

                    _fixedMode = value;
                }
            }
        }

        public List<Component> Items
        {
            set
            {
                Component items = FindChild("items");
                items.Children = value;

                foreach (var item in value)
                {
                    if (item is NavigationItem)
                    {
                        (item as NavigationItem).Action = (_) =>
                        {
                            FindChild("content_view").Children = (_ as NavigationItem).Content;
                        };
                    }
                }
            }
        }

        public uint SelectedItem
        {
            get { return (uint)Math.Abs(_selectedItem); }
            set
            {
                if (_selectedItem != value)
                {
                    List<NavigationItem> items = FindChild("items").Children.Where(i => i is NavigationItem).Cast<NavigationItem>().ToList();

                    if (value < items.Count)
                    {
                        items[(int)value].IsSelected = true;
                        FindChild("content_view").Children = items[(int)value].Content;
                    }

                    _selectedItem = (int)value;
                }
            }
        }

        private int _selectedItem = -1;
        private bool _fixedMode = false;
        private bool _isExpanded = false;
        private string _expandedModeWidth = "270px";

        public NavigationView()
        {
            Width = "100%";
            Height = "100%";

            Children = new List<Component>()
            {
                new Component()
                {
                    AutosizeX = true,
                    Height = "100%",
                    Name = "navigation_view",
                    Children = new List<Component>()
                    {
                        new Panel()
                        {
                            Width = FoldModeNavWidth,
                            Height = "100%",
                            Style = "navigation_panel_default",
                            Name = "navigation_panel",
                            Layout = LayoutType.Column,
                            Overflow = OverflowType.Hidden,
                            Children = new List<Component>()
                            {
                                new NavigationHeader()
                                {
                                    Action = (_) => IsExpanded = !IsExpanded,
                                    Name = "navigation_header",
                                    MarginBottom = "10px"
                                },
                                new Component()
                                {
                                    Width = "100%",
                                    Expand = true,
                                    Name = "items",
                                    Layout = HlyssUI.Layout.LayoutType.Column,
                                    Overflow = HlyssUI.Layout.OverflowType.Scroll
                                }
                            }
                        }
                    }
                },
                new Component()
                {
                    Expand = true,
                    Height = "100%",
                    Name = "content_view",
                    Style = "navigation_content_view_default"
                }
            };

            IsExpanded = true;
        }

        public override void OnKeyPressed(Keyboard.Key key)
        {
            base.OnKeyPressed(key);

            if (key == Keyboard.Key.F1)
                FixedMode = !FixedMode;
        }
    }

    class NavigationHeader : ListItem
    {
        public NavigationHeader() : base(string.Empty)
        {
            (FindChild("listitem_label") as Label).Font = Fonts.MontserratSemiBold;
            Padding = "8px 0px";
            Icon = Icons.Bars;
            AutosizeY = false;
            Height = "41px";
        }
    }
}
