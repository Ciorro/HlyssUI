using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class Divider : Component
    {
        public enum DividerType
        {
            Horizontal, Vertical
        }

        private RectangleShape _divider = new RectangleShape();

        public DividerType Type
        {
            set
            {
                if(value == DividerType.Horizontal)
                {
                    Height = "1px";
                    Width = "100%";
                }
                else
                {
                    Height = "100%";
                    Width = "1px";
                }
            }
        }

        public Divider(DividerType type = DividerType.Horizontal)
        {
            Type = type;
            Style = "divider_default";
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _divider.Position = (Vector2f)GlobalPosition;
            _divider.Size = (Vector2f)Size;
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _divider.FillColor = StyleManager.GetColor("primary-color");
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);
            target.Draw(_divider);
        }
    }
}
