using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class ToolTip : Panel
    {
        public string Text
        {
            get { return (FindChild("tooltip_text") as Label).Text; }
            set
            {
                (FindChild("tooltip_text") as Label).Text = value;
            }
        }

        public Vector2i Offset { get; set; } = new Vector2i(15, 15);

        public ToolTip()
        {
            Padding = "5px";
            Autosize = true;
            Name = "tooltip_panel";

            Children = new List<Component>()
            {
                new Label()
                {
                    Name = "tooltip_text"
                }
            };

            DefaultStyle = new Themes.Style()
            {
                {"character-size", "12" }
            };
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            System.Console.WriteLine("Tooltip Init");
        }

        public override void OnMouseMovedAnywhere(Vector2i location)
        {
            base.OnMouseMovedAnywhere(location);

            //if (Initialized && App.Root.Children.Contains(this))
             //   App.Root.Children.Remove(this);
        }
    }
}
