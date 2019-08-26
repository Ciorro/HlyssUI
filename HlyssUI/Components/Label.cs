using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

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

        protected Text _text = new Text();

        public Label()
        {
            Text = string.Empty;
        }

        public Label(string text)
        {
            Text = text;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);
            
            _text.Font = Gui.DefaultFont;
            _text.CharacterSize = Style.CharacterSize;
            updateSize();
        }

        public override void Update()
        {
            base.Update();

            if(TransformChanged)
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
            _text.FillColor = Style["Text"];
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
