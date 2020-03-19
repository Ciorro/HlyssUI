using HlyssUI.Components.Internals;
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
                ScheduleRefresh();
                SetText(value);
            }
        }

        public uint CharacterSize
        {
            get { return _characterSize; }
            set
            {
                if (value != _characterSize)
                {
                    foreach (var letter in _letters)
                    {
                        letter.CharacterSize = value;
                    }

                    _characterSize = value;
                    ScheduleRefresh();
                }
            }
        }

        public string Selected
        {
            get
            {
                string selected = string.Empty;

                for (int i = 0; i < _letters.Count; i++)
                {
                    if (_letters[i].Selected)
                        selected += _letters[i].Character;
                }

                return selected;
            }
        }

        public bool Selectable { get; set; }
        public bool TextAutosize { get; set; }
        public bool WordWrap { get; set; } = true;
        public TextAlign Align = TextAlign.Left;

        private uint _characterSize;
        private string _text;
        private List<Letter> _letters = new List<Letter>();
        private List<TextLine> _lines = new List<TextLine>();

        private int _selectionStart = -1;
        private int _selectionEnd = -1;
        private bool _isSeleting = false;

        private Vector2i _prevSize = new Vector2i();

        public override void OnRefresh()
        {
            base.OnRefresh();

            if (_prevSize != TargetSize)
            {
                CreateLines();
                UpdateTextSize();
            }
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

            PlaceLines();
            _prevSize = TargetSize;
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            foreach (var letter in _letters)
            {
                letter.Color = StyleManager.GetColor("text-color");
            }

            CharacterSize = StyleManager.GetUint("font-size");
        }

        public override void Draw(RenderTarget target)
        {
            foreach (var letter in _letters)
            {
                if (letter.Bounds.Intersects((Parent != null) ? Parent.ClipArea.Bounds : Form.Root.Bounds))
                {
                    letter.Draw(target);
                }
            }
        }

        public override void OnMousePressedAnywhere(Vector2i location, Mouse.Button button)
        {
            base.OnMousePressedAnywhere(location, button);

            _selectionStart = GetLetterByPosition(Mouse.GetPosition(Form.Window));
            _isSeleting = true;
            _selectionEnd = -1;
        }

        public override void OnMouseReleasedAnywhere(Vector2i location, Mouse.Button button)
        {
            base.OnMouseReleasedAnywhere(location, button);
            _isSeleting = false;
            _selectionEnd = GetLetterByPosition(location);
        }

        public override void OnKeyPressed(Keyboard.Key key)
        {
            base.OnKeyPressed(key);

            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) && key == Keyboard.Key.C)
                Clipboard.Contents = Selected;
        }

        public override void Update()
        {
            base.Update();

            if (!Selectable)
                return;

            if (_isSeleting)
                _selectionEnd = GetLetterByPosition(Mouse.GetPosition(Form.Window));

            for (int i = 0; i < _letters.Count; i++)
            {
                if (i >= 0 && i < _letters.Count && i >= Math.Min(_selectionEnd, _selectionStart) && i <= Math.Max(_selectionEnd, _selectionStart))
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
            _lines.Add(new TextLine(TargetSize.X));

            for (int i = 0; i < _letters.Count; i++)
            {
                List<Letter> word = GetWord(i);

                if (!WordWrap)
                {
                    _lines.Last().TryAddWord(word, true);
                }
                else if (!_lines.Last().TryAddWord(word))
                {
                    _lines.Add(new TextLine(TargetSize.X));
                    _lines.Last().TryAddWord(word);
                }

                i += word.Count;

                if (i < _letters.Count && _letters[i].Character != "\n")
                    _lines.Last().TryAddLetter(_letters[i]);
                else if (i < _letters.Count && _letters[i].Character == "\n")
                    _lines.Add(new TextLine(TargetSize.X));
            }
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
            OnStyleChanged();

            if (TextAutosize)
                UpdateTextSize();
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

        private void UpdateTextSize()
        {
            //int height = getHeight();

            ////TODO: ...
            //if (height == 0)
            //    Height = "1px";
            //else
            //    Height = $"{height}px";

            Height = $"{getHeight()}px";

            if (!WordWrap)
                Width = $"{(int)_lines[0].FastWidth}px";
        }

        private int getHeight()
        {
            int height = 0;

            foreach (var line in _lines)
            {
                height += line.Height;
            }

            return height;
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

                return w;
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

        public bool CanAddLetter(Letter letter)
        {
            return Width + letter.Size.X <= MaxWidth;
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

        public bool TryAddWord(List<Letter> letters, bool force = false)
        {
            if (CanAddWord(letters) || force)
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

        public bool TryAddLetter(Letter letter, bool force = false)
        {
            if (CanAddLetter(letter) || force)
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
