using HlyssUI.Layout;
using SFML.System;
using SFML.Window;

namespace HlyssUI.Components
{
    public class Flyout : Panel
    {
        public delegate void ShowHandler(object sender);
        public event ShowHandler Shown;

        public delegate void HideHandler(object sender);
        public event HideHandler Hidden;

        public bool CloseOnClickOutside { get; set; } = true;

        public Flyout()
        {
            Visible = false;
            Overflow = OverflowType.Hidden;
            PositionType = PositionType.Fixed;
        }

        public virtual void Show(Vector2i position)
        {
            Left = $"{position.X}px";
            Top = $"{position.Y}px";
            Visible = true;

            OnShown();
            Shown?.Invoke(this);
            FitInWindow();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            App.Window.Resized += (object sender, SizeEventArgs e) =>
            {
                FitInWindow();
                MaxWidth = $"{e.Width}px";
                MaxHeight = $"{e.Height}px";
            };
        }

        public void Hide()
        {
            Visible = false;

            OnHidden();
            Hidden?.Invoke(this);
        }

        public override void OnMousePressedAnywhere(Vector2i location, Mouse.Button button)
        {
            base.OnMousePressedAnywhere(location, button);

            if (!Hovered)
            {
                Hide();
            }
        }

        protected virtual void FitInWindow()
        {
            if (Visible)
            {
                if (TargetSize.Y < App.Root.TargetSize.Y)
                {
                    if (TargetPosition.Y + TargetSize.Y > App.Root.TargetSize.Y)
                    {
                        Top = $"{App.Root.TargetSize.Y - TargetSize.Y}px";
                        System.Console.WriteLine("1");
                    }
                    if (TargetPosition.Y < 0)
                    {
                        Top = "0px";
                        System.Console.WriteLine("2");
                    }
                }

                if (TargetSize.X < App.Root.TargetSize.X)
                {
                    if (TargetPosition.X + TargetSize.X > App.Root.TargetSize.X)
                    {
                        Left = $"{App.Root.TargetSize.X - TargetSize.X}px";
                        System.Console.WriteLine("3");
                    }
                    if (TargetPosition.X < 0)
                    {
                        Left = "0px";
                        System.Console.WriteLine("4");
                    }
                }
            }
        }

        protected virtual void OnShown() { }

        protected virtual void OnHidden() { }
    }
}
