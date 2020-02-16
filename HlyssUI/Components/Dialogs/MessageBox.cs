using SFML.System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HlyssUI.Components.Dialogs
{
    public class MessageBox : HlyssForm
    {
        public Action<object, int> ResultHandler;

        private string[] _buttons;
        public string Content { get; set; }

        public MessageBox(string title, string content, params string[] buttons)
        {
            Title = title;
            Size = new Vector2u(400, 150);
            WindowStyle = SFML.Window.Styles.Close;

            Content = content;
            _buttons = buttons;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            Root.Layout = Layout.LayoutType.Column;
            Root.Padding = "10px";

            Root.Children.AddRange(new List<Component>()
            {
                new Component()
                {
                    Width = "100%",
                    Expand = true,
                    //Overflow = Layout.OverflowType.Hidden,
                    Children = new List<Component>()
                    {
                        new Icon(Graphics.Icons.Warning)
                        {
                            Style = "mbox_icon_default",
                            Margin = "0px 5px"
                        },
                        new TextArea()
                        {
                            Text = Content,
                            Height = "100%",
                            Expand = true,
                            Margin = "5px 0px 0px 5px",
                            Name = "content"
                        },
                    }
                },
                new Component()
                {
                    Name = "buttons",
                    ReversedHorizontal = true,
                    AutosizeY = true,
                    Width = "100%",
                }
            });

            for (int i = _buttons.Length - 1; i >= 0; i--)
            {
                int index = i;

                Root.GetChild("buttons").Children.Add(new Button(_buttons[i])
                {
                    Name = $"messagebox_button_{i}",
                    MarginLeft = "5px",
                    Action = () =>
                    {
                        ResultHandler?.Invoke(this, index);
                        OnButtonClicked(index);
                        Hide();
                    }
                });
            }
        }

        protected override void OnShown()
        {
            (Root.FindChild("content") as TextArea).Text = Content;
            base.OnShown();
        }

        protected virtual void OnButtonClicked(int index) { }
    }
}
