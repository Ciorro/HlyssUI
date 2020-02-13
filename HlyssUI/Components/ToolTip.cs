using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class ToolTip : Flyout
    {
        public Component Target { get; set; }
        public int Delay { get; set; } = 1000;

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

            Style = "tooltip_default";
        }

        public override void Update()
        {
            base.Update();

            if(Target != null && !Target.Hovered)
            {
                _clock.Restart();
            }

            if(Target != null && Target.Hovered && _clock.ElapsedTime.AsMilliseconds() > Delay && !Visible)
            {
                _clock.Restart();
                Show(Mouse.GetPosition(Form.Window) + Offset);
            }

            if(Target != null && !Target.Hovered && Visible)
            {
                Hide();
            }
        }
    }
}
