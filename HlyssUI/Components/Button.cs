using HlyssUI.Graphics;
using HlyssUI.Themes;
using SFML.Graphics;

namespace HlyssUI.Components
{
    public class Button : Panel
    {
        public enum ButtonStyle
        {
            Outline, Filled, Flat
        }

        public string Label
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;
            }
        }

        private Label _label;
        private ButtonStyle _style;

        public ButtonStyle Appearance
        {
            get { return _style; }
            set
            {
                _style = value;

                if (value != ButtonStyle.Outline)
                {
                    Style["Secondary"] = Color.Transparent;
                }
                if (value == ButtonStyle.Filled)
                {
                    _label.Font = Fonts.MontserratSemiBold;
                    Style["Primary"] = Theme.GetColor("Accent");
                    Style["Text"] = Themes.Style.GetLegibleColor(Theme.GetColor("Accent"));
                }
                else
                {
                    _label.Font = Fonts.MontserratMedium;
                }
            }
        }

        public Button(string label = "")
        {
            _label = new Label(label);
            Layout = HlyssUI.Layout.LayoutType.Row;

            PaddingLeft = "20px";
            PaddingRight = "20px";
            PaddingTop = "8px";
            PaddingBottom = "8px";

            Autosize = true;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            AddChild(_label);
            _label.Font = Fonts.MontserratMedium;

            CascadeStyle = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Style["primary"] = Style.GetDarker(getStyleColor(), 20);
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            Style["primary"] = getStyleColor();
        }

        public override void OnPressed()
        {
            base.OnPressed();
            Style["primary"] = Style.GetDarker(getStyleColor(), 40);
            
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Style["primary"] = Style.GetDarker(getStyleColor(), 20);
            
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
            child.CoverParent = false;
        }

        private Color getStyleColor()
        {
            switch (Appearance)
            {
                case ButtonStyle.Filled:
                    return Theme.GetColor("accent");
                case ButtonStyle.Flat:
                case ButtonStyle.Outline:
                    return Theme.GetColor("primary");
                default:
                    return Theme.GetColor("primary");
            }
        }
    }
}
