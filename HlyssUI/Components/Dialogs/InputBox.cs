using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components.Dialogs
{
    class InputBox : HlyssForm
    {
        public Action<object, string> ResultHandler;

        public string Message { get; set; }

        public InputBox(string title, string message)
        {
            Title = title;
            Size = new Vector2u(400, 150);
            WindowStyle = SFML.Window.Styles.Close;

            Message = message;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Root.Layout = Layout.LayoutType.Column;
            Root.Padding = "10px";

            Root.Children.AddRange(new List<Component>()
            {
                new TextArea()
                {
                    Text = Message,
                    Width = "100%",
                    Expand = true,
                    Margin = "5px 0px 0px 5px",
                    Name = "content"
                },
                new TextBox()
                {
                    Width = "100%",
                    Margin = "5px 0px",
                    Name = "input",
                    Focused = true
                },
                new Component()
                {
                    Name = "buttons",
                    ReversedHorizontal = true,
                    AutosizeY = true,
                    Width = "100%",
                    Children = new List<Component>()
                    {
                        new Button("Ok")
                        {
                            Appearance = Button.ButtonStyle.Filled,
                            Action = () =>
                            {
                                string text = (Root.FindChild("input") as TextBox).Text;

                                OnButtonClicked(text);
                                ResultHandler?.Invoke(this, text);
                                Hide();
                            }
                        }
                    }
                }
            });
        }

        protected override void OnShown()
        {
            (Root.FindChild("content") as TextArea).Text = Message;
            base.OnShown();
        }

        protected virtual void OnButtonClicked(string text) { }
    }
}
