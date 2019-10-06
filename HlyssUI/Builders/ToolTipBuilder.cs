using HlyssUI.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Builders
{
    public class ToolTipBuilder : IOverlayBuilder
    {
        public string Text { get; set; } = string.Empty;

        public GuiScene Build(Gui gui)
        {
            GuiScene scene = new GuiScene(gui);

            scene.Root.Children = new List<Components.Component>()
            {
                new Panel()
                {
                    Padding = "5px",
                    Name = "tooltip_panel",
                    Children = new List<Component>()
                    {
                        new Label()
                        {
                            Text = Text,
                            Name = "tooltip_text"
                        }
                    }
                }
            };

            return scene;
        }
    }
}
