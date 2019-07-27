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

        public Card(GuiScene scene) : base(scene)
        {
            _body = new RoundedRectangle();
            _body.FillColor = Style["Primary"];
            _body.OutlineColor = Style["Secondary"];
            _body.OutlineThickness = -1;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _body.Position = (Vector2f)GlobalPosition;
            _body.Size = (Vector2f)Size;
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _body.FillColor = Style["Primary"];
            _body.OutlineColor = Style["Secondary"];

            _body.OutlineThickness = Style.BorderThickness * -1;
            _body.Radius = Style.BorderRadius;
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_body);
        }
    }
}
