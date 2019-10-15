using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class ToolTip : Panel
    {
        public Component Target { get; set; }

        private Clock _clock = new Clock();

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

        public override void Update()
        {
            base.Update();

            if(Target != null && Target.Hovered && _clock.ElapsedTime.AsMilliseconds() > 1000 && !Visible)
            {
                Visible = true;
                _clock.Restart();
                Left = $"{Mouse.GetPosition(App.Window).X + Offset.X}px";
                Top = $"{Mouse.GetPosition(App.Window).Y + Offset.Y}px";
            }
        }

        public override void OnMouseMovedAnywhere(Vector2i location)
        {
            base.OnMouseMovedAnywhere(location);

            _clock.Restart();
            Visible = false;
        }
    }
}
