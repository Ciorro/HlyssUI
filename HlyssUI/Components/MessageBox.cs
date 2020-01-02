using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class MessageBox : Dialog
    {
        public MessageBox()
        {
            Slot.Children = new List<Component>()
            {
                new TextArea()
                {
                    Width = "280px",
                    Height = "50px",
                    Text = "Czy na pewno chcesz opuścić program?"
                },
                new Component()
                {
                    Width = "280px",
                    AutosizeY = true,
                    Reversed = true,
                    Children = new List<Component>()
                    {
                        new Button()
                        {
                            Label = "TAK",
                            Appearance = Button.ButtonStyle.Flat,
                            //DefaultStyle = new Themes.Style()
                            //{
                            //    {"text-color", "accent" },
                            //    {"border-thickness", "0" }
                            //}
                        },
                        new Button()
                        {
                            Label = "NIE",
                            Appearance = Button.ButtonStyle.Flat
                        },
                        new Button()
                        {
                            Label = "ANULUJ",
                            Appearance = Button.ButtonStyle.Flat
                        }
                    }
                }
            };

            Slot.Layout = HlyssUI.Layout.LayoutType.Column;
        }
    }
}
