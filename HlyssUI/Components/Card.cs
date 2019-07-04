using HlyssUI.Graphics;
using HlyssUI.Layout;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class Card : Box
    {
        private RoundedRectangle _body;

        public Card(Gui gui) : base(gui)
        {
            _body = new RoundedRectangle();
            _body.FillColor = Colors.PrimaryColor;
            _body.OutlineColor = Colors.AccentColor;
            _body.OutlineThickness = -1;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _body.Position = (Vector2f)GlobalPosition;
            _body.Size = (Vector2f)Size;
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_body);
        }
    }
}
