using HlyssUI.Graphics;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Components
{
    public class Panel : Component
    {
        public Texture Texture
        {
            get { return _body.Texture; }
            set { _body.Texture = value; }
        }

        private RoundedRectangle _body = new RoundedRectangle();

        public override void OnRefresh()
        {
            base.OnRefresh();

            _body.Position = (Vector2f)GlobalPosition;
            _body.Size = (Vector2f)Size;
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();

            _body.FillColor = Style.GetColor("primary-color");
            _body.OutlineColor = Style.GetColor("secondary-color");

            _body.OutlineThickness = Style.GetUint("border-thickness") * -1;
            _body.Radius = Style.GetUint("border-radius");
        }

        public override void Draw(RenderTarget target)
        {
            _body.UpdateGeometry();
            target.Draw(_body);
        }
    }
}
