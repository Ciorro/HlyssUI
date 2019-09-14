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
                _text.DisplayedString = value;
                TransformChanged = true;
            }
        }

        public Font Font
        {
            get { return _text.Font; }
            set
            {
                _text.Font = value;
                TransformChanged = true;
            }
        }

        public uint CharacterSize
        {
            get { return _text.CharacterSize; }
            set
            {
                _text.CharacterSize = value;
                TransformChanged = true;
            }
        }

        public Text.Styles TextStyle
        {
            get { return _text.Style; }
            set
            {
                _text.Style = value;
                TransformChanged = true;
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

        private Text _text = new Text();

        public Label()
        {
            Text = string.Empty;

            _text.Font = Fonts.MontserratRegular;
        }

        public Label(string text)
        {
            Text = text;

            _text.Font = Fonts.MontserratRegular;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);
            updateSize();
        }

        public override void Update()
        {
            base.Update();

            if (TransformChanged)
            {
                updateSize();
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _text.Position = (Vector2f)GlobalPosition;
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            
            _text.FillColor = Style.GetColor("text-color");
            _text.CharacterSize = Style.GetUint("character-size");

            updateSize();
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_text);
        }

        private int getHeight()
        {
            return (int)(_text.Font.GetLineSpacing(_text.CharacterSize) * Lines);
        }

        private void updateSize()
        {
            Width = $"{_text.GetGlobalBounds().Width}px";
            Height = $"{getHeight()}px";
        }
    }
}
