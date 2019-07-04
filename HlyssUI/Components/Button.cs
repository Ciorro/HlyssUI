using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class Button : Card
    {
        Label _label;

        public Button(Gui gui) : base(gui)
        {
            PaddingLeft = "20px";
            PaddingRight = "20px";
            PaddingTop = "10px";
            PaddingBottom = "10px";

            _label = new Label(gui, "Button");
            _label.CharacterSize = gui.DefaultCharacterSize + 1;
            AddChild(_label);
        }
    }
}
