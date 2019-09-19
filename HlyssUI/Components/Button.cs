using HlyssUI.Graphics;
using HlyssUI.Themes;
using HlyssUI.Extensions;

namespace HlyssUI.Components
{
    public class Button : Panel
    {
        public enum ButtonStyle
        {
            Outline, Filled, Flat
        }

        #region Styles

        protected readonly Style OutlineDefault = Style.DefaultStyle;

        protected readonly Style OutlineHover = new Style()
        {
            {"primary-color", "primary -20" }
        };

        protected readonly Style OutlinePressed = new Style()
        {
            {"primary-color", "primary -40" }
        };

        protected readonly Style FillDefault = new Style()
        {
            {"primary-color", "accent" },
            {"border-thickness", "0" },
            {"text-color", Theme.GetColor("accent").GetLegibleColor().ToHex() }
        };

        protected readonly Style FillHover = new Style()
        {
            {"primary-color", "accent -20" }
        };

        protected readonly Style FillPressed = new Style()
        {
            {"primary-color", "accent -40" }
        };

        protected readonly Style FlatDefault = new Style()
        {
            {"border-thickness", "0" }
        };

        protected readonly Style FlatHover = new Style()
        {
            {"primary-color", "secondary +20" }
        };

        protected readonly Style FlatPressed = new Style()
        {
            {"primary-color", "secondary +40" }
        };
        #endregion

        private Label _label;
        private ButtonStyle _style;

        public string Label
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;
            }
        }

        public ButtonStyle Appearance
        {
            get { return _style; }
            set
            {
                _style = value;

                switch (value)
                {
                    case ButtonStyle.Outline:
                        DefaultStyle = OutlineDefault;
                        HoverStyle = OutlineHover;
                        PressedStyle = OutlinePressed;
                        break;
                    case ButtonStyle.Filled:
                        DefaultStyle = FillDefault;
                        HoverStyle = FillHover;
                        PressedStyle = FillPressed;
                        break;
                    case ButtonStyle.Flat:
                        DefaultStyle = FlatDefault;
                        HoverStyle = FlatHover;
                        PressedStyle = FlatPressed;
                        break;
                }
            }
        }

        public Button(string label = "")
        {
            Appearance = ButtonStyle.Outline;

            _label = new Label(label)
            {
                Font = Fonts.MontserratMedium
            };
            Layout = HlyssUI.Layout.LayoutType.Row;

            PaddingLeft = "20px";
            PaddingRight = "20px";
            PaddingTop = "8px";
            PaddingBottom = "8px";

            Autosize = true;

            Children.Add(_label);
        }

        public override void OnChildAdded(Component child)
        {
            base.OnChildAdded(child);
        }
    }
}
