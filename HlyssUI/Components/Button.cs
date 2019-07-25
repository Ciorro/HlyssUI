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

                if (value != ButtonStyle.Outline)
                {
                    Style["Accent"] = Color.Transparent;
                }
                if (value == ButtonStyle.Filled)
                {
                    Style["Primary"] = Theme.GetColor("Accent");
                    Style["Text"] = Themes.Style.GetLegibleColor(Theme.GetColor("Accent"));
                }
            }
        }

        public Button(GuiScene scene) : base(scene)
        {
            Layout = HlyssUI.Layout.LayoutType.Row;

            PaddingLeft = "20px";
            PaddingRight = "20px";
            PaddingTop = "10px";
            PaddingBottom = "10px";

            _label = new Label(scene, "Transition");
            _label.CharacterSize = scene.Gui.DefaultCharacterSize;
            _label.Font = Fonts.MontserratSemiBold;
            AddChild(_label);

            CascadeColor = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Transition($"color: primary to {getStyleColor()} darken 20");
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            Transition($"color: primary to {getStyleColor()}");
        }

        public override void OnPressed()
        {
            base.OnPressed();
            Transition($"color: primary to {getStyleColor()} darken 40");
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Transition($"color: primary to {getStyleColor()} darken 20");
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
            child.CoverParent = false;
        }

        private string getStyleColor()
        {
            switch (ButtonAppearance)
            {
                case ButtonStyle.Filled:
                    return "accent";
                case ButtonStyle.Flat:
                case ButtonStyle.Outline:
                    return "primary";
                default:
                    return "primary";
            }
        }
    }
}
