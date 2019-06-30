using HlyssUI.Graphics;
using HlyssUI.Utils;
using SFML.System;

namespace HlyssUI.Components
{
    public class Panel : Component
    {
        private RoundedRectangle _body;

        public Panel(Gui gui) : base(gui)
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

        public override void Draw()
        {
            Gui.Window.Draw(_body);
        }
    }
}
