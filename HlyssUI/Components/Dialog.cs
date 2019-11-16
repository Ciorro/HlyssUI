using HlyssUI.Extensions;
using HlyssUI.Themes;
using SFML.System;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class Dialog : Flyout
    {
        private Component _originalParent;

        public Dialog()
        {
            Children = new List<Component>()
            {
                new Component()
                {
                    Width = "100%",
                    Padding = "10px",
                    AutosizeY = true,
                    Children = new List<Component>()
                    {
                        new Label()
                        {
                            Text = "Wyjście",
                            DefaultStyle = new Style()
                            {
                                {"character-size", "20" }
                            }
                        },
                        new Spacer(),
                        new Icon(Graphics.Icons.X)
                        {
                            IconSize = 20,
                            Name = "dialog_close",
                            HoverStyle = new Style()
                            {
                                {"text-color", "e81123" }
                            }
                        }
                    }
                },
                new Component()
                {
                    Autosize = true,
                    Padding = "10px",
                    Name = "dialog_content"
                }
            };

            Width = "300px";
            Layout = HlyssUI.Layout.LayoutType.Column;
            DisableClipping = false;

            DefaultStyle = new Style()
            {
                {"secondary-color", "accent" },
                {"primary-color", "primary" },
                {"size-ease", "out" }
            };

            FindChild("dialog_close").Clicked += (object sender) => Hide();

            SlotName = "dialog_content";
        }

        protected override void OnShown()
        {
            if (_originalParent == null)
                _originalParent = Parent;

            Component dialogScaffold = new Panel()
            {
                Width = $"{App.Root.W}px",
                Height = $"{App.Root.H}px",
                CenterContent = true,
                Layout = HlyssUI.Layout.LayoutType.Relative,
                PositionType = HlyssUI.Layout.Positioning.PositionType.Fixed,
                ReceiveStyle = false,
                DefaultStyle = Style.DefaultStyle.Combine(new Style()
                {
                    {"primary-color", "22000000" },
                    {"secondary-color", "22000000" }
                })
            };

            Parent.Children.Add(dialogScaffold);
            Reparent(dialogScaffold);

            AutosizeY = true;
        }

        protected override void OnHidden()
        {
            base.OnHidden();

            _originalParent.Children.Remove(Parent);
            Parent = null;
            Reparent(_originalParent);

            AutosizeY = false;
            Height = "0px";
        }
    }
}
