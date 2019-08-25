using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI.Components
{
    public class TextArea : Component
    {
        public enum TextAlign
        {
            Left, Right, Center, Justify
        }

        public string Text
        {
            get { return _text; }
            set
            {
                SetText(value);
            }
        }

        public uint CharacterSize
        {
            get { return _characterSize; }
            set
            {
                foreach (var letter in _letters)
                {
                    letter.CharacterSize = value;
                }

                _characterSize = value;
                TransformChanged = true;
            }
        }

        public TextAlign Align = TextAlign.Left;

        private uint _characterSize;
        private string _text;
        private List<Letter> _letters = new List<Letter>();
        private List<TextLine> _lines = new List<TextLine>();

        private int _selectionStart = -1;
        private int _selectionEnd = -1;
        private bool _isSeleting = false;

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);
            CharacterSize = Style.CharacterSize;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            CreateLines();
            PlaceLines();

            foreach (var line in _lines)
            {
                switch (Align)
                {
                    case TextAlign.Left:
                        line.AlignToLeft();
                        break;
                    case TextAlign.Right:
                        line.AlignToRight();
                        break;
                    case TextAlign.Center:
                        line.AlignToCenter();
                        break;
                    case TextAlign.Justify:
                        if (line != _lines.Last())
                            line.Justify();
                        else
                            line.AlignToLeft();
                        break;
                }
            }
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            foreach (var letter in _letters)
            {
                letter.Color = Style["text"];
            }
        }

        public override void Draw(RenderTarget target)
        {
            foreach (var letter in _letters)
            {
                letter.Draw(target);
            }
        }

        public override void OnMousePressedAnywhere(Vector2i location, Mouse.Button button)
        {
            base.OnMousePressedAnywhere(location, button);

            _selectionStart = GetLetterByPosition(Mouse.GetPosition(Gui.Window));
            _isSeleting = true;
            _selectionEnd = -1;
        }

        public override void OnMouseReleasedAnywhere(Vector2i location, Mouse.Button button)
        {
            base.OnMouseReleasedAnywhere(location, button);
            _isSeleting = false;
            _selectionEnd = GetLetterByPosition(location);
        }

        public override void Update()
        {
            base.Update();

            if(_isSeleting)
                _selectionEnd = GetLetterByPosition(Mouse.GetPosition(Gui.Window));

            for (int i = 0; i < _letters.Count; i++)
            {
                if (i > 0 && i < _letters.Count && i >= Math.Min(_selectionEnd, _selectionStart) && i <= Math.Max(_selectionEnd, _selectionStart))
                    _letters[i].Selected = true;
                else
                    _letters[i].Selected = false;
            }
        }

        private void PlaceLines()
        {
            int currentHeight = 0;

            foreach (var line in _lines)
            {
                line.Position = new Vector2i(GlobalPosition.X, GlobalPosition.Y + currentHeight);
                currentHeight += line.Height;
            }
        }

        private void CreateLines()
        {
            _lines.Clear();
            _lines.Add(CreateLine());

            for (int i = 0; i < _letters.Count; i++)
            {
                List<Letter> word = GetWord(i);

                if (!_lines.Last().TryAddWord(word))
                {
                    _lines.Add(CreateLine());
                    _lines.Last().TryAddWord(word);
                }

                i += word.Count;

                if (i < _letters.Count && _letters[i].Character != "\n")
                    _lines.Last().TryAddLetter(_letters[i]);
                else if (i < _letters.Count && _letters[i].Character == "\n")
                    _lines.Add(CreateLine());
            }
        }

        private TextLine CreateLine()
        {
            return new TextLine(TargetSize.X);
        }

        private List<Letter> GetWord(int index)
        {
            List<Letter> word = new List<Letter>();

            while (index < _letters.Count && _letters[index].Character != " " && _letters[index].Character != "\n")
            {
                word.Add(_letters[index]);
                index++;
            }

            return word;
        }

        private void SetText(string text)
        {
            _text = text;
            _letters.Clear();

            foreach (var letter in text)
            {
                _letters.Add(new Letter(letter));
            }

            CreateLines();
        }

        private int GetLetterByPosition(Vector2i position)
        {
            for (int i = 0; i < _letters.Count; i++)
            {
                if (_letters[i].Bounds.Contains(position.X, position.Y))
                    return i;
            }

            return -1;
        }
    }

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
            _letter = new Text(character.ToString(), Fonts.MontserratRegular, Theme.CharacterSize);
            _selectionRect = new RectangleShape();
            _selectionRect.FillColor = new Color(163, 199, 216);
        }

        public void Draw(RenderTarget target)
        {
            if(Selected)
            {
                _selectionRect.Size = Size;
                _selectionRect.Position = (Vector2f)Position;
                target.Draw(_selectionRect);
            }

            target.Draw(_letter);
        }
    }

    internal class TextLine
    {
        public float Width
        {
            get
            {
                float w = 0;

                foreach (var letter in _letters)
                {
                    w += letter.Advance;
                }

                return (float)w;
            }
        }

        public float FastWidth
        {
            get
            {
                if (_letters.Count > 0)
                    return _letters.Last().Position.X + _letters.Last().Size.X;
                else return 0;
            }
        }

        public int Height { get; private set; }

        public int MaxWidth = 0;
        public Vector2i Position = new Vector2i();

        private List<Letter> _letters = new List<Letter>();

        public TextLine(int maxWidth)
        {
            MaxWidth = maxWidth;
        }

        public bool CanAddWord(List<Letter> letters)
        {
            float lettersWidth = 0;

            foreach (var letter in letters)
            {
                lettersWidth += letter.Size.X;
            }

            return Width + lettersWidth <= MaxWidth;
        }

        public bool TryAddWord(List<Letter> letters)
        {
            if (CanAddWord(letters))
            {
                _letters.AddRange(letters);

                foreach (var letter in letters)
                {
                    int letterHeight = (int)letter.Font.GetLineSpacing(letter.CharacterSize);
                    if (letterHeight > Height)
                    {
                        Height = letterHeight;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanAddLetter(Letter letter)
        {
            return Width + letter.Size.X <= MaxWidth;
        }

        public bool TryAddLetter(Letter letter)
        {
            if (CanAddLetter(letter))
            {
                _letters.Add(letter);

                int letterHeight = (int)letter.Font.GetLineSpacing(letter.CharacterSize);
                if (letterHeight > Height)
                {
                    Height = letterHeight;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void AlignToLeft()
        {
            int currX = 0;

            foreach (var letter in _letters)
            {
                letter.Position = new Vector2i(Position.X + currX, Position.Y);
                currX += (int)letter.Advance;
            }
        }

        public void AlignToRight()
        {
            int currX = MaxWidth - (int)Width;

            foreach (var letter in _letters)
            {
                letter.Position = new Vector2i(Position.X + currX, Position.Y);
                currX += (int)letter.Advance;
            }
        }

        public void AlignToCenter()
        {
            int currX = (MaxWidth - (int)Width) / 2;

            foreach (var letter in _letters)
            {
                letter.Position = new Vector2i(Position.X + currX, Position.Y);
                currX += (int)letter.Advance;
            }
        }

        public void Justify()
        {
            float currX = 0;
            float spaceWidth = calcSpaceWidth();

            foreach (var letter in _letters)
            {
                letter.Position = new Vector2i((int)(Position.X + currX), Position.Y);

                if (letter.Character == " ")
                    currX += spaceWidth + letter.Advance;
                else
                    currX += letter.Advance;
            }
        }

        public float calcSpaceWidth()
        {
            int spaceCount = getSpaceCount();
            return (MaxWidth - Width) / ((spaceCount != 0) ? (float)spaceCount : 1f);
        }

        public int getSpaceCount()
        {
            int count = 0;

            for (int i = 0; i < _letters.Count; i++)
            {
                if (_letters[i].Character == " " || _letters[i].Character == "\n" || i == _letters.Count - 1)
                    count++;
            }

            return count - 1;
        }
    }
}
