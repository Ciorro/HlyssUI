using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Builders
{
    interface IOverlayBuilder
    {
        GuiScene Build(Gui gui);
    }
}
