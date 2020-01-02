using HlyssUI.Graphics;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using System;

namespace HlyssUI.Components
{
    public class Icon : Component
    {
        public uint IconSize
        {
            get { return StyleManager.GetUint("font-size"); }
            set { /*DefaultStyle = DefaultStyle.Combine(new Style() { { "font-size", value.ToString() } });*/ }
        }

        public Icons IconType
        {
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    _iconTxt.DisplayedString = ((char)_icon).ToString();
                    updateSize();
                }
            }
        }

        private Font _iconFont;
        private Text _iconTxt;
        private Icons _icon;

        public Icon(Icons icon)
        {
            _iconFont = new Font(Properties.Resources.Line_Awesome);
            _iconTxt = new Text(string.Empty, _iconFont, StyleManager.GetUint("font-size"));
            IconType = icon;
            updateSize();
        }

        public override void Update()
        {
            base.Update();

            if (TransformChanged)
            {
                updateSize();
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _iconTxt.Position = new Vector2f((int)(GlobalPosition.X + (TargetSize.X - _iconTxt.GetGlobalBounds().Width) / 2), GlobalPosition.Y);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _iconTxt.FillColor = StyleManager.GetColor("text-color");

            if (_iconTxt.CharacterSize != StyleManager.GetUint("font-size"))
            {
                _iconTxt.CharacterSize = StyleManager.GetUint("font-size");
                updateSize();
            }
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_iconTxt);
        }

        private int getHeight()
        {
            int newLineCount = 0;
            foreach (var letter in _iconTxt.DisplayedString)
            {
                if (letter == '\n' || letter.ToString() == Environment.NewLine)
                    newLineCount++;
            }

            return (int)(_iconTxt.Font.GetLineSpacing(_iconTxt.CharacterSize) * (newLineCount + 1));
        }

        private void updateSize()
        {
            int size = (int)Math.Max(_iconTxt.GetGlobalBounds().Width, getHeight());

            Width = $"{size}px";
            Height = $"{size}px";
        }
    }
}
