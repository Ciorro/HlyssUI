using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class Label : Component
    {
        public string Text
        {
            get { return _text.DisplayedString; }
            set
            {
                _text.DisplayedString = value;
                NeedsRefresh = true;
            }
        }

        public Font Font
        {
            get { return _text.Font; }
            set
            {
                _text.Font = value;
                NeedsRefresh = true;
            }
        }

        public uint CharacterSize
        {
            get { return _text.CharacterSize; }
            set
            {
                _text.CharacterSize = value;
                NeedsRefresh = true;
            }
        }

        public Text.Styles Style
        {
            get { return _text.Style; }
            set
            {
                _text.Style = value;
                NeedsRefresh = true;
            }
        }

        public bool Autosize
        {
            get { return _autosize; }
            set
            {
                _autosize = value;
                NeedsRefresh = true;
            }
        }

        private bool _autosize = true;
        private Text _text;

        public Label(GuiScene scene) : base(scene)
        {
            _text = new Text(string.Empty, scene.Gui.DefaultFont, scene.Gui.DefaultCharacterSize);
            updateSize();
        }

        public Label(GuiScene scene, string text) : base(scene)
        {
            _text = new Text(string.Empty, scene.Gui.DefaultFont, scene.Gui.DefaultCharacterSize);
            Text = text;
            updateSize();
        }

        public override void Update()
        {
            base.Update();

            if(Autosize && NeedsRefresh)
            {
                updateSize();
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _text.Position = (Vector2f)GlobalPosition;
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _text.FillColor = base.Style["Text"];
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_text);
        }

        private int getHeight()
        {
            int newLineCount = 0;
            foreach (var letter in _text.DisplayedString)
            {
                if (letter == '\n' || letter.ToString() == Environment.NewLine)
                    newLineCount++;
            }

            return (int)(_text.Font.GetLineSpacing(_text.CharacterSize) * (newLineCount + 1));
        }

        private void updateSize()
        {
            Width = $"{_text.GetGlobalBounds().Width}px";
            Height = $"{getHeight()}px";
        }
    }
}
