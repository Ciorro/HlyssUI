using HlyssUI.Graphics;
using SFML.Graphics;
using SFML.System;

namespace HlyssUI.Components
{
    public class PictureBox : Component
    {
        public enum Stretch
        {
            Scale,
            Fill,
            Letterbox
        }

        public Texture Image
        {
            get { return _image.Texture; }
            set
            {
                _image.Texture = value;
                updateStretch();
            }
        }

        public string Source
        {
            set
            {
                Image = new Texture(value);
            }
        }

        public Color ImageColor
        {
            get { return _image.FillColor; }
            set { _image.FillColor = value; }
        }

        public bool SmoothImage
        {
            get { return _image.Texture.Smooth; }
            set { _image.Texture.Smooth = value; }
        }

        public Stretch StretchMode
        {
            get { return stretch; }
            set
            {
                stretch = value;
                updateStretch();
            }
        }

        private Stretch stretch = Stretch.Fill;
        private RoundedRectangle _image;

        public PictureBox()
        {
            create();
        }

        public PictureBox(string source)
        {
            create();
            Source = source;
            setDefaultSize();
        }

        public PictureBox(Texture texture)
        {
            create();
            Image = texture;
            setDefaultSize();
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            if (Visible)
            {
                _image.Position = (Vector2f)GlobalPosition;
                _image.Size = (Vector2f)TargetSize;

                updateStretch();
            }
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            
            _image.Radius = StyleManager.GetUint("border-radius");
        }

        public override void Draw(RenderTarget target)
        {
            _image.UpdateGeometry();
            target.Draw(_image);
        }

        private void updateStretch()
        {
            switch (StretchMode)
            {
                case Stretch.Scale:
                    scale();
                    break;
                case Stretch.Fill:
                    fill();
                    break;
                case Stretch.Letterbox:
                    letterbox();
                    break;
            }
        }

        private void fill()
        {
            FloatRect rect = (FloatRect)Bounds;

            float multW = _image.Texture.Size.X / rect.Width;

            rect.Width *= multW;
            rect.Height *= multW;

            if (rect.Height > _image.Texture.Size.Y)
            {
                float multH = _image.Texture.Size.Y / rect.Height;

                rect.Width *= multH;
                rect.Height *= multH;
            }

            rect.Left = (_image.Texture.Size.X - rect.Width) / 2;
            rect.Top = (_image.Texture.Size.Y - rect.Height) / 2;

            _image.TextureRect = (IntRect)rect;
        }

        private void letterbox()
        {
            this.scale();

            float scale = (float)TargetSize.X / _image.Texture.Size.X;
            _image.Size = (Vector2f)_image.Texture.Size;
            _image.Scale = new Vector2f(scale, scale);

            if (_image.GetGlobalBounds().Height > TargetSize.Y)
            {
                scale = (float)TargetSize.Y / _image.Texture.Size.Y;
                _image.Size = (Vector2f)_image.Texture.Size;
                _image.Scale = new Vector2f(scale, scale);
            }

            _image.Position = (Vector2f)(GlobalPosition + new Vector2i((int)(TargetSize.X - _image.GetGlobalBounds().Width) / 2, (int)(TargetSize.Y - _image.GetGlobalBounds().Height) / 2));
        }

        private void scale()
        {
            _image.TextureRect = new IntRect(new Vector2i(), (Vector2i)_image.Texture.Size);
        }

        private void create()
        {
            _image = new RoundedRectangle();
        }

        private void setDefaultSize()
        {
            Width = $"{Image.Size.X}px";
            Height = $"{Image.Size.Y}px";
        }

        ~PictureBox()
        {
            _image.Dispose();
        }
    }
}
