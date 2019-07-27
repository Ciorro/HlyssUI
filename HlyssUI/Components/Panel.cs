﻿using HlyssUI.Graphics;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Components
{
    public class Panel : Component
    {
        private RoundedRectangle _body;

        public Panel(GuiScene scene) : base(scene)
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
