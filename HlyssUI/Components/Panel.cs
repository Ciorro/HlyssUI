using HlyssUI.Graphics;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Components
{
    public class Panel : Component
    {
        private RoundedRectangle _body = new RoundedRectangle();

        public Texture Texture
        {
            get { return _body.Texture; }
            set { _body.Texture = value; }
        }

        public override void Update()
        {
            base.Update();
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
            _body.UpdateGeometry();
            target.Draw(_body);
        }
    }
}
