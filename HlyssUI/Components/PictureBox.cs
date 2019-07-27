using HlyssUI.Graphics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

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
        private RectangleShape _background;
        private RoundedRectangle _image;

        public PictureBox(GuiScene scene) : base(scene)
        {
            create();
        }

        public PictureBox(GuiScene scene, string source) : base(scene)
        {
            create();
            Source = source;
            setDefaultSize();
        }

        public PictureBox(GuiScene scene, Texture texture) : base(scene)
        {
            create();
            Image = texture;
            setDefaultSize();
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            _background.FillColor = Style["primary"];
            _background.Position = (Vector2f)GlobalPosition;
            _background.Size = (Vector2f)Size;

            _image.Position = (Vector2f)GlobalPosition;
            _image.Size = (Vector2f)Size;

            updateStretch();
        }

        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            _image.Radius = Style.BorderRadius;
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(_background);
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

            float scale = (float)Size.X / _image.Texture.Size.X;
            _image.Size = (Vector2f)_image.Texture.Size;
            _image.Scale = new Vector2f(scale, scale);

            if (_image.GetGlobalBounds().Height > Size.Y)
            {
                scale = (float)Size.Y / _image.Texture.Size.Y;
                _image.Size = (Vector2f)_image.Texture.Size;
                _image.Scale = new Vector2f(scale, scale);
            }

            _image.Position = (Vector2f)(GlobalPosition + new Vector2i((int)(Size.X - _image.GetGlobalBounds().Width) / 2, (int)(Size.Y - _image.GetGlobalBounds().Height) / 2));
        }

        private void scale()
        {
            _image.TextureRect = new IntRect(new Vector2i(), (Vector2i)_image.Texture.Size);
        }

        private void create()
        {
            _background = new RectangleShape();
            _background.FillColor = Color.Transparent;

            _image = new RoundedRectangle();
        }

        private void setDefaultSize()
        {
            Width = $"{Image.Size.X}px";
            Height = $"{Image.Size.Y}px";
        }
    }
}
