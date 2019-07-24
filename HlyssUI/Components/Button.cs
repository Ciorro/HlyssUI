using HlyssUI.Graphics;
using HlyssUI.Themes;
using SFML.Graphics;

namespace HlyssUI.Components
{
    public class Button : Card
    {
        public enum ButtonStyle
        {
            Outline, Filled, Flat
        }

        private Label _label;
        private ButtonStyle _style;

        public ButtonStyle ButtonAppearance
        {
            get { return _style; }
            set
            {
                _style = value;

                base.Style["Accent"] = Color.Transparent;
                
                 if (value == ButtonStyle.Filled)
                {
                    base.Style["Primary"] = Theme.GetColor("Accent");
                    base.Style["Text"] = Themes.Style.GetLegibleColor(Theme.GetColor("Accent"));
                }
            }
        }

        public Button(Gui gui) : base(gui)
        {
            PaddingLeft = "20px";
            PaddingRight = "20px";
            PaddingTop = "10px";
            PaddingBottom = "10px";

            _label = new Label(gui, "Install now");
            _label.CharacterSize = gui.DefaultCharacterSize;
            _label.CoverParent = false;
            _label.Font = Fonts.MontserratSemiBold;
            AddChild(_label);

            CascadeColor = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            ChangeColor("Primary", Themes.Style.GetDarker(getStyleColor(), 20));
            _label.ChangeColor("Text", Themes.Style.GetLegibleColor(Themes.Style.GetDarker(getStyleColor(), 20)));
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            ChangeColor("Primary", getStyleColor());
            _label.ChangeColor("Text", Themes.Style.GetLegibleColor(getStyleColor()));
        }

        public override void OnPressed()
        {
            base.OnPressed();
            ChangeColor("Primary", Themes.Style.GetDarker(getStyleColor(), 50));
            _label.ChangeColor("Text", Themes.Style.GetLegibleColor(Themes.Style.GetDarker(getStyleColor(), 50)));
        }

        public override void OnReleased()
        {
            base.OnReleased();

            ChangeColor("Primary", Themes.Style.GetDarker(getStyleColor(), 20));
            _label.ChangeColor("Text", Themes.Style.GetLegibleColor(Themes.Style.GetDarker(getStyleColor(), 20)));
        }

        private Color getStyleColor()
        {
            switch (ButtonAppearance)
            {
                case ButtonStyle.Filled:
                    return Theme.GetColor("Accent");
                case ButtonStyle.Flat:
                case ButtonStyle.Outline:
                    return Theme.GetColor("Primary");
                default:
                    return Theme.GetColor("Primary");
            }
        }
    }
}
