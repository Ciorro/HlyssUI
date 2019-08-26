using HlyssUI.Themes;
using SFML.System;
using SFML.Window;
using System;

namespace HlyssUI.Components
{
    public class TextBox : Panel
    {
        private string _realText = string.Empty;
        private int _currentIndex;
        private Label _text;
        private bool _active;

        public bool AllowNumbers { get; set; } = true;
        public bool AllowLetters { get; set; } = true;
        public bool AllowSpecialCharacters { get; set; } = true;
        public bool Password { get; set; }

        public int MaxLines { get; set; } = 1;

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;

                StyleChanged = true;

                if (value)
                {
                    Style["secondary"] = Theme.GetColor("accent");
                    Style.BorderThickness = 2;
                }
                else
                {
                    Style["secondary"] = Theme.GetColor("secondary");
                    Style.BorderThickness = 1;
                }
            }
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            Padding = "11px";

            _text = new Label();
            _text.CoverParent = false;
            AddChild(_text);

            AutosizeY = true;
            DisableClipping = false;
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

            if(Active)
            {
                if (key == Keyboard.Key.Left && _currentIndex > 0)
                    _currentIndex--;
                else if (key == Keyboard.Key.Right && _currentIndex < _realText.Length) 
                    _currentIndex++;
            }
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
        }
    }
}
