using HlyssUI.Graphics;
using SFML.Graphics;
using SFML.System;
using System;

namespace HlyssUI.Components
{
    public class Icon : Component
    {
        public Icons IconType
        {
            get { return _icon; }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    _iconTxt.DisplayedString = ((char)_icon).ToString();
                    UpdateGlyph();
                    UpdateSize();
                }
            }
        }

        private Font _iconFont;
        private Text _iconTxt;
        private Icons _icon;
        private Glyph _glyph;

        public Icon(Icons icon)
        {
            _iconFont = Fonts.LineAwesome;
            _iconTxt = new Text(string.Empty, _iconFont, StyleManager.GetUint("font-size"));
            IconType = icon;
            UpdateSize();
        }

        public override void Update()
        {
            base.Update();

            if (TransformChanged)
            {
                UpdateSize();
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _iconTxt.Position = new Vector2f((int)(GlobalPosition.X - (_glyph.Advance - _glyph.Bounds.Width) / 2 + (Size.X - _glyph.Bounds.Width) / 2), GlobalPosition.Y);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _iconTxt.FillColor = StyleManager.GetColor("text-color");
            uint charSize = StyleManager.GetUint("icon-size");

            if (_iconTxt.CharacterSize != charSize)
            {
                _iconTxt.CharacterSize = charSize;
                UpdateGlyph();
                UpdateSize();
            }
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_iconTxt);
        }

        private int GetHeight()
        {
            int newLineCount = 0;
            foreach (var letter in _iconTxt.DisplayedString)
            {
                if (letter == '\n' || letter.ToString() == Environment.NewLine)
                    newLineCount++;
            }

            return (int)(_iconTxt.Font.GetLineSpacing(_iconTxt.CharacterSize) * (newLineCount + 1));
        }

        private void UpdateSize()
        {
            int size = (int)(_glyph.Bounds.Width);

            Width = $"{GetHeight()}px";
            Height = $"{GetHeight()}px";
        }

        private void UpdateGlyph()
        {
            _glyph = _iconFont.GetGlyph(_iconTxt.DisplayedString[0], _iconTxt.CharacterSize, false, 0);
        }
    }
}
