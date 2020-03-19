using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class MenuDivider : MenuItem
    {
        public MenuDivider() : base(string.Empty)
        {
            Padding = "0px";
            Hoverable = false;
            Clickable = false;
            Children = new List<Component>()
            {
                new Divider()
            };
        }
    }
}
