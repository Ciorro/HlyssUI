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
            DisableClipping = false;
            OnTop = true;
        }

        public virtual void Show(Vector2i position)
        {
            Left = $"{position.X}px";
            Top = $"{position.Y}px";
            Visible = true;

            //Parent.ReorderChild(this, Parent.Children.Count);

            OnShown();
            Shown?.Invoke(this);
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

            if (!Bounds.Contains(location.X, location.Y) && CloseOnClickOutside)
            {
                Hide();
            }
        }

        public override void Update()
        {
            base.Update();
            
            if(Visible)
            {
                if (TargetSize.Y < App.Root.TargetSize.Y)
                {
                    if (TargetPosition.Y + TargetSize.Y > App.Root.TargetSize.Y)
                    {
                        Top = $"{App.Root.TargetSize.Y - TargetSize.Y}px";
                    }
                    if (TargetPosition.Y < 0)
                    {
                        Top = "0px";
                    }
                }

                if(TargetSize.X < App.Root.TargetSize.X)
                {
                    if (TargetPosition.X + TargetSize.X > App.Root.TargetSize.X)
                    {
                        Left = $"{App.Root.TargetSize.X - TargetSize.X}px";
                    }
                    if (TargetPosition.X < 0)
                    {
                        Left = "0px";
                    }
                }
            }
        }

        protected virtual void OnShown() { }

        protected virtual void OnHidden() { }
    }
}
