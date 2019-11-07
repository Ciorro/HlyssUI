using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class Dropdown : Component
    {
        private List<string> _items = new List<string>();

        public List<string> Items
        {
            get { return _items; }
            set 
            { 
                _items = value;

                List<MenuItem> _menuItems = new List<MenuItem>();
                foreach (var item in _items)
                {
                    _menuItems.Add(new MenuItem(item));
                }

                (GetChild("dropdown_menu") as Menu).Items = _menuItems;
            }
        }

        public Dropdown()
        {
            Children = new List<Component>()
            {
                new Button()
                {
                    AutosizeX = false,
                    Width = "100%",
                    Name = "dropdown_button",
                    Children = new List<Component>()
                    {
                        new Label("Item1"),
                        new Spacer(),
                        new Icon(Graphics.Icons.AngleDown)
                    }
                },
                new Menu()
                {
                    Name = "dropdown_menu",
                    Width = "100%"
                }
            };

            AutosizeY = true;
            Width = "250px";

            GetChild("dropdown_button").Clicked += (object sender) =>
            {
                (GetChild("dropdown_menu") as Menu).Show(GlobalPosition + new Vector2i(0, TargetSize.Y + 2));
            };
        }
    }
}
