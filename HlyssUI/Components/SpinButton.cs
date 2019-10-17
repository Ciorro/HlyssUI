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
        public int MinValue { get; set; } = -100;

        public SpinButton()
        {
            Children = new List<Component>()
            {
                new Button()
                {
                    Children = new List<Component>(){new Icon(Graphics.Icons.Minus) },
                    Padding = "5px",
                    Appearance = Button.ButtonStyle.Flat,
                    Name = "spinbutton_minus"
                },
                new TextBox()
                {
                    AllowLetters = false,
                    AllowSpecialCharacters = false,
                    Expand = true,
                    Margin = "0px 2px",
                    Name = "spinbutton_textbox"
                },
                new Button()
                {
                    Children = new List<Component>(){new Icon(Graphics.Icons.Plus) },
                    Padding = "5px",
                    Appearance = Button.ButtonStyle.Flat,
                    Name = "spinbutton_plus"
                }
            };

            AutosizeY = true;
            CenterContent = true;

            Value = 0;

            GetChild("spinbutton_textbox").FocusLost += SpinButton_FocusLost;
            GetChild("spinbutton_plus").Clicked += (object sender) => Value++;
            GetChild("spinbutton_minus").Clicked += (object sender) => Value--;
        }

        public override void OnScrolledAnywhere(float scroll)
        {
            base.OnScrolledAnywhere(scroll);

            if ((GetChild("spinbutton_textbox") as Component).Focused)
                Value += (int)scroll;
        }

        private void SpinButton_FocusLost(object sender)
        {
            System.Console.WriteLine("lost zagubieni");

            int val = 0;
            int.TryParse((GetChild("spinbutton_textbox") as TextBox).Text, out val);

            Value = val;
        }
    }
}
