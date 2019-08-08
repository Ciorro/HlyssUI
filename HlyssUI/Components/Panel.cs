using HlyssUI.Graphics;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Components
{
    public class Panel : Component
    {
        private RoundedRectangle _body = new RoundedRectangle();

        public override void Update()
        {
            base.Update();
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            //System.Console.WriteLine(GlobalPosition);

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
