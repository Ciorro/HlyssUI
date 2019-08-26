using HlyssUI.Components.Internals;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace HlyssUI.Components
{
    public class TextBox : Panel
    {
        private RectangleShape _cursor = new RectangleShape();
        private Clock _cursorTimer = new Clock();
        private EditableLabel _text;
        private bool _cursorVisible;
        private string _realText = string.Empty;
        private string _placeholder = string.Empty;
        private int _currentIndex;
        private bool _active;

        public bool AllowNumbers { get; set; } = true;
        public bool AllowLetters { get; set; } = true;
        public bool AllowSpecialCharacters { get; set; } = true;
        public bool Password { get; set; }

        public int MaxLines { get; set; } = 1;

        public string Placeholder
        {
            get { return _placeholder; }
            set
            {
                _placeholder = value;
                updateValue();
            }
        }

        public string Text
        {
            get { return _realText; }
            set
            {
                _realText = value;
                updateValue();
            }
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;

                if (value)
                {
                    Style["secondary"] = Theme.GetColor("accent");
                    Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 10);
                    Style.BorderThickness = 2;

                    if (_realText.Length == 0)
                        _text.Text = string.Empty;
                }
                else
                {
                    Style["secondary"] = Theme.GetColor("secondary");
                    Style["primary"] = Theme.GetColor("primary");
                    Style.BorderThickness = 1;

                    if (_realText.Length == 0)
                        _text.Text = Placeholder;
                }
            }
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            Padding = "11px";

            _text = new EditableLabel();
            _text.CoverParent = false;
            AddChild(_text);

            AutosizeY = true;
            DisableClipping = false;

            updateValue();
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
                    if (_realText.Length > 0 && _currentIndex > 0)
                    {
                        _realText = _realText.Remove(_currentIndex - 1, 1);
                        _currentIndex--;
                    }
                }
                else if (c == 13 && _text.Lines < MaxLines)
                {
                    _realText = _realText.Insert(_currentIndex, "\n");
                    _currentIndex++;
                }
                else if (!char.IsControl(c))
                {
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

                updateValue();

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
                    _cursor.Position = _text.GetLetterPosition((uint)_currentIndex) + (Vector2f)_text.GlobalPosition;
                }
                else if (key == Keyboard.Key.Right && _currentIndex < _realText.Length)
                {
                    _currentIndex++;
                    _cursor.Position = _text.GetLetterPosition((uint)_currentIndex) + (Vector2f)_text.GlobalPosition;
                }
            }
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 10);
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();

            if (!Active)
                Style["primary"] = Theme.GetColor("primary");
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _cursor.FillColor = Style["text"];
        }

        public override void Update()
        {
            base.Update();

            if (_cursorTimer.ElapsedTime.AsMilliseconds() > 500)
            {
                _cursor.Position = _text.GetLetterPosition((uint)_currentIndex) + (Vector2f)_text.GlobalPosition;
                _cursor.Size = new Vector2f(1, _text.CharacterSize);

                _cursorVisible = !_cursorVisible;
                _cursorTimer.Restart();
            }
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);

            if (_cursorVisible && Active)
                target.Draw(_cursor);
        }

        private void updateValue()
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

            _cursor.Position = _text.GetLetterPosition((uint)_currentIndex) + (Vector2f)_text.GlobalPosition;

            if (_realText.Length == 0)
            {
                _text.Style["text"] = Theme.GetColor("808080");
                _text.Text = Placeholder;
            }
            else
                _text.Style["text"] = Theme.GetColor("text");
        }
    }
}
