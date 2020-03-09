using HlyssUI.Graphics;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using System;

namespace HlyssUI.Components.Internals
{
    internal class Letter
    {
        private Text _letter;

        public Vector2i Position
        {
            get { return (Vector2i)_letter.Position; }
            set { _letter.Position = (Vector2f)value; }
        }

        public Vector2f Size
        {
            get
            {
                return new Vector2f(Advance, Font.GetLineSpacing(CharacterSize));
            }
        }

        public Color Color
        {
            get { return _letter.FillColor; }
            set { _letter.FillColor = value; }
        }

        public Font Font
        {
            get { return _letter.Font; }
        }

        public uint CharacterSize
        {
            get { return _letter.CharacterSize; }
            set { _letter.CharacterSize = value; }
        }

        public bool Selected;

        private RectangleShape _selectionRect;
        private bool _isCustomAdvance = false;
        private float _customAdvance = 0;

        public float Advance
        {
            set
            {
                if (value == 0)
                    _isCustomAdvance = false;
                else
                {
                    _customAdvance = value;
                    _isCustomAdvance = true;
                }
            }
            get
            {
                if (_isCustomAdvance)
                    return _customAdvance;
                else
                    return _letter.Font.GetGlyph(_letter.DisplayedString[0], _letter.CharacterSize, _letter.Style == Text.Styles.Bold, _letter.OutlineThickness).Advance;
            }
        }

        public string Character
        {
            get { return _letter.DisplayedString; }
            set { _letter.DisplayedString = value; }
        }

        public bool IsNewLine
        {
            get { return Character == "\n" || Character == Environment.NewLine; }
        }

        public IntRect Bounds
        {
            get
            {
                return new IntRect(Position, (Vector2i)Size);
            }
        }

        public Letter(char character)
        {
            _letter = new Text(character.ToString(), Fonts.MontserratRegular, ThemeManager.CharacterSize);
            _selectionRect = new RectangleShape();
            _selectionRect.FillColor = new Color(153, 201, 239);
        }

        public void Draw(RenderTarget target)
        {
            if (Selected)
            {
                _selectionRect.Size = Size;
                _selectionRect.Position = (Vector2f)Position;
                target.Draw(_selectionRect);
            }

            target.Draw(_letter);
        }
    }
}
