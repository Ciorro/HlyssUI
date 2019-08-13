using HlyssUI.Layout;
using SFML.Graphics;
using SFML.System;
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
                _text = value;
                _letters.Clear();

                TransformChanged = true;

                if (Gui == null)
                    return;

                foreach (var letter in value)
                {
                    _letters.Add(new Letter(letter, Gui));
                }

                createLines();
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

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);
            CharacterSize = Gui.DefaultCharacterSize;
        }

        public override void OnRefresh()
        {
            //TODO: Improve performance
            base.OnRefresh();

            createLines();
            updateLettersStyle();
            placeLines();

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

        public override void Draw(RenderTarget target)
        {
            foreach (var letter in _letters)
            {
                letter.Draw();
            }
        }

        private void updateLettersStyle()
        {
            foreach (var letter in _letters)
            {
                letter.Color = Style["text"];
            }
        }

        private void placeLines()
        {
            int currentHeight = 0;

            foreach (var line in _lines)
            {
                line.Position = new Vector2i(GlobalPosition.X, GlobalPosition.Y + currentHeight);
                currentHeight += line.Height;
            }
        }

        private void createLines()
        {
            _lines.Clear();
            _lines.Add(createLine());

            for (int i = 0; i < _letters.Count; i++)
            {
                List<Letter> word = getWord(i);

                if (!_lines.Last().TryAddWord(word))
                {
                    _lines.Add(createLine());
                    _lines.Last().TryAddWord(word);
                }

                i += word.Count;

                if (i < _letters.Count && _letters[i].Character != "\n")
                    _lines.Last().TryAddLetter(_letters[i]);
                else if (i < _letters.Count && _letters[i].Character == "\n")
                    _lines.Add(createLine());
            }
        }

        private TextLine createLine()
        {
            return new TextLine(TargetSize.X);
        }

        private List<Letter> getWord(int index)
        {
            List<Letter> word = new List<Letter>();

            while (index < _letters.Count && _letters[index].Character != " " && _letters[index].Character != "\n")
            {
                word.Add(_letters[index]);
                index++;
            }

            return word;
        }
    }

    internal class Letter
    {
        private Text _letter;
        private Gui _gui;

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

        public Letter(char character, Gui gui)
        {
            _gui = gui;
            _letter = new Text(character.ToString(), gui.DefaultFont, gui.DefaultCharacterSize);
            //Color = Style["text"];
        }

        public void Draw()
        {
            _gui.Window.Draw(_letter);
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
