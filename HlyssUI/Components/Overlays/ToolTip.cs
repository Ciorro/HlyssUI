using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components.Overlays
{
    class ToolTip : Overlay
    {
        public string Text
        {
            get
            {
                return (Root.FindChild("tooltip_label") as Label).Text;
            }
            set
            {
                (Root.FindChild("tooltip_label") as Label).Text = value;
            }
        }

        public ToolTip(Gui gui) : base(gui)
        {
            Root.Children = new List<Component>()
            {
                new Panel()
                {
                    Name = "tooltip_panel",
                    Children = new List<Component>()
                    {
                        new Label()
                        {
                            Name = "tooltip_label"
                        }
                    }
                }
            };
        }
    }
}
