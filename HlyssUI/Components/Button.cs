using HlyssUI.Graphics;
using HlyssUI.Themes;
using HlyssUI.Extensions;
using System.Collections.Generic;
using System;

namespace HlyssUI.Components
{
    public class Button : Panel
    {
        public enum ButtonStyle
        {
            Outline, Filled, Flat
        }

        public Action Action { get; set; }

        private Dictionary<ButtonStyle, string> _styles = new Dictionary<ButtonStyle, string>()
        {
            {ButtonStyle.Outline, "outline_button_default" },
            {ButtonStyle.Filled, "filled_button_default" },
            {ButtonStyle.Flat, "flat_button_default" }
        };

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
                Style = _styles[value];
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

        public override void OnClicked()
        {
            base.OnClicked();
            Action?.Invoke();
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            //Console.WriteLine($"{Name}: [Pad] {Paddings}, [Mar] {Margins}, [Size] {Size}, [Pos] {Position}");
        }
    }
}
