using HlyssUI.Graphics;
using SFML.Graphics;
using SFML.System;
using System;

namespace HlyssUI.Components
{
    public class Icon : Component
    {
        private static Font _iconFont = new Font(Properties.Resources.Line_Awesome);
        private Text _iconTxt;
        private Icons _icon;

        public uint IconSize
        {
            get { return Style.CharacterSize; }
            set { Style.CharacterSize = value; }
        }

        public Icon(Icons icon)
        {
            _icon = icon;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            _iconTxt = new Text(((char)_icon).ToString(), _iconFont, Style.CharacterSize);
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
            _iconTxt.Position = new Vector2f(GlobalPosition.X + (TargetSize.X - _iconTxt.GetGlobalBounds().Width) / 2, GlobalPosition.Y);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _iconTxt.FillColor = Style["Text"];
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
