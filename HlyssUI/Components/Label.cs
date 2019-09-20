using HlyssUI.Graphics;
using SFML.Graphics;
using SFML.System;
using System;

namespace HlyssUI.Components
{
    public class Label : Component
    {
        public string Text
        {
            get { return _text.DisplayedString; }
            set
            {
                if (_text.DisplayedString != value)
                {
                    _text.DisplayedString = value;
                    UpdateSize();
                }
            }
        }

        public Font Font
        {
            get { return _text.Font; }
            set
            {
                if (_text.Font != value)
                {
                    _text.Font = value;
                    UpdateSize();
                }
            }
        }

        public uint CharacterSize
        {
            get { return _text.CharacterSize; }
            set
            {
                if (_text.CharacterSize != value)
                {
                    _text.CharacterSize = value;
                    UpdateSize();
                }
            }
        }

        public Text.Styles TextStyle
        {
            get { return _text.Style; }
            set
            {
                if (_text.Style != value)
                {
                    _text.Style = value;
                    UpdateSize();
                }
            }
        }

        public int Lines
        {
            get
            {
                int newLineCount = 0;

                foreach (var letter in _text.DisplayedString)
                {
                    if (letter == '\n' || letter.ToString() == Environment.NewLine)
                        newLineCount++;
                }

                return ++newLineCount;
            }
        }

        private Text _text;

        public Label(string text = "")
        {
            _text = new Text(text, Fonts.MontserratRegular);
            Autosize = true;

            Component stretcher = new Component()
            {
                Name = "stretcher"
            };
            Children.Add(stretcher);

            UpdateSize();
        }

        public override void Update()
        {
            base.Update();

            if (TransformChanged)
            {
                UpdateSize();
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            Vector2f position = (Vector2f)GlobalPosition;

            if (!AutosizeY)
                position.Y += (Size.Y - GetHeight()) / 2;

            _text.Position = position;
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            if (_text.FillColor != Style.GetColor("text-color") || _text.CharacterSize != Style.GetUint("character-size"))
            {
                _text.FillColor = Style.GetColor("text-color");
                _text.CharacterSize = Style.GetUint("character-size");

                UpdateSize();
            }
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_text);
        }

        private int GetHeight()
        {
            return (int)(_text.Font.GetLineSpacing(_text.CharacterSize) * Lines);
        }

        private void UpdateSize()
        {

            if (AutosizeX)
            {
                GetChild("stretcher").Width = $"{_text.GetGlobalBounds().Width}px";
            }

            if (AutosizeY)
            {
            Console.WriteLine(GetHeight() + this.ToString());
                GetChild("stretcher").Height = $"{GetHeight()}px";
            }
        }
    }
}
