using SFML.System;

namespace HlyssUI.Components
{
    public class MenuItem : ListItem
    {
        private Menu _menu;

        public Menu Menu
        {
            get { return _menu; }
            set
            {
                if (_menu != null && Children.Contains(_menu))
                    Children.Remove(_menu);

                _menu = value;

                if (value != null)
                    Children.Add(_menu);

                FindChild("menuitem_submenuicon").Visible = value != null;
            }
        }

        public MenuItem(string label = "") : base(label)
        {
            Padding = "4px 10px";

            Children.Add(new Spacer());
            Children.Add(new Icon(Graphics.Icons.AngleRight)
            {
                Name = "menuitem_submenuicon",
                Style = "menu_item_icon_default",
                Visible = false
            });

            Style = "list_item_default menu_item_default";
            IconWidth = "20px";
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();

            if (Menu != null)
            {
                Menu.Show(GlobalPosition + new Vector2i(TargetSize.X - 5, -2));
            }
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();

            if (Menu != null)
            {
                Menu.Hide();
            }
        }
    }
}
