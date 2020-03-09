using HlyssUI.Components.Internals;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class TextBox : Panel
    {
        public delegate void TextChangedHandler(object sender, string currentText);
        public event TextChangedHandler OnTextChanged;

        private RectangleShape _cursor = new RectangleShape();
        private Clock _cursorTimer = new Clock();
        private Component _textView;
        private EditableLabel _text;
        private bool _cursorVisible;
        private string _realText = string.Empty;
        private string _placeholder = string.Empty;
        private int _currentIndex;

        public string Placeholder
        {
            get { return _placeholder; }
            set
            {
                _placeholder = value;
                UpdateValue();
            }
        }

        public string Text
        {
            get { return _realText; }
            set
            {
                _realText = value;
                UpdateValue();
            }
        }

        public bool AllowNumbers { get; set; } = true;
        public bool AllowLetters { get; set; } = true;
        public bool AllowSpecialCharacters { get; set; } = true;
        public bool Password { get; set; }
        public bool SelectOnFocus { get; set; } = true;

        public int MaxLines { get; set; } = 1;

        public TextBox()
        {
            Padding = "10px";

            _text = new EditableLabel();
            _text.ClipArea.OutlineThickness = -2;

            _textView = new Component()
            {
                AutosizeY = true,
                Expand = true,
                Overflow = OverflowType.Scroll,
                ScrollOffset = new Vector2i(20, 10),
                Children = new List<Component>() { _text }
            };

            Children = new List<Component>()
            {
                _textView
            };

            AutosizeY = true;
            Overflow = OverflowType.Hidden;

            UpdateValue();

            Style = "textbox_off_default";
            OverwriteChildren = false;
        }

        public override void OnFocusGained()
        {
            base.OnFocusGained();

            if (_realText.Length == 0)
                _text.Text = string.Empty;
            else if (SelectOnFocus)
                _text.SelectAll();

            Style = "textbox_on_default";

            _currentIndex = _realText.Length;
        }

        public override void OnFocusLost()
        {
            base.OnFocusLost();

            if (_realText.Length == 0)
                _text.Text = Placeholder;

            Style = "textbox_off_default";
        }

        public override void OnTextInput(string text)
        {
            base.OnTextInput(text);

            if (Focused)
            {
                char c = text[0];
                string tmpText = _realText;

                if (c == 8)
                {
                    if (_realText.Length > 0)
                    {
                        if (_text.IsAnyTextSelected)
                        {
                            RemoveSelectedText();
                        }
                        else if (_currentIndex > 0)
                        {
                            _realText = _realText.Remove(_currentIndex - 1, 1);
                            _currentIndex--;
                        }
                    }
                }
                else if (c == 13 && _text.Lines < MaxLines)
                {
                    _realText = _realText.Insert(_currentIndex, "\n");
                    _currentIndex++;
                }
                else if (!char.IsControl(c))
                {
                    RemoveSelectedText();

                    if ((char.IsDigit(c) && AllowNumbers == true) || (c == '-' && _currentIndex == 0 && _realText.Contains("-") == false))
                    {
                        _realText = _realText.Insert(_currentIndex, text);
                        _currentIndex++;
                    }
                    if (char.IsLetter(c) && AllowLetters == true)
                    {
                        _realText = _realText.Insert(_currentIndex, text);
                        _currentIndex++;
                    }
                    if ((char.IsSymbol(c) || char.IsPunctuation(c) || char.IsSeparator(c)) && char.IsWhiteSpace(c) == false && AllowSpecialCharacters == true)
                    {
                        _realText = _realText.Insert(_currentIndex, text);
                        _currentIndex++;
                    }
                    if (char.IsWhiteSpace(c))
                    {
                        _realText = _realText.Insert(_currentIndex, text);
                        _currentIndex++;
                    }
                }

                UpdateValue();

                if (_realText != tmpText)
                {
                    OnTextChanged?.Invoke(this, _realText);
                }
            }
        }

        public override void OnKeyPressed(Keyboard.Key key)
        {
            base.OnKeyPressed(key);

            if (Focused)
            {
                if (key == Keyboard.Key.Left && _currentIndex > 0)
                {
                    _currentIndex--;
                    _cursor.Position = _text.GetLetterPosition(_currentIndex) + (Vector2f)_text.GlobalPosition;
                }
                else if (key == Keyboard.Key.Right && _currentIndex < _realText.Length)
                {
                    _currentIndex++;
                    _cursor.Position = _text.GetLetterPosition(_currentIndex) + (Vector2f)_text.GlobalPosition;
                }
                else if (key == Keyboard.Key.Delete && _currentIndex < _realText.Length)
                {
                    _realText = _realText.Remove(_currentIndex, 1);
                    UpdateValue();
                }
                else if (key == Keyboard.Key.Home)
                {
                    _currentIndex = 0;
                }
                else if (key == Keyboard.Key.End)
                {
                    _currentIndex = _realText.Length;
                }
            }
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Form.Window.SetMouseCursor(Cursors.Text);
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            Form.Window.SetMouseCursor(Cursors.Arrow);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _cursor.FillColor = StyleManager.GetColor("text-color");
        }

        public override void Update()
        {
            base.Update();

            if (_cursorTimer.ElapsedTime.AsMilliseconds() > 500)
            {
                Vector2f letterPos = _text.GetLetterPosition(_currentIndex);

                _cursor.Position = letterPos + (Vector2f)_text.GlobalPosition;
                _cursor.Size = new Vector2f(1, StyleManager.GetUint("font-size"));

                _cursorVisible = !_cursorVisible;
                _cursorTimer.Restart();
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && Focused)
            {
                int letterIndex = _text.GetLetterByPosition(Mouse.GetPosition(Form.Window));

                if (letterIndex >= 0)
                {
                    _currentIndex = letterIndex;
                }
            }

            UpdateTextView();
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);

            if (_cursorVisible && Focused)
                target.Draw(_cursor);
        }

        private void UpdateValue()
        {
            if (Password == true)
            {
                _text.Text = string.Empty;
                for (int i = 0; i < _realText.Length; i++)
                {
                    _text.Text += "•";
                }
            }
            else
            {
                _text.Text = _realText;
            }

            if (_realText.Length == 0)
            {
                _text.Text = Placeholder;
                _text.Style = "textbox_placeholder_default";
            }
            else
                _text.Style = string.Empty;

            if (_currentIndex > _realText.Length)
                _currentIndex = _realText.Length;
        }

        private void UpdateTextView()
        {
            int xOffset = (_textView.Bounds.Left + _textView.Bounds.Width) - (Bounds.Left + Bounds.Width - TargetPaddings.Horizontal);

            if (xOffset > 0)
            {
                _text.ScrollOffset = new Vector2i(-xOffset, 0);
            }
            else
            {
                _text.ScrollOffset = new Vector2i();
            }

            UpdateScrollOffset();

            _cursor.Position = _text.GetLetterPosition(_currentIndex) + (Vector2f)_text.GlobalPosition;
        }

        private void UpdateScrollOffset()
        {
            Vector2f letterPos = _cursor.Position - (Vector2f)_text.GlobalPosition;
            Vector2f localCursorPosition = _cursor.Position - (Vector2f)_textView.GlobalPosition;

            if (localCursorPosition.X > _textView.Size.X)
            {
                _textView.ScrollToX(-(int)(letterPos.X - _textView.Size.X));
            }
            if (localCursorPosition.X < 0)
            {
                _textView.ScrollToX(-(int)(letterPos.X));
            }
        }

        private void RemoveSelectedText()
        {
            if (_text.IsAnyTextSelected)
            {
                Range selection = _text.SelectionRange;
                _text.ResetSelection();

                _realText = _realText.Remove(selection.Min, selection.Max - selection.Min + 1);
                _currentIndex = selection.Min;
            }
        }
    }
}
