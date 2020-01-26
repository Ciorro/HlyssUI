using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Styling;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class Form : Component
    {
        private HlyssApp _internalApp;
        private bool _shouldClose = false;
        private string _caption = string.Empty;
        
        public RenderWindow Window { get; private set; }

        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                UpdateWindow();
            }
        }

        public bool IsOpen
        {
            get { return Window != null && Window.IsOpen; }
        }

        public Form()
        {
            Width = "550px";
            Height = "400px";

            ForceRefresh();
        }

        public void Show()
        {
            if (!IsOpen)
            {
                Window = new RenderWindow(new VideoMode((uint)W, (uint)H), Caption);
                Window.Closed += Window_Closed;
                Window.Resized += Window_Resized;
                Window.SetFramerateLimit(60);

                _internalApp = new HlyssApp(Window);
            }

            Visible = true;
        }

        public void Close()
        {
            if (IsOpen)
            {
                Window.Close();
                Window.Dispose();
                Window = null;
            }

            Visible = false;
        }

        public override void Update()
        {
            base.Update();

            if (IsOpen)
            {
                Window.Clear(Theme.GetColor("primary"));
                Window.DispatchEvents();

                _internalApp.Update();
                _internalApp.Draw();

                Window.Display();

                if (_shouldClose)
                {
                    Close();
                    _shouldClose = false;
                }
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            UpdateWindow();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _shouldClose = true;
        }

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            if (!AutosizeX && !AutosizeY)
            {
                Width = $"{e.Width}px";
                Height = $"{e.Height}px";
            }
        }

        private void UpdateWindow()
        {
            if(!IsOpen) return;

            Window.SetTitle(_caption);

            if (Window.Size != (Vector2u)Size)
                Window.Size = (Vector2u)Size;
        }
    }
}
