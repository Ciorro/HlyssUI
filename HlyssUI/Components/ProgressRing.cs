using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class ProgressRing : Component
    {
        public enum ProgressRingSize
        {
            ExtraSmall, Small, Medium, Large, ExtraLarge
        }

        private Sprite _spinner;
        private int _rotation = 0;
        private DeltaTime _deltaTime = new DeltaTime();

        public ProgressRingSize RingSize
        {
            set
            {
                int size = value switch
                {
                    ProgressRingSize.ExtraSmall => 8,
                    ProgressRingSize.Small => 16,
                    ProgressRingSize.Medium => 32,
                    ProgressRingSize.Large => 48,
                    ProgressRingSize.ExtraLarge => 64,
                    _ => 16
                };

                Width = $"{size}px";
                Height = $"{size}px";

                _spinner.Scale = new Vector2f(size / 64f, size / 64f);
            }
        }

        public ProgressRing()
        {
            _spinner = new Sprite(new Texture(Properties.Resources.Progress_Ring));
            _spinner.Origin = new Vector2f(32, 32);
            _spinner.Texture.Smooth = true;

            RingSize = ProgressRingSize.Medium;
        }

        public override void Update()
        {
            base.Update();

            _deltaTime.Update();

            _spinner.Rotation = _rotation;
            _rotation += (int)(500 * _deltaTime.Current);
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _spinner.Position = (Vector2f)(GlobalPosition + Size / 2);
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _spinner.Color = StyleManager.GetColor("accent-color");
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);
            target.Draw(_spinner);
        }
    }
}
