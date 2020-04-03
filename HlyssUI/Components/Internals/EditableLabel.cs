using HlyssUI.Graphics;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI.Components.Internals
{
    internal class EditableLabel : Component
    {
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    UpdateText(_text, value);
                    _text = value;
                    ScheduleRefresh();
                }
            }
        }

        public uint CharacterSize
        {
            get { return _characterSize; }
            set
            {
                _characterSize = value;
                ScheduleRefresh();
            }
        }

        public Text.Styles TextStyle
        {
            get { return _textStyle; }
            set
            {
                _textStyle = value;
                ScheduleRefresh();
            }
        }

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                ScheduleRefresh();
            }
        }

        public int Lines
        {
            get
            {
                int newLineCount = 0;

                foreach (var letter in _text)
                {
                    if (letter == '\n' || letter.ToString() == System.Environment.NewLine)
                        newLineCount++;
                }

                return ++newLineCount;
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

        public Range SelectionRange
        {
            get
            {
                int start = _selectionStart;
                int end = _selectionEnd;

                if (end < start)
                {
                    int tmp = start;
                    start = end;
                    end = tmp;
                }

                if (end >= 0 && start < 0)
                    start = 0;

                return new Range(start, end);
            }
            set
            {
                _selectionStart = value.Min;
                _selectionEnd = value.Max;

                UpdateSelection();
            }
        }

        public bool IsAnyTextSelected
        {
            get { return _selectionStart >= 0 && _selectionEnd >= 0; }
        }

        private List<Letter> _letters = new List<Letter>();

        private string _text = string.Empty;
        private uint _characterSize;
        private Text.Styles _textStyle;
        private Font _font;

        private int _selectionStart = -1;
        private int _selectionEnd = -1;
        private bool _isSeleting = false;

        public EditableLabel()
        {
            Font = Fonts.MontserratRegular;
            CharacterSize = StyleManager.GetUint("font-size");
            TextStyle = SFML.Graphics.Text.Styles.Regular;
            Text = string.Empty;
        }

        public EditableLabel(string text)
        {
            Font = Fonts.MontserratRegular;
            CharacterSize = StyleManager.GetUint("font-size");
            TextStyle = SFML.Graphics.Text.Styles.Regular;
            Text = text;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            Align();
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            foreach (var letter in _letters)
            {
                letter.Color = StyleManager.GetColor("text-color");
                letter.CharacterSize = StyleManager.GetUint("font-size");
            }
        }

        public override void OnMousePressedAnywhere(Vector2i location, Mouse.Button button)
        {
            base.OnMousePressedAnywhere(location, button);

            ResetSelection();

            _selectionStart = GetLetterByPosition(Mouse.GetPosition(Form.Window));
            _isSeleting = true;
        }

        public override void OnMouseReleasedAnywhere(Vector2i location, Mouse.Button button)
        {
            base.OnMouseReleasedAnywhere(location, button);
            _isSeleting = false;
        }

        public override void OnKeyPressed(Keyboard.Key key)
        {
            base.OnKeyPressed(key);

            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) && key == Keyboard.Key.C)
                Clipboard.Contents = Selected;
        }

        public override void OnMouseMovedAnywhere(Vector2i location)
        {
            base.OnMouseMovedAnywhere(location);

            if (_isSeleting)
            {
                _selectionEnd = GetLetterByPosition(location);
            }
        }

        public override void Update()
        {
            base.Update();

            if (TransformChanged)
            {
                UpdateSize();
            }

            if (IsAnyTextSelected)
            {
                UpdateSelection();
            }
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);

            foreach (var letter in _letters)
            {
                letter.Draw(target);
            }
        }

        public Vector2f GetLetterPosition(int index)
        {
            index--;

            if (index >= 0 && index < _letters.Count)
            {
                return (Vector2f)(_letters[index].Position
                                  + (!_letters[index].IsNewLine ? new Vector2i((int)_letters[index].Advance, 0) : new Vector2i())
                                  - GlobalPosition);
            }

            return new Vector2f();
        }

        public int GetLetterByPosition(Vector2i position)
        {
            for (int i = 0; i < _letters.Count; i++)
            {
                if (_letters[i].Bounds.Contains(position.X, position.Y))
                    return i;
            }

            return -1;
        }

        public void ResetSelection()
        {
            SelectionRange = new Range(-1, -1);
        }

        public void SelectAll()
        {
            SelectionRange = new Range(0, _text.Length - 1);
        }

        private void UpdateText(string prevText, string currentText)
        {
            int diff = currentText.Length - prevText.Length;

            if (diff > 0)
            {
                for (int i = prevText.Length; i < currentText.Length; i++)
                {
                    _letters.Add(new Letter(currentText[i]));
                }
            }
            else if (diff < 0)
            {
                _letters.RemoveRange(_letters.Count + diff, System.Math.Abs(diff));
            }

            for (int i = 0; i < _letters.Count; i++)
            {
                _letters[i].Character = currentText[i].ToString();
            }

            StyleChanged = true;

            ResetSelection();
            Align();
        }

        private void Align()
        {
            float x = GlobalPosition.X;
            float y = GlobalPosition.Y;

            float lineSpacing = Font.GetLineSpacing(StyleManager.GetUint("font-size"));

            foreach (var letter in _letters)
            {
                if (letter.IsNewLine)
                {
                    y += lineSpacing;
                    x = GlobalPosition.X;
                }

                letter.Position = new Vector2i((int)x, (int)y);

                if (!letter.IsNewLine)
                    x += letter.Advance;
            }
        }

        private void UpdateSize()
        {
            int width = 0;

            foreach (var letter in _letters)
            {
                width = (int)System.Math.Max(width, letter.Position.X - GlobalPosition.X + letter.Advance);
            }

            Width = $"{(width == 0 ? 1 : width)}px";
            Height = $"{(int)(Font.GetLineSpacing(StyleManager.GetUint("font-size")) * Lines)}px";
        }

        private void UpdateSelection()
        {
            for (int i = 0; i < _letters.Count; i++)
            {
                if (i >= 0 && i < _letters.Count && i >= System.Math.Min(_selectionEnd, _selectionStart) && i <= System.Math.Max(_selectionEnd, _selectionStart))
                    _letters[i].Selected = true;
                else
                    _letters[i].Selected = false;
            }
        }
    }
}
