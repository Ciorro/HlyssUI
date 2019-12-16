using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Components
{
    public class Dropdown : Component
    {
        public delegate void SelectedHandler(object sender, string text, int id);
        public event SelectedHandler OnSelected;

        private List<string> _items = new List<string>();
        private int _currentId = -1;
        private string _currentText = string.Empty;

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
                    _menuItems.Last().Clicked += ItemClicked;
                }

                (GetChild("dropdown_menu") as Menu).Items = _menuItems;
            }
        }

        public uint ItemId
        {
            get { return (uint)_currentId; }
            set
            {
                SetItem((int)value);
            }
        }

        public string ItemString
        {
            get { return _currentText; }
            set
            {
                SetItem(_items.IndexOf(value));
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
                        new Label("")
                        {
                            Name = "dropdown_label"
                        },
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

        private void ItemClicked(object sender)
        {
            SetItem(_items.IndexOf((sender as MenuItem).Label));
        }

        private void SetItem(int id)
        {
            if (_items.Count >= id && id >= 0)
            {
                _currentId = id;
                _currentText = _items[id];

                (FindChild("dropdown_label") as Label).Text = _currentText;
                OnSelected?.Invoke(this, _currentText, id);
            }
        }
    }
}
