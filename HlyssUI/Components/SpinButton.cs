using System;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class SpinButton : Component
    {
        private int _value = 0;

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;

                if (_value > MaxValue)
                    _value = MaxValue;
                if (_value < MinValue)
                    _value = MinValue;

                (GetChild("spinbutton_textbox") as TextBox).Text = _value.ToString();
            }
        }

        public int MaxValue { get; set; } = 100;
        public int MinValue { get; set; } = 0;
        public int Step { get; set; } = 1;

        public SpinButton()
        {
            Children = new List<Component>()
            {
                new TextBox()
                {
                    AllowLetters = false,
                    AllowSpecialCharacters = false,
                    Expand = true,
                    //Margin = "0px 2px",
                    Padding = "2px 2px 2px 10px",
                    CenterContent = true,
                    Name = "spinbutton_textbox",
                },
               
            };

            GetChild("spinbutton_textbox").Children.AddRange(new List<Component>()
            {
                new Button()
                {
                    Children = new List<Component>(){new Icon(Graphics.Icons.AngleUp) },
                    Padding = "10px",
                    Appearance = Button.ButtonStyle.Flat,
                    Name = "spinbutton_plus"
                },
                new Button()
                {
                    Children = new List<Component>(){new Icon(Graphics.Icons.AngleDown) },
                    Padding = "10px",
                    Appearance = Button.ButtonStyle.Flat,
                    Name = "spinbutton_minus"
                },
            });

            AutosizeY = true;
            CenterContent = true;

            Value = 0;

            GetChild("spinbutton_textbox").FocusLost += SpinButton_FocusLost;
            FindChild("spinbutton_plus").Clicked += (object sender) => Value += Step;
            FindChild("spinbutton_minus").Clicked += (object sender) => Value -= Step;
        }

        public override void OnScrolledAnywhere(float scroll)
        {
            base.OnScrolledAnywhere(scroll);

            if ((GetChild("spinbutton_textbox") as Component).Focused)
                Value += Step * Math.Sign(scroll);
        }

        private void SpinButton_FocusLost(object sender)
        {
            int val = 0;
            int.TryParse((GetChild("spinbutton_textbox") as TextBox).Text, out val);

            Value = val;
        }
    }
}
