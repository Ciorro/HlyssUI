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
            Padding = "5px 10px";

            GetChild("listitem_text").Expand = true;
            Children.Add(new Icon(Graphics.Icons.AngleRight)
            {
                Name = "menuitem_submenuicon",
                Visible = false
            });
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();

            if (Menu != null)
            {
                Menu.Show(GlobalPosition + new Vector2i(TargetSize.X, -1));
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
