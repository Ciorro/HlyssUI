using HlyssUI.Components.Internals;
using HlyssUI.Themes;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace HlyssUI.Components
{
    public class TextBox : Panel
    {
        private RectangleShape _cursor = new RectangleShape();
        private Clock _cursorTimer = new Clock();
        private Component _textView;
        private EditableLabel _text;
        private bool _cursorVisible;
        private string _realText = string.Empty;
        private string _placeholder = string.Empty;
        private int _currentIndex;
        private bool _active;

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

        public bool Active
        {
            get { return _active; }
            set
            {
                if (value)
                {
                    Style.SetValue("secondary-color", "accent");
                    Style.SetValue("primary-color", "primary -10");
                    Style.SetValue("border-thickness", 2);

                    if (_realText.Length == 0)
                        _text.Text = string.Empty;
                    else if (SelectOnFocus && !_active)
                        _text.SelectAll();

                    if (!_active)
                        _currentIndex = _realText.Length;
                }
                else
                {
                    Style.SetValue("secondary-color", "secondary");
                    Style.SetValue("primary-color", "primary");
                    Style.SetValue("border-thickness", 1);

                    if (_realText.Length == 0)
                        _text.Text = Placeholder;
                }

                _active = value;
            }
        }

        public bool AllowNumbers { get; set; } = true;
        public bool AllowLetters { get; set; } = true;
        public bool AllowSpecialCharacters { get; set; } = true;
        public bool Password { get; set; }
        public bool SelectOnFocus { get; set; } = true;

        public int MaxLines { get; set; } = 1;

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            Padding = "10px";

            _textView = new Component();
            _textView.Autosize = true;
            _textView.DisableClipping = false;
            _textView.ClipArea.OutlineThickness = -2;
            _textView.CoverParent = false;
            AddChild(_textView);

            _text = new EditableLabel();
            _text.CoverParent = false;
            _textView.AddChild(_text);

            AutosizeY = true;
            DisableClipping = false;

            UpdateValue();
        }

        public override void OnClicked()
        {
            base.OnClicked();
            Active = true;
        }

        public override void OnMousePressedAnywhere(Vector2i location, Mouse.Button button)
        {
            base.OnMousePressedAnywhere(location, button);

            if (!Bounds.Contains(location.X, location.Y))
                Active = false;
        }

        public override void OnTextInput(string text)
        {
            base.OnTextInput(text);

            if (Active)
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
                    //OnTextChanged?.Invoke(this, _realText);
                }
            }
        }

        public override void OnKeyPressed(Keyboard.Key key)
        {
            base.OnKeyPressed(key);

            if (Active)
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
            Style.SetValue("primary-color", "primary -10");

            Gui.Window.SetMouseCursor(new Cursor(Cursor.CursorType.Text));
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();

            if (!Active)
                Style.SetValue("primary-color", "primary");

            Gui.Window.SetMouseCursor(new Cursor(Cursor.CursorType.Arrow));
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _cursor.FillColor = Style.GetColor("text-color");
        }

        public override void Update()
        {
            base.Update();

            if (_cursorTimer.ElapsedTime.AsMilliseconds() > 500)
            {
                _cursor.Position = _text.GetLetterPosition(_currentIndex) + (Vector2f)_text.GlobalPosition;
                _cursor.Size = new Vector2f(1, _text.CharacterSize);

                _cursorVisible = !_cursorVisible;
                _cursorTimer.Restart();
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && Active)
            {
                int letterIndex = _text.GetLetterByPosition(Mouse.GetPosition(Gui.Window));

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

            if (_cursorVisible && Active)
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
                _text.Style.SetValue("text-color", "808080");
                _text.Text = Placeholder;
            }
            else
                _text.Style.SetValue("text-color", "text");

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

            _cursor.Position = _text.GetLetterPosition(_currentIndex) + (Vector2f)_text.GlobalPosition;
        }

        private void RemoveSelectedText()
        {
            if (_text.IsAnyTextSelected)
            {
                Range selection = _text.SelectionRange;
                _text.SelectionRange = new Range(-1, -1);

                _realText = _realText.Remove(selection.Min, selection.Max - selection.Min + 1);
                _currentIndex = selection.Min;
            }
        }
    }
}
