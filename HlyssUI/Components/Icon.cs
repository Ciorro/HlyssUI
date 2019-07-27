using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class Icon : Component
    {
        private static Font _iconFont = new Font(HlyssUI.Properties.Resources.Line_Awesome);
        private Text _icon;

        public Icon(GuiScene scene, Icons icon) : base(scene)
        {
            _icon = new Text(((char)icon).ToString(), _iconFont, scene.Gui.DefaultCharacterSize);
            updateSize();
        }

        public override void Update()
        {
            base.Update();

            if (NeedsRefresh)
            {
                updateSize();
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _icon.Position = new Vector2f(GlobalPosition.X + (Size.X - _icon.GetGlobalBounds().Width) / 2, GlobalPosition.Y);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _icon.FillColor = Style["Text"];
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_icon);
        }

        private int getHeight()
        {
            int newLineCount = 0;
            foreach (var letter in _icon.DisplayedString)
            {
                if (letter == '\n' || letter.ToString() == Environment.NewLine)
                    newLineCount++;
            }

            return (int)(_icon.Font.GetLineSpacing(_icon.CharacterSize) * (newLineCount + 1));
        }

        private void updateSize()
        {
            int size = (int)Math.Max(_icon.GetGlobalBounds().Width, getHeight());

            Width = $"{size}px";
            Height = $"{size}px";
        }
    }
}
