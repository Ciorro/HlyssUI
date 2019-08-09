﻿using HlyssUI.Graphics;
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

        public ButtonStyle ButtonAppearance
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
                    Style["Primary"] = Theme.GetColor("Accent");
                    Style["Text"] = Themes.Style.GetLegibleColor(Theme.GetColor("Accent"));
                }
            }
        }

        public Button(string label = "")
        {
            _label = new Label(label);
            Layout = HlyssUI.Layout.LayoutType.Row;

            PaddingLeft = "20px";
            PaddingRight = "20px";
            PaddingTop = "10px";
            PaddingBottom = "10px";
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            _label.CharacterSize = Gui.DefaultCharacterSize;
            _label.Font = Fonts.MontserratSemiBold;
            AddChild(_label);

            CascadeStyle = true;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Style["primary"] = Style.GetDarker(getStyleColor(), 20);
            StyleChanged = true;
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            Style["primary"] = getStyleColor();
            StyleChanged = true;
        }

        public override void OnPressed()
        {
            base.OnPressed();
            Style["primary"] = Style.GetDarker(getStyleColor(), 40);
            StyleChanged = true;
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Style["primary"] = Style.GetDarker(getStyleColor(), 20);
            StyleChanged = true;
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
            child.CoverParent = false;
        }

        private Color getStyleColor()
        {
            switch (ButtonAppearance)
            {
                case ButtonStyle.Filled:
                    return Style["accent"];
                case ButtonStyle.Flat:
                case ButtonStyle.Outline:
                    return Style["primary"];
                default:
                    return Style["primary"];
            }
        }
    }
}
