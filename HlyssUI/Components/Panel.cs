using HlyssUI.Graphics;
using HlyssUI.Layout;
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

            _body.FillColor = StyleManager.GetColor("primary-color");
            _body.OutlineColor = StyleManager.GetColor("secondary-color");

            _body.OutlineThickness = StyleManager.GetUint("border-thickness") * -1;
            _body.BorderRadius = new BorderRadius(StyleManager.GetString("border-radius"));
        }

        public override void Draw(RenderTarget target)
        {
            _body.UpdateGeometry();
            target.Draw(_body);
        }
    }
}
